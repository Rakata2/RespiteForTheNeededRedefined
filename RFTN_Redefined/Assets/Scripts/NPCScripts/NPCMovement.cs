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
    public NPCResponseCompleteData NPCCompleteDataDB;

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

    public bool PhysicalIDIsGovIssued;
    public bool HasLetter;
    public bool PhysicalLetterIsGovIssued;

    private int MistakeCount = 0;

    private SpriteRenderer NPCSpriteRenderer;
    private Sprite NormalSprite;
    public Sprite HighlitedSprite;

    void Awake()
    {
        NPCSpriteRenderer = GetComponent<SpriteRenderer>();    
    }

    void Start()
    {

        if (NextButton != null)
        {
            NextButton.gameObject.SetActive(false);
        }

        int MasterRoll = Random.Range(0, 101);

        if (MasterRoll <= 60)
        {
            PhysicalIDIsGovIssued = true;
            HasLetter = true;
            PhysicalLetterIsGovIssued = true;
        }
        else
        {
            int TrickRoll = Random.Range(0, 5);
             switch (TrickRoll)
            {
                case 0:
                    PhysicalIDIsGovIssued = true;
                    HasLetter = true;
                    PhysicalLetterIsGovIssued = false;
                    break;
                case 1:
                    PhysicalIDIsGovIssued = true;
                    HasLetter = false;
                    PhysicalLetterIsGovIssued = false;
                    break;
                case 2:
                    PhysicalIDIsGovIssued = false;
                    HasLetter = true;
                    PhysicalLetterIsGovIssued = true;
                    break;
                case 3:
                    PhysicalIDIsGovIssued = false;
                    HasLetter = true;
                    PhysicalLetterIsGovIssued = false;
                    break;
            }
        }

        NormalSprite = NPCSpriteRenderer.sprite;
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
            GameUIManager.instance.DeskCard.gameObject.SetActive(true);
            GameUIManager.instance.DeskCard.ReceiveID(ChosenID, PhysicalIDIsGovIssued);
        }

        if (IsShelterType() && GameUIManager.instance.DeskLetter != null && ChosenID != null)
        {
            if(HasLetter == true)
            {
                GameUIManager.instance.DeskLetter.gameObject.SetActive(true);
                GameUIManager.instance.DeskLetter.ReceiveLetterData(ChosenID, PhysicalLetterIsGovIssued);
            }
            else
            {
                GameUIManager.instance.DeskLetter.gameObject.SetActive (false);
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

    




    public void OnCloseDialogueClicked()
    {
        if (NextButton != null) NextButton.gameObject.SetActive(false);
        GameUIManager.instance.SetDialogueActive(false);
        if(ChatBubble != null) ChatBubble.SetActive(false);
        StartLeaving(true);
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
