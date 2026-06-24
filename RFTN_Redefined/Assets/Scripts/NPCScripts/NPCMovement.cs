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
    public float TypingSpeed = 0.05f;

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
    private int DatabaseExcuseChoice;

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

        if (IsApplicationNPC)
        {
            HasLetter = false;
            HasApplication = true;
            PhysicalApplicationIsGovIssued = (Random.Range(1, 101) <= 70);

            int AppBehavior = Random.Range(0, 3);
            if (AppBehavior == 0)
            {
                AppCircle = false;
                HasID = false;
                PhysicalIDIsGovIssued = false;
            }
            else if(AppBehavior == 1)
            {
                AppCircle = true;
                HasID = true;
                PhysicalIDIsGovIssued = (Random.Range(1, 101) <= 70);
            }
            else
            {
                AppCircle = true;
                HasID = false;
                PhysicalIDIsGovIssued = false;
            }
        }
        else
        {
            HasApplication = false;
            HasLetter = true;
            PhysicalLetterIsGovIssued = (Random.Range(1, 101) <= 70);

            HasID = (Random.Range(1, 101) <= 70);
            PhysicalIDIsGovIssued = HasID ? (Random.Range(1, 101) <= 70) : false;
        }

        

        if(HasID && PhysicalIDIsGovIssued)
        {
            if(Random.Range(1, 101) <= 15)
            {
                IsFaceMissmatch = true;
                FaceOnIDCard = AllGameFaces[Random.Range(0, AllGameFaces.Length)];

                while(FaceOnIDCard == ChosenID.Photo)
                {
                    FaceOnIDCard = AllGameFaces[Random.Range(0, AllGameFaces.Length)];
                }
            }
        }
    }
    void Update()
    {
        switch(CurrentState)
        {
            case NPCState.MovingToCenter:
                MoveTo(CenterPoint);
                if (IsAtPosition(CenterPoint))
                {
                    CurrentState = NPCState.Interact;

                    if(ChatBubble != null) ChatBubble.SetActive(true);
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

    IEnumerator StartInteraction()
    {
        CurrentClient = this;

        //CurrentState = NPCState.Interact;
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
        
        if(IsShelterType()&& GameUIManager.instance.DeskCard != null && ChosenID != null)
        {
            if(HasID)
            {
                GameUIManager.instance.DeskCard.gameObject.SetActive(true);
                GameUIManager.instance.DeskCard.ReceiveID(ChosenID, PhysicalIDIsGovIssued, FaceOnIDCard);
            }
            else
            {
                GameUIManager.instance.DeskCard.gameObject.SetActive(false);
            }
        }

        if (IsShelterType() && GameUIManager.instance.DeskLetter != null && ChosenID != null)
        {
            GameUIManager.instance.DeskLetter.gameObject.SetActive(HasLetter);
            if (HasLetter) GameUIManager.instance.DeskLetter.ReceiveLetterData(ChosenID, PhysicalLetterIsGovIssued);
        }

        if (IsShelterType() && GameUIManager.instance.DeskApplication != null && ChosenID != null)
        {
            GameUIManager.instance.DeskApplication.gameObject.SetActive(HasApplication);
            if(HasApplication)
            {
                GameUIManager.instance.DeskApplication.ReceiveApplicationData(ChosenID, PhysicalApplicationIsGovIssued, CheckReasonIndex, AppCircle);
            }
        }

        Debug.Log("Interaction type: " + NPCRequestType);

        List<string> SelectedList = GetListByType(NPCRequestType);
        string ChosenText = SelectedList[Random.Range(0, SelectedList.Count)];
        DialogueText.text = "";

        if(NextButton != null) NextButton.gameObject.SetActive(false);

        foreach (char letter in ChosenText.ToCharArray())
        {
            DialogueText.text += letter;
            yield return new WaitForSeconds(TypingSpeed);
        }
        if(NextButton != null)
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

    public void EvaluateQuestion(string Topic)
    {
        string ChosenText = "...";
        if (Topic == "ID")
        {
            if (IsFaceMissmatch)
            {
                ChosenText = PickRandomResponse(NPCResponseDB.QuestionFakeID); //they will leave here
            }
            else if(HasID == false)
            {
                ChosenText = PickRandomResponse(NPCResponseDB.QuestionIDNotThere);
            }
            else if(HasLetter == true && HasID == false)
            {
                ChosenText = PickRandomResponse(NPCResponseDB.QuestionIDNotThereLetter);
            }
            else if(HasApplication == true && AppCircle == true && PhysicalApplicationIsGovIssued == true)
            {
                ChosenText = PickRandomResponse(NPCResponseDB.QuestionIDNotThereApplicationPassed);
            }
            else if(HasApplication == true && AppCircle == true && PhysicalApplicationIsGovIssued == false)
            {
                ChosenText = PickRandomResponse(NPCResponseDB.QuestionIDFailed);
            }
            else if (HasApplication == true && AppCircle == false)
            {
                ChosenText = PickRandomResponse(NPCResponseDB.QuestionIDNotThereApplicationFailed);
            }
            else if (PhysicalIDIsGovIssued)
            {
                ChosenText = PickRandomResponse(NPCResponseDB.QuestionIDPass);
            }
            else
            {
                ChosenText = PickRandomResponse(NPCResponseDB.QuestionIDFailed);
            }
        }
        else if (Topic == "Application")
        {
            if (AppCircle == true && HasID == false)
            {
                ChosenText = PickRandomResponse(NPCResponseDB.QuestionApplicationIDFailed);
            }
            else if(AppCircle == true && HasID == true)
            {
                ChosenText = PickRandomResponse(NPCResponseDB.QuestionApplicationPassed);
            }
            else if (PhysicalApplicationIsGovIssued)
            {
                ChosenText = PickRandomResponse(NPCResponseDB.QuestionApplicationPassed);
            }
            else
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
    }
}
