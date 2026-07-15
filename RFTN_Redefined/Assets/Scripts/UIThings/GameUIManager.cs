using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager instance;
    [SerializeField] private CanvasGroup ComputerPanel;
    [Header("ComputerDisplay")]
    [SerializeField] private GameObject MainMenuPanel;
    [SerializeField] private GameObject DatabasePanel;
    [SerializeField] private GameObject PoliciesPanel;
    


    [SerializeField] private CanvasGroup IDCardPanel;
    [SerializeField] private CanvasGroup LetterPanel;
    [SerializeField] private CanvasGroup ApplicationPanel;
    [SerializeField] public CanvasGroup ActionPanel;
    [SerializeField] private GameObject Action;
    [SerializeField] private GameObject Question;
    [SerializeField] private GameObject ClarificationAccept;
    [SerializeField] private GameObject ClarificationReject;
    [SerializeField] private GameObject UIBlocker;

    [SerializeField] public GameObject MinimizedTray;
    [SerializeField] private TMP_Text TrayText;

    private bool PendingDecision;

    

    public enum WindowType
    {
        None,
        MainMenuPanel,
        DatabasePanel,
        PoliciesPanel
    }

    private WindowType CurrentlyMinimizedWindow = WindowType.None;

    private bool isDialogueActive = false;

    public DeskCardInteractable DeskCard;
    public DeskLetterInteractable DeskLetter;
    public DeskApplicationInteractable DeskApplication;

    public DatabaseUI DatabaseUIScript;
    public PoliciesUIManager PoliciesUIScript;


    private void Awake()
    {
        instance = this;
        UIBlocker.SetActive(false);
        ComputerPanel.gameObject.SetActive(false);
        IDCardPanel.gameObject.SetActive(false);

        if(MinimizedTray != null) MinimizedTray.SetActive(false);
    }

    public bool IsMouseBlocked()
    {
        return UIBlocker.activeInHierarchy || isDialogueActive;
    }

    public void SetDialogueActive(bool active)
    {
        isDialogueActive = active;
        
    }

    public void OpenComputer()
    {
        ShowPanel(ComputerPanel);
        MainMenuPanel.SetActive(true);

        DatabasePanel.SetActive(false);
        PoliciesPanel.SetActive(false);
        
    }

    public void SwitchToSubPanel(GameObject TargetPanel)
    {
        MainMenuPanel.SetActive(false);
        TargetPanel.SetActive(true);
    }

    public void ReturnToComputerMainMenu()
    {
        MainMenuPanel.SetActive(true);

        DatabasePanel.SetActive(false);
        if (DatabaseUIScript != null)
        {
            DatabaseUIScript.ResetToFirstPage();
        }

        PoliciesPanel.SetActive(false);
        if(PoliciesUIScript != null)
        {
            PoliciesUIScript.PreviousPage();
        }

    }



    public void CloseComputer()
    {
        HidePanel(ComputerPanel);

        if(DatabaseUIScript != null)
        {
            DatabaseUIScript.ResetToFirstPage();
        }

        if(PoliciesUIScript != null)
        {
            PoliciesUIScript.PreviousPage();
        }
    }

    public void OpenIDCard()
    {
        ShowPanel(IDCardPanel);
    }

    public void CloseIDCard()
    {
        HidePanel(IDCardPanel);
    }

    public void OpenLetter()
    {
        ShowPanel(LetterPanel);
    }

    public void CloseLetter()
    {
        HidePanel(LetterPanel);
    }

    public void OpenApplication()
    {
        ShowPanel(ApplicationPanel);
    }

    public void CloseApplication()
    {
        HidePanel(ApplicationPanel);
    }

    public void OpenActionMenu()
    {
        ShowPanel(ActionPanel);
    }

    public void CloseActionMenu()
    {
        Question.SetActive(false);
        ClarificationAccept.SetActive(false);
        ClarificationReject.SetActive(false);
        Action.SetActive(true);
        HidePanel(ActionPanel);
    }

    public void AcceptEntrance()
    {
        PendingDecision = true; //[NEW]
        Question.SetActive(false);
        Action.SetActive(false);
        ClarificationAccept.SetActive(true);
    }

    public void ClarifiedAccept()
    {
        Question.SetActive(false);
        ClarificationAccept.SetActive(false);
        Action.SetActive(true);
        CloseActionMenu();

        //[NEW]
        if(NPCMovement.CurrentClient != null)
        {
            ViolationManager.instance.ProcessPlayerDecision(PendingDecision, NPCMovement.CurrentClient);
        }
        else
        {
            Debug.LogWarning("No npc at counter");
        }
    }

    public void CanceledAccept()
    {
        Question.SetActive(false);
        ClarificationAccept.SetActive(false);
        Action.SetActive(true);
    }

    public void RejectEntrance()
    {
        PendingDecision = false;
        Question.SetActive(false);
        Action.SetActive(false);
        ClarificationAccept.SetActive(true);
    }

    public void ClarifiedReject()
    {
        Question.SetActive(false);
        ClarificationAccept.SetActive(false);
        Action.SetActive(true);
        CloseActionMenu();

        if (NPCMovement.CurrentClient != null)
        {
            ViolationManager.instance.ProcessPlayerDecision(PendingDecision, NPCMovement.CurrentClient);
        }
        else
        {
            Debug.LogWarning("No npc at counter");
        }
    }

    public void CanceledReject()
    {
        Question.SetActive(false);
        ClarificationReject.SetActive(false);
        Action.SetActive(true);
    }

    public void QuestionClicked()
    {
        Question.SetActive(true);
        Action.SetActive(false);
    }

    public void QuestionID()
    {
        Question.SetActive(false);
        Action.SetActive(true);
        CloseActionMenu();
    }

    public void QuestionLetter()
    {
        Question.SetActive(false);
        Action.SetActive(true);
        CloseActionMenu();
    }

    public void QuestionData()
    {
        Question.SetActive(false);
        Action.SetActive(true);
        CloseActionMenu();
    }

    

    

    public void BackButton()
    {
        Question.SetActive(false);
        Action.SetActive(true);
    }

 
    public void ShowPanel(CanvasGroup panel)
    {
        UIBlocker.SetActive(true);
        panel.gameObject.SetActive(true);

        panel.alpha = 1;
        panel.interactable = true;
        panel.blocksRaycasts = true;
    }

    public void HidePanel(CanvasGroup panel)
    {
        UIBlocker.SetActive(false);
        panel.alpha = 0;
        panel.interactable = false;
        panel.blocksRaycasts = false;
        panel.gameObject.SetActive(false);
    }

    public void MinimizedCurrentWindow(int WindowID)
    {
        CurrentlyMinimizedWindow = (WindowType)WindowID;
        if(CurrentlyMinimizedWindow == WindowType.MainMenuPanel) TrayText.text = "Main Menu";
        else if(CurrentlyMinimizedWindow == WindowType.DatabasePanel) TrayText.text = "Database";
        else if(CurrentlyMinimizedWindow == WindowType.PoliciesPanel) TrayText.text = "Policies";

        HidePanel(ComputerPanel);

        MinimizedTray.SetActive(true);
    }

    public void RestoredWindow()
    {
        if(CurrentlyMinimizedWindow != WindowType.None)
        {
            ShowPanel(ComputerPanel);
            MinimizedTray.SetActive(false);
            if (CurrentlyMinimizedWindow == WindowType.MainMenuPanel) SwitchToSubPanel(MainMenuPanel);
            else if (CurrentlyMinimizedWindow == WindowType.DatabasePanel) SwitchToSubPanel(DatabasePanel);
            else if (CurrentlyMinimizedWindow == WindowType.PoliciesPanel) SwitchToSubPanel(PoliciesPanel);
            CurrentlyMinimizedWindow = WindowType.None;
        }
    }

    public void CloseMinimizedTray()
    {
        CurrentlyMinimizedWindow = WindowType.None;
        if(MinimizedTray != null) MinimizedTray.SetActive(false);
    }

    public bool IsComputerMinimized()
    {
        return CurrentlyMinimizedWindow != WindowType.None;
    }

    

    public void CloseDialogue(CanvasGroup PrefabPanel)
    {
        PrefabPanel.alpha = 0;
        PrefabPanel.blocksRaycasts = false;
        PrefabPanel.interactable = false;

        SetDialogueActive(false);
        
         //i added the line here

        //untrustable block

        if (NPCMovement.CurrentClient != null)
        {
            if(NPCMovement.CurrentClient.IsLeaving == true)
            {
                NPCMovement.CurrentClient.StartLeaving(NPCMovement.CurrentClient.IsSuccessExit);
                if(DeskCard != null) DeskCard.GetComponent<DocumentAnimator>().HideDocument();
                if(DeskLetter != null) DeskLetter.GetComponent<DocumentAnimator>().HideDocument();
                if(DeskApplication != null) DeskApplication.GetComponent<DocumentAnimator>().HideDocument();
            }
            else
            {
                NPCMovement.CurrentClient.CurrentState = NPCMovement.NPCState.WaitingForDecision;
            }
        }

        //Debug.Log("Closed dialogue panel, other interactions are unlocked"); 
    }
}