using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using JetBrains.Annotations;
using TMPro;
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