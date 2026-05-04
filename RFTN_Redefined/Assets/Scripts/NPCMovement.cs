using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NPCMovement : MonoBehaviour
{
    public Transform SpawnPoint;
    public Transform CenterPoint;
    public Transform ExitPoint;

    public Button NextButton;

    public float speed = 5f;

    public GameObject ChatBubble;
    public TMP_Text DialogueText;
    public float TypingSpeed = 0.05f;

    public DialogueDatabase Database;
    public RequestType NPCRequestType;
    
    public enum NPCState
    {
        MovingToCenter,
        Interact,
        MovingToExit
    }

    public enum RequestType
    {
        Shelter,
        Soup,
        Porridge,
        Sandwich
    }

    private NPCState CurrentState = NPCState.MovingToCenter;

    void Start()
    {
        if(NextButton != null)
        {
            NextButton.gameObject.SetActive(false);
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
                    if(ChatBubble != null)
                    {
                        ChatBubble.SetActive(true);
                    }

                    StartCoroutine(StartInteraction());
                }
                break;
            case NPCState.Interact:
                
                break;
            case NPCState.MovingToExit:
                MoveTo(ExitPoint);
                if (IsAtPosition(ExitPoint))
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
        GameUIManager.instance.SetDialogueActive(true);

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
    }

    //public void CloseDialogue()
    //{
    //    GameUIManager.instance.SetDialogueActive(false);
    //    ChatBubble.SetActive(false);
    //}

    List<string> GetListByType(RequestType type)
    {
        switch (type)
        {
            case RequestType.Shelter:
                return Database.ShelterDialogues;
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
    public void StartLeaving()
    {
        CurrentState = NPCState.MovingToExit;
    }
}
