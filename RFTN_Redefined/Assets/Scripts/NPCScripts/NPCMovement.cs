using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Security.Cryptography;


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

    public DialogueDatabase Database;
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
        Behavioral,
        Soup,
        Porridge,
        Sandwich,
        Food
    }

    public NPCState CurrentState = NPCState.MovingToCenter;

    public static NPCMovement CurrentClient;

    public IdentityProfile[] PossibleIDs;
    public IdentityProfile ChosenID;

    private int MistakeCount = 0;

    public ResourceDatabase Resources;
    void Start()
    {

        if (NextButton != null)
        {
            NextButton.gameObject.SetActive(false);
        }

        if(IsFoodType())
        {
            RequestType[] FoodOptions = { RequestType.Soup, RequestType.Porridge, RequestType.Sandwich };
            NPCRequestType = FoodOptions[Random.Range(0, FoodOptions.Length)];
        }

        if(IsShelterType() && PossibleIDs != null && PossibleIDs.Length > 0)
        {
            int randomIndex = Random.Range(0, PossibleIDs.Length);
            ChosenID = PossibleIDs[randomIndex];
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
            GameUIManager.instance.DeskCard.ReceiveID(ChosenID);
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
                return Database.ShelterDialogues;
            case RequestType.Medical:
                return Database.ShelterMedicalNeeds;
            case RequestType.Isolation:
                return Database.ShelterIsolationNeeds;
            case RequestType.Behavioral:
                return Database.ShelterBehavioralNeeds;
            case RequestType.Soup:
                return Database.SoupDialogues;
            case RequestType.Porridge:
                return Database.PorridgeDialogues;
            case RequestType.Sandwich:
                return Database.SandwichDialogues;
            default:
                return Database.ShelterDialogues; //MIND THIS PLEASE MEOW MEOW MEOW MEOW MEOW MEOW MEOW MEOW MEOW 
        }
    }

    public void PlayerGivesSoup() => ProcessFoodDelivery(RequestType.Soup);
    public void PlayerGivesPorridge() => ProcessFoodDelivery(RequestType.Porridge);
    public void PlayerGivesSandwich() => ProcessFoodDelivery(RequestType.Sandwich);

    IEnumerator CorrectFoodRoutine()
    {
        CurrentState = NPCState.Finished;

        ShowChatBubble();
        string ThankYouText = Database.FoodThankYou[Random.Range(0, Database.FoodThankYou.Count)];
        DialogueText.text = "";
        foreach(char letter in ThankYouText.ToCharArray())
        {
            DialogueText.text += letter;
            yield return new WaitForSeconds(TypingSpeed);
        }
        if (NextButton != null)
        {
            NextButton.gameObject.SetActive(true);
        }
    }

    IEnumerator FirstMistakeRoutine()
    {
        CurrentState = NPCState.Interact;
        ShowChatBubble();
        
        DialogueText.text = "";
        string MistakeText = "";

        switch(NPCRequestType)
        {
            case RequestType.Soup:
                MistakeText = Database.FoodCorrectionSoup[Random.Range(0, Database.FoodCorrectionSoup.Count)];
                break;
            case RequestType.Porridge:
                MistakeText = Database.FoodCorrectionPorridge[Random.Range(0, Database.FoodCorrectionPorridge.Count)];
                break;
            case RequestType.Sandwich:
                MistakeText = Database.FoodCorrectionSandwich[Random.Range(0, Database.FoodCorrectionSandwich.Count)];
                break;
        }

        foreach (char letter in MistakeText.ToCharArray())
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

    IEnumerator SecondMistakeRoutine()
    {
        CurrentState = NPCState.Finished;
        ShowChatBubble();
        DialogueText.text = "";

        string FrustratedText = Database.FoodSecondDecline[Random.Range(0, Database.FoodSecondDecline.Count)];
        foreach (char letter in FrustratedText.ToCharArray())
        {
            DialogueText.text += letter;
            yield return new WaitForSeconds(TypingSpeed);
        }
        if (NextButton != null)
        {
            NextButton.gameObject.SetActive(true);
        }
        if(ViolationManager.instance != null)
        {
            ViolationManager.instance.AddViolation();
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
        return NPCRequestType == RequestType.Shelter ||
               NPCRequestType == RequestType.Medical ||
               NPCRequestType == RequestType.Isolation;       
    }

    public bool IsFoodType()
    {
        return NPCRequestType == RequestType.Food ||
               NPCRequestType == RequestType.Soup ||
               NPCRequestType == RequestType.Porridge ||
               NPCRequestType == RequestType.Sandwich;
    }

    private void ProcessFoodDelivery(RequestType OfferedFood)
    {
        if (CurrentState != NPCState.WaitingForDecision) return;

        if (OfferedFood == NPCRequestType)
        {
            StartCoroutine(CorrectFoodRoutine());
        }
        else
        {
            Debug.Log("Wrong food selected");
            MistakeCount++;
            if (MistakeCount == 1)
            {
                StartCoroutine(FirstMistakeRoutine());
            }
            else if (MistakeCount >= 2)
            {
                TakeFood(OfferedFood);
                StartCoroutine(SecondMistakeRoutine());
            }
        }
    }

    private void TakeFood(RequestType Food)
    {
            switch (Food)
            {
                case RequestType.Soup:
                Resources.SoupStock--;
                    break;
                case RequestType.Porridge:
                    Resources.PorridgeStock--;
                    break;
                case RequestType.Sandwich:
                    Resources.SandwichStock--;
                    break;
        }

        FindObjectOfType<FoodDisplay>().UpdateFoodUI(); //to be updated later maybe
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

    public void StartLeaving(bool IsSuccess)
    {
        if(IsFoodType())
        {
            ChosenExit = ExitPointFood;
        }
        else if(IsShelterType())
        {
            ChosenExit = IsSuccess ? ExitPointShelter : ExitPointShelterFailed;
        }
        CurrentState = NPCState.MovingToExit;
    }
}
