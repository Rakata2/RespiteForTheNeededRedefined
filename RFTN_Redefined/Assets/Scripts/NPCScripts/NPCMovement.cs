using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Security.Cryptography;
using Unity.VisualScripting;


public class NPCMovement : MonoBehaviour
{
    public Transform SpawnPoint;
    public Transform CenterPoint;
    public Transform ExitPointFood;
    public Transform ExitPointShelter;
    public Transform ExitPointShelterFailed;

    private Transform ChosenExit;

    public Button NextButton;

    public float speed = 5f;

    public GameObject ChatBubble;
    public TMP_Text DialogueText;
    public float TypingSpeed = 0.01f;

    [Header("Databases")]


    public ShelterDialogueDatabase ShelterDialogueDB;
    public NPCResponse NPCResponseDB;

    public RequestType NPCRequestType;
    private Animator BellAnimator;
    
    public enum NPCState
    {
        MovingToCenter,
        Interact,
        WaitingForDecision,
        Finished,
        MovingToExit
    }

    public enum RequestType
    {
        Shelter,
        Medical,
        Isolation,
        Behavioral
    }

    //[NEW] Reaction
    public enum LeaveReaction
    {
        Accepted,
        RejectedCorrectly,
        RejectIncorrectly,
        CaughtFakeDocument
    }

    public NPCState CurrentState = NPCState.MovingToCenter;

    public static NPCMovement CurrentClient;

    
    public IdentityProfile ChosenID;

    public bool HasID;
    public bool PhysicalIDIsGovIssued;
    public bool PhysicalApplicationIsGovIssued;
    public bool HasLetter;
    public bool HasApplication;
    public bool AppCircle;
    public int CheckReasonIndex; //0 = eviction, 1= family, 2= job
    public bool PhysicalLetterIsGovIssued;

    

    private SpriteRenderer NPCSpriteRenderer;
    private Sprite NormalSprite;
    public Sprite HighlitedSprite;

    public Sprite[] AllGameFaces;
    public bool IsFaceMissmatch;
    private Sprite FaceOnIDCard;

    public GameObject ActionPanel;

    public bool IsHospitalized;
    public int DatabaseExcuseChoice;

    public bool IsLeaving = false;
    public bool IsSuccessExit = false;

    public bool IsTyping = false;
    private Coroutine TypingCoroutine;
    //private string CurrentFullSentence;
    private bool AcceptedByPlayer = false;

    public AudioSource WalkingSoundEffect;
    public float StepInterval = 0.4f;
    private bool IsWalking = false;
    private Coroutine FootStepCoroutine;

    public bool IsTrulyValid;

    private List<string> AskedTopics = new List<string>();

    private bool PenaltyRepeated = false;

    public List<IdentityProfile> AllNPCProfiles;
    public bool IsNameMissmatch;
    public bool IsDOBMissmatch;
    public string DisplayedName;
    public string DisplayedNickName;
    public string DisplayedDOB;


    public bool HasTrayMechanic;

    public List<ItemList.ItemEntry> AllowedItems = new List<ItemList.ItemEntry>();
    public List<ItemList.ItemEntry> RejectedItems = new List<ItemList.ItemEntry>();

    //NEW CODE HERE
    public ItemList MasterItemList;
    public List<ItemList.ItemEntry> CurrentNPCItems = new List<ItemList.ItemEntry>();

    void Awake()
    {
        NPCSpriteRenderer = GetComponent<SpriteRenderer>();
        NormalSprite = NPCSpriteRenderer.sprite;
    }

    void Start()
    {
        CheckReasonIndex = Random.Range(0, 3);

        bool IsApplicationNPC = (Random.value > 0.5f);
        FaceOnIDCard = ChosenID.Photo;
        IsFaceMissmatch = false;
        DatabaseExcuseChoice = Random.Range(0, 2);

        if (GameUIManager.instance != null && GameUIManager.instance.ActionPanel != null)
        {
            ActionPanel = GameUIManager.instance.ActionPanel.gameObject;
        }

        if(IsHospitalized)
        {
            HasID = true;
            PhysicalIDIsGovIssued = true;
            IsTrulyValid = true;

            if(IsApplicationNPC)
            {
                HasLetter = false;
                HasApplication = true;
                PhysicalApplicationIsGovIssued = true;
                AppCircle = true;
            }
            else
            {
                HasApplication = false;
                HasLetter = true;
                PhysicalLetterIsGovIssued = true;
            }

            if(Random.Range(1, 101) <= 20)
            {
                IsFaceMissmatch = true;
                IsTrulyValid = false;
                FaceOnIDCard = AllGameFaces[Random.Range(0, AllGameFaces.Length)];
                while(FaceOnIDCard == ChosenID.Photo)
                {
                    FaceOnIDCard = AllGameFaces[Random.Range(0, AllGameFaces.Length)];
                }
            }
        }
        else
        {
            int ValidNPCS = 70;
            bool IsValidNPC = (Random.Range(1, 101) <= ValidNPCS);
            IsTrulyValid = IsValidNPC;
            if(IsValidNPC)
            {
                if(IsApplicationNPC)
                {
                    HasLetter = false;
                    HasApplication = true;
                    PhysicalApplicationIsGovIssued = true;
                    if(Random.value > 0.5f)
                    {
                        AppCircle = true;
                        HasID = true;
                        PhysicalIDIsGovIssued = true;
                    }
                    else
                    {
                        AppCircle = false;
                        HasID = false;
                        PhysicalIDIsGovIssued = false;
                    }
                }
                else
                {
                    HasApplication = false;
                    HasLetter = true;
                    PhysicalLetterIsGovIssued = true;
                    HasID = true;
                    PhysicalIDIsGovIssued = true;
                }
                IsFaceMissmatch = false;
            }
            else
            {
                int ViolationType = Random.Range(0, 3);

                if(IsApplicationNPC)
                {
                    HasLetter = false;
                    HasApplication = true;
                    PhysicalApplicationIsGovIssued = true;

                    if(ViolationType == 0)
                    {
                        AppCircle = true;
                        HasID = false;
                        PhysicalIDIsGovIssued = false;
                    }
                    else if(ViolationType == 1)
                    {
                        AppCircle = true;
                        HasID = true;
                        PhysicalIDIsGovIssued= false;
                    }
                    else
                    {
                        AppCircle = true;
                        HasID = true;
                        PhysicalIDIsGovIssued = true;

                        IsFaceMissmatch = true;
                        FaceOnIDCard = AllGameFaces[Random.Range(0, AllGameFaces.Length)];
                        while(FaceOnIDCard == ChosenID.Photo)
                        {
                            FaceOnIDCard = AllGameFaces[Random.Range(0, AllGameFaces.Length)];
                        }
                    }
                }
                else
                {
                    HasApplication = false;
                    HasLetter = true;

                    if(ViolationType == 0)
                    {
                        PhysicalLetterIsGovIssued = false;
                        HasID = true;
                        PhysicalIDIsGovIssued = true;
                    }
                    else if(ViolationType == 1)
                    {
                        PhysicalLetterIsGovIssued = true;
                        HasID = (Random.value > 0.5f);
                        PhysicalIDIsGovIssued = false;
                    }
                    else
                    {
                        PhysicalLetterIsGovIssued = true;
                        HasID = true;
                        PhysicalIDIsGovIssued = true;
                        IsFaceMissmatch = true;
                        FaceOnIDCard = AllGameFaces[Random.Range(0, AllGameFaces.Length)];
                        while(FaceOnIDCard == ChosenID.Photo)
                        {
                            FaceOnIDCard = AllGameFaces[Random.Range(0, AllGameFaces.Length)];
                        }
                    }
                }
            }
        }
        ApplyDocumentFakes();
        StartFootstep();
    }
    void Update()
    {
        switch(CurrentState)
        {
            case NPCState.MovingToCenter:
                MoveTo(CenterPoint);
                if (IsAtPosition(CenterPoint))
                {
                    StopFootstep();
                    CurrentState = NPCState.Interact;

                    //if(ChatBubble != null) ChatBubble.SetActive(true);
                    StartCoroutine(StartInteraction());
                }
                break;
            case NPCState.Interact:
                
                break;
            case NPCState.WaitingForDecision:
                
                break;

            case NPCState.MovingToExit:
                MoveTo(ChosenExit);
                if (IsAtPosition(ChosenExit))
                {
                    StopFootstep();
                    if(ObjectiveManager.instance != null)
                    {
                        ObjectiveManager.instance.EvaluatePlayerDecision(AcceptedByPlayer, IsTrulyValid);
                    }
                    Destroy(gameObject);
                }
                break;
        }
    }
    
    void MoveTo(Transform target)
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    bool IsAtPosition(Transform target)
    {
        return Vector2.Distance(transform.position, target.position) < 0.1f;
    }

    public void TriggerInterrogation(string ResponseText)
    {
        StartCoroutine(InterrogationRoutine(ResponseText));
    }
    
    //[NEW] triggers reaction
    public void TriggerReaction(LeaveReaction Reaction)
    {
        StartCoroutine(LeaveRoutine(Reaction));
    }

    private void ApplyDocumentFakes()
    {
        DisplayedName = ChosenID.Name;
        DisplayedDOB = ChosenID.DateOfBirth;
        DisplayedNickName = ChosenID.NickName;

        if(IsFaceMissmatch == true)
        {
            IsFaceMissmatch = false;
            FaceOnIDCard = ChosenID.Photo;
            int FakeType = Random.Range(0, 3);

            if(FakeType == 0)
            {
                IsFaceMissmatch = true;
                FaceOnIDCard = AllGameFaces[Random.Range(0, AllGameFaces.Length)];
                while(FaceOnIDCard == ChosenID.Photo)
                {
                    FaceOnIDCard = AllGameFaces[Random.Range(0, AllGameFaces.Length)];
                }
            }
            else if(FakeType == 1)
            {
                IsNameMissmatch = true;
                if(AllNPCProfiles != null && AllNPCProfiles.Count > 1)
                {
                    IdentityProfile FakeProfile = AllNPCProfiles[Random.Range(0, AllNPCProfiles.Count)];
                    while (FakeProfile == ChosenID)
                    {
                        FakeProfile = AllNPCProfiles[Random.Range(0, AllNPCProfiles.Count)];
                    }
                    DisplayedName = FakeProfile.Name;
                    DisplayedNickName = FakeProfile.NickName;
                }
            }
            else if (FakeType == 2)
            {
                IsDOBMissmatch = true;
                if (AllNPCProfiles != null && AllNPCProfiles.Count > 1)
                {
                    IdentityProfile FakeProfile = AllNPCProfiles[Random.Range(0, AllNPCProfiles.Count)];
                    while (FakeProfile == ChosenID)
                    {
                        FakeProfile = AllNPCProfiles[Random.Range(0, AllNPCProfiles.Count)];
                    }
                    DisplayedDOB = FakeProfile.DateOfBirth;
                }
            }
        }
    }

    IEnumerator StartInteraction()
    {
        CurrentClient = this;
        if (IsShelterType() && GameUIManager.instance.DeskCard != null && ChosenID != null)
        {
            if (HasID)
            {
                GameUIManager.instance.DeskCard.GetComponent<DocumentAnimator>().ShowDocument();
                GameUIManager.instance.DeskCard.ReceiveID(ChosenID, PhysicalIDIsGovIssued, FaceOnIDCard, DisplayedName, DisplayedDOB);
            }
            else
            {
                GameUIManager.instance.DeskCard.gameObject.SetActive(false);
            }
        }

        if (IsShelterType() && GameUIManager.instance.DeskLetter != null && ChosenID != null)
        {
            if (HasLetter)
            {
                GameUIManager.instance.DeskLetter.GetComponent<DocumentAnimator>().ShowDocument();
                GameUIManager.instance.DeskLetter.ReceiveLetterData(ChosenID, PhysicalLetterIsGovIssued, DisplayedNickName);
            }
            else
            {
                GameUIManager.instance.DeskLetter.gameObject.SetActive(false);
            }

        }

        if (IsShelterType() && GameUIManager.instance.DeskApplication != null && ChosenID != null)
        {
            if (HasApplication)
            {
                GameUIManager.instance.DeskApplication.GetComponent<DocumentAnimator>().ShowDocument();
                GameUIManager.instance.DeskApplication.ReceiveApplicationData(ChosenID, PhysicalApplicationIsGovIssued, CheckReasonIndex, AppCircle, DisplayedName, DisplayedDOB);
            }
            else
            {
                GameUIManager.instance.DeskApplication.gameObject.SetActive(false);
            }
        }

        if(HasTrayMechanic)
        {
            yield return new WaitForSeconds(0.3f);
            SpawnTrayItems();
        }
        yield return new WaitForSeconds(0.3f);


        if (ChatBubble != null) ChatBubble.SetActive(true);


        if (BellBridge.instance != null)
        {
            BellBridge.instance.SetTrigger("RingBell");
        }
        else
        {
            Debug.LogError("Bellbridge missing from scene");
        }
        GetComponent<AudioSource>().Play();
        GameUIManager.instance.SetDialogueActive(true);
        Debug.Log("Interaction type: " + NPCRequestType);

        List<string> SelectedList = GetListByType(NPCRequestType);
        string ChosenText = SelectedList[Random.Range(0, SelectedList.Count)];
        DialogueText.text = "";

        if (NextButton != null) NextButton.gameObject.SetActive(false);

        foreach (char letter in ChosenText.ToCharArray())
        {
            DialogueText.text += letter;
            yield return new WaitForSeconds(TypingSpeed);
        }
        if (NextButton != null)
        {
            NextButton.gameObject.SetActive(true);
        }
        CurrentState = NPCState.WaitingForDecision;
    }

    IEnumerator InterrogationRoutine(string ResponseText)
    {
        if (ActionPanel != null) ActionPanel.SetActive(false);
        GameUIManager.instance.SetDialogueActive(true);
        if (ChatBubble != null) ShowChatBubble();
        if (NextButton != null) NextButton.gameObject.SetActive(false);
        DialogueText.text = "";

        if (NextButton != null) NextButton.gameObject.SetActive(false);

        foreach (char letter in ResponseText.ToCharArray())
        {
            DialogueText.text += letter;
            yield return new WaitForSeconds(TypingSpeed);
        }
        if (NextButton != null)
        {
            NextButton.gameObject.SetActive(true);
        }
    }

    //[NEW] coroutine for NPC reactions
    IEnumerator LeaveRoutine(LeaveReaction Reaction)
    {
        string ChosenText = "...";
        IsLeaving = true;
        AcceptedByPlayer = (Reaction == LeaveReaction.Accepted);

        switch (Reaction)
        {
            case LeaveReaction.Accepted:
                IsSuccessExit = true;
                if (HasTrayMechanic)
                {
                    ChosenText = PickRandomResponse(NPCResponseDB.Level3ThankYouResponse);
                }
                else
                {
                    ChosenText = PickRandomResponse(NPCResponseDB.Accept);
                }
                break;
            case LeaveReaction.CaughtFakeDocument:
                ChosenText = PickRandomResponse(NPCResponseDB.QuestionFakeID);
                GameUIManager.instance.HideAllItems();
                IsSuccessExit = false;
                break;
            case LeaveReaction.RejectedCorrectly:
                if (GameUIManager.instance != null)
                {
                    GameUIManager.instance.HideAllItems();
                    GameUIManager.instance.HideAllDocuments();
                }
                if(IsFaceMissmatch == true || IsNameMissmatch == true || IsDOBMissmatch == true)
                {
                    ChosenText = PickRandomResponse(NPCResponseDB.ThankYouResponseFake);
                    if (GameUIManager.instance != null) GameUIManager.instance.ShowEmptyApplication();
                }
                else
                {
                    ChosenText = PickRandomResponse(NPCResponseDB.RejectApplicationConfirmed);
                    if (GameUIManager.instance != null) GameUIManager.instance.ShowEmptyApplication();
                }
                IsSuccessExit = false;
                break;
            case LeaveReaction.RejectIncorrectly:
                ChosenText = PickRandomResponse(NPCResponseDB.RejectSecondComplete);
                GameUIManager.instance.HideAllItems();
                IsSuccessExit = false;
                break;
        }
        
        if (ActionPanel != null) ActionPanel.SetActive(false);
        GameUIManager.instance.SetDialogueActive(true);
        if (ChatBubble != null) ShowChatBubble();
        DialogueText.text = "";

        if (NextButton != null) NextButton.gameObject.SetActive(false);

        foreach (char letter in ChosenText.ToCharArray())
        {
            DialogueText.text += letter;
            yield return new WaitForSeconds(TypingSpeed);
        }
        if (NextButton != null)
        {
            NextButton.gameObject.SetActive(true);
        }
        yield return new WaitUntil(() => NextButton == null || !NextButton.gameObject.activeInHierarchy);
        if (Reaction == LeaveReaction.Accepted && HasTrayMechanic)
        {
            GameUIManager.instance.SetDialogueActive(false);
            if(ChatBubble != null) ChatBubble.SetActive(false);
            if(GameUIManager.instance != null && GameUIManager.instance.TrayPanelManagerScript != null)
            {
                GameUIManager.instance.TrayPanelManagerScript.PrepareTrayItem(CurrentNPCItems);
                GameUIManager.instance.OpenTray();
            }
            yield break;
        }

        yield return new WaitForSeconds(0.3f);

        if (GameUIManager.instance != null)
        {
            GameUIManager.instance.HideAllDocuments();
        }

        yield return new WaitForSeconds(0.3f);

        if (ObjectiveManager.instance != null & AcceptedByPlayer)
        {
            ObjectiveManager.instance.TotalAdmitted++;
            if(ObjectiveManager.instance.TotalAdmitted >= ObjectiveManager.instance.TargetObjective)
            {
                if(GameUIManager.instance != null)
                {
                    GameUIManager.instance.LockGame();
                }
            }
        }

        if(Reaction == LeaveReaction.RejectedCorrectly)
        {
            if(GameUIManager.instance != null)
            {
                GameUIManager.instance.HideEmptyApplication();
            }

            yield return new WaitForSecondsRealtime(0.4f);
        }
        StartLeaving(IsSuccessExit);
    }

    
    List<string> GetListByType(RequestType type)
    {
        switch (type)
        {
            case RequestType.Shelter:
                return ShelterDialogueDB.ShelterDialogues;
            case RequestType.Medical:
                return ShelterDialogueDB.ShelterMedicalNeeds;
            case RequestType.Isolation:
                return ShelterDialogueDB.ShelterIsolationNeeds;
            case RequestType.Behavioral:
                return ShelterDialogueDB.ShelterBehavioralNeeds;
            default:
                return ShelterDialogueDB.ShelterDialogues; //MIND THIS PLEASE MEOW MEOW MEOW MEOW MEOW MEOW MEOW MEOW MEOW 
        }
    }

    private void SpawnTrayItems()
    {
        CurrentNPCItems.Clear();
        List<ItemList.ItemEntry> AvailableItems = new List<ItemList.ItemEntry>(MasterItemList.Items);
        for(int i = 0; i < 3; i++)
        {
            if (AvailableItems.Count == 0) break;
            if (AvailableItems.Count == 0) break;
            int RandomIndex = Random.Range(0, AvailableItems.Count);
            ItemList.ItemEntry ChosenItem = AvailableItems[RandomIndex];
            CurrentNPCItems.Add(ChosenItem);
            AvailableItems.RemoveAt(RandomIndex);
            if (GameUIManager.instance.DeskItemRenderers[i] != null)
            {
                GameUIManager.instance.DeskItemRenderers[i].sprite = ChosenItem.SmallSprite;
            }
            if (GameUIManager.instance.DeskItemAnimators[i] != null)
            {
                GameUIManager.instance.DeskItemAnimators[i].ShowItem();
            }
        }
        //if(GameUIManager.instance.TrayPanelManagerScript != null)
        //{
        //    GameUIManager.instance.TrayPanelManagerScript.PrepareTrayItem(CurrentNPCItems);
        //}
    }

    public void EvaluateQuestion(string Topic)
    {
        if(AskedTopics.Contains(Topic))
        {
            if(!PenaltyRepeated)
            {
                PenaltyRepeated = true;
                if (ObjectiveManager.instance != null) ObjectiveManager.instance.DeductAccuracy();
            }
            string RepeatedText = PickRandomResponse(NPCResponseDB.AskedQuestionsTwice);
            TriggerInterrogation(RepeatedText);
            return;
        }
        AskedTopics.Add(Topic);
        string ChosenText = "...";
        if (Topic == "ID")
        {
            if(HasID == true && PhysicalIDIsGovIssued == true)
            {
                ChosenText = PickRandomResponse(NPCResponseDB.QuestionIDPass);
            }
            else if(HasID == true && PhysicalIDIsGovIssued == false)
            {
                ChosenText = PickRandomResponse(NPCResponseDB.QuestionIDFailed);
            }
            else
            {
                if(HasLetter)
                {
                    ChosenText = PickRandomResponse(NPCResponseDB.QuestionIDNotThereLetter);
                }

                if(HasApplication)
                {
                    if(AppCircle == true)
                    {
                        ChosenText = PickRandomResponse(NPCResponseDB.QuestionIDNotThereApplicationFailed);
                    }
                    else
                    {
                        ChosenText = PickRandomResponse(NPCResponseDB.QuestionIDNotThereApplicationPassed);
                    }
                }

                ChosenText = PickRandomResponse(NPCResponseDB.QuestionIDNotThere);
            }

            if(IsFaceMissmatch == true || IsNameMissmatch == true || IsDOBMissmatch == true)
            {
                TriggerReaction(LeaveReaction.CaughtFakeDocument);
                return;
            }

        }
        else if (Topic == "Application")
        {
            if(HasApplication == true && PhysicalApplicationIsGovIssued == true)
            {
                if(HasID == true && AppCircle == true)
                {
                    ChosenText = PickRandomResponse(NPCResponseDB.QuestionApplicationPassed);
                }
                else if(HasID == false && AppCircle == true)
                {
                    ChosenText = PickRandomResponse(NPCResponseDB.QuestionIDNotThereApplicationFailed);
                }
                else
                {
                    ChosenText = PickRandomResponse(NPCResponseDB.QuestionIDNotThereApplicationPassed);
                }
            }
            
            if(HasApplication == true && PhysicalApplicationIsGovIssued == false)
            {
                ChosenText = PickRandomResponse(NPCResponseDB.QuestionApplicationFailed);
            }
            
        }
        else if (Topic == "Letter")
        {
            if (PhysicalLetterIsGovIssued)
            {
                ChosenText = PickRandomResponse(NPCResponseDB.QuestionLetterPassed);
            }
            else
            {
                ChosenText = PickRandomResponse(NPCResponseDB.QuestionLetterFailed);
            }
        }
        else if(Topic == "Database")
        {
            bool IsFound = DatabaseManager.Instance.IsNPCIsVisibleInDatabse(ChosenID);

            if(IsFound)
            {
                ChosenText = PickRandomResponse(NPCResponseDB.QuestionDataPassed);
            }
            else
            {
                if(IsHospitalized)
                {
                    ChosenText = PickRandomResponse(NPCResponseDB.QuestionDataFailedHospitalReasoning);
                }
                else
                {
                    if(DatabaseExcuseChoice == 0)
                    {
                        ChosenText = PickRandomResponse(NPCResponseDB.QuestionDataFailed);
                        IsTrulyValid = false;
                    }
                    else
                    {
                        ChosenText = PickRandomResponse(NPCResponseDB.QuestionDataFailedViolentSituationReasoning);
                    }
                }
            }
        }
        TriggerInterrogation(ChosenText);
    }

    private string PickRandomResponse(List<string> ResponseList)
    {
        if (ResponseList == null || ResponseList.Count == 0)
        {
            return "...";
        }
        return ResponseList[Random.Range(0, ResponseList.Count)];
    }

    public void FinishTrayInteractionAndLeave()
    {
        StartCoroutine(FinalExitRoutine());
    }

    private IEnumerator FinalExitRoutine()
    {
        string ChosenText = PickRandomResponse(NPCResponseDB.Accept);

        if (ActionPanel != null) ActionPanel.SetActive(false);
        GameUIManager.instance.SetDialogueActive(true);
        if (ChatBubble != null) ShowChatBubble();
        DialogueText.text = "";
        if(NextButton != null) NextButton.gameObject.SetActive(false);
        
        foreach(char letter in ChosenText.ToCharArray())
        {
            DialogueText.text += letter;
            yield return new WaitForSeconds(TypingSpeed);
        }
        if (NextButton != null)
        {
            NextButton.gameObject.SetActive(true);
        }
        yield return new WaitUntil(() => NextButton == null || !NextButton.gameObject.activeInHierarchy);

        GameUIManager.instance.SetDialogueActive(false);
        if(ChatBubble != null) ChatBubble.SetActive(false);

        if(GameUIManager.instance != null)
        {
            GameUIManager.instance.HideAllDocuments();
            GameUIManager.instance.HideAllItems();
        }

        yield return new WaitForSeconds(0.3f);
        if (ObjectiveManager.instance != null)
        {
            ObjectiveManager.instance.TotalAdmitted++;
            if (ObjectiveManager.instance.TotalAdmitted >= ObjectiveManager.instance.TargetObjective)
            {
                if (GameUIManager.instance != null)
                {
                    GameUIManager.instance.LockGame();
                }
            }
        }
        StartLeaving(true);

    }


    public void OnCloseDialogueClicked()
    {
        if (NextButton != null) NextButton.gameObject.SetActive(false);
        GameUIManager.instance.SetDialogueActive(false);
        if(ChatBubble != null) ChatBubble.SetActive(false);
    }

    public void OnNextButtonClicked()
    {
        if (ChatBubble != null) ChatBubble.SetActive(false);
        if (NextButton != null) NextButton.gameObject.SetActive(false);
        if(GameUIManager.instance != null)
        {
            GameUIManager.instance.SetDialogueActive(false);
        }
    }


    public bool IsShelterType()
    {
        return NPCRequestType == RequestType.Shelter;           
    }



    private void ShowChatBubble()
    {
        if (ChatBubble != null)
        {
            ChatBubble.SetActive(true);
            CanvasGroup cg = ChatBubble.GetComponent<CanvasGroup>();
            if (cg == null) cg = ChatBubble.GetComponentInParent<CanvasGroup>();

            if (cg != null)
            {
                cg.alpha = 1;
                cg.interactable = true;
                cg.blocksRaycasts = true;
            }
        }
        GameUIManager.instance.SetDialogueActive(true);
        if(NextButton != null) NextButton.gameObject.SetActive(false);
    }

    public void StartFootstep()
    {
        if(!IsWalking)
        {
            IsWalking = true;
            if (FootStepCoroutine != null) StopCoroutine(FootStepCoroutine);
            FootStepCoroutine = StartCoroutine(FootStepRoutine()); ;
        }
    }

    public void StopFootstep()
    {
        IsWalking=false;
        if(FootStepCoroutine != null)
        {
            StopCoroutine(FootStepCoroutine);
            FootStepCoroutine = null;
        }
    }

    private IEnumerator FootStepRoutine()
    {
        while(IsWalking)
        {
            if(WalkingSoundEffect != null && WalkingSoundEffect.clip != null)
            {
                WalkingSoundEffect.PlayOneShot(WalkingSoundEffect.clip);
            }
            yield return new WaitForSeconds(StepInterval);
        }
    }

    private void OnMouseDown()
    {
        if (GameUIManager.instance.IsMouseBlocked()) return;

        if(CurrentState == NPCState.WaitingForDecision)
        {
            GameUIManager.instance.OpenActionMenu();
        }
    }

    private void OnMouseEnter()
    {
        if (GameUIManager.instance.IsMouseBlocked()) return;

        if(CurrentState == NPCState.WaitingForDecision && HighlitedSprite != null)
        {
            NPCSpriteRenderer.sprite = HighlitedSprite;
        }
    }

    private void OnMouseExit()
    {
        if(NPCSpriteRenderer != null && NormalSprite != null)
        {
            NPCSpriteRenderer.sprite = NormalSprite;
        }
    }

    public void StartLeaving(bool IsSuccess)
    {
        if(IsShelterType())
        {
            ChosenExit = IsSuccess ? ExitPointShelter : ExitPointShelterFailed;
        }
        CurrentState = NPCState.MovingToExit;
        StartFootstep();
    }

    
}
