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
    [SerializeField] private GameObject Application;
    [SerializeField] private GameObject UIBlocker;

    [SerializeField] private GameObject MinimizedTray;
    [SerializeField] private TMP_Text TrayText;

    

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
        PoliciesPanel.SetActive(false);

    }



    public void CloseComputer()
    {
        HidePanel(ComputerPanel);
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
        Action.SetActive(true);
        Application.SetActive(false);
        HidePanel(ActionPanel);
    }

    public void AcceptEntrance()
    {
        Question.SetActive(false);
        Action.SetActive(true);
        Application.SetActive(false);
        CloseActionMenu();
        //accepted saying thank you or something
    }

    public void RejectEntrance()
    {
        Question.SetActive(false);
        Action.SetActive(false);
        Application.SetActive(true);
    }

    public void RejectWithApplication()
    {
        Question.SetActive(false);
        Action.SetActive(true);
        Application.SetActive(false);
        CloseActionMenu();
    }

    public void RejectWithoutApplication()
    {
        Question.SetActive(false);
        Action.SetActive(true);
        Application.SetActive(false);
        CloseActionMenu();
    }
    public void QuestionClicked()
    {
        Question.SetActive(true);
        Action.SetActive(false);
        Application.SetActive(false);
    }

    public void QuestionID()
    {
        Question.SetActive(false);
        Action.SetActive(true);
        Application.SetActive(false);
        CloseActionMenu();
    }

    public void QuestionLetter()
    {
        Question.SetActive(false);
        Action.SetActive(true);
        Application.SetActive(false);
        CloseActionMenu();
    }

    public void QuestionData()
    {
        Question.SetActive(false);
        Action.SetActive(true);
        Application.SetActive(false);
        CloseActionMenu();
    }

    

    public void BackButton()
    {
        Question.SetActive(false);
        Action.SetActive(true);
        Application.SetActive(false);
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
             if(CurrentlyMinimizedWindow == WindowType.MainMenuPanel) SwitchToSubPanel(MainMenuPanel);
             else if(CurrentlyMinimizedWindow == WindowType.DatabasePanel) SwitchToSubPanel(DatabasePanel);
             else if(CurrentlyMinimizedWindow == WindowType.PoliciesPanel) SwitchToSubPanel(PoliciesPanel);
             CurrentlyMinimizedWindow = WindowType.None;
        }
    }

    public void CloseMinimizedTray()
    {
        CurrentlyMinimizedWindow = WindowType.None;
        if(MinimizedTray != null) MinimizedTray.SetActive(false);
    }

    public void CloseDialogue(CanvasGroup PrefabPanel)
    {
        PrefabPanel.alpha = 0;
        PrefabPanel.blocksRaycasts = false;
        PrefabPanel.interactable = false;

        SetDialogueActive(false);
        
        NPCMovement.CurrentClient.CurrentState = NPCMovement.NPCState.WaitingForDecision; //i added the line here

        //Debug.Log("Closed dialogue panel, other interactions are unlocked"); 
    }
}