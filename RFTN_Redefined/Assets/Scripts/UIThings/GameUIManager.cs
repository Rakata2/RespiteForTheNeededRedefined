using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager instance;
    [SerializeField] private CanvasGroup ComputerPanel;
    [Header("ComputerDisplay")]
    [SerializeField] private GameObject MainMenuPanel;
    [SerializeField] private GameObject BedAssignmentPanel;
    [SerializeField] private GameObject ShelterDiversionPanel;
    [SerializeField] private GameObject RestockFoodPanel;

    [SerializeField] private CanvasGroup FoodDisplayPanel;
    [SerializeField] private GameObject UIBlocker;
    private bool isDialogueActive = false;

    private void Awake()
    {
        instance = this;
        UIBlocker.SetActive(false);
        ComputerPanel.gameObject.SetActive(false);
        FoodDisplayPanel.gameObject.SetActive(false);
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

        BedAssignmentPanel.SetActive(false);
        ShelterDiversionPanel.SetActive(false);
        RestockFoodPanel.SetActive(false);
    }

    public void SwitchToSubPanel(GameObject TargetPanel)
    {
        MainMenuPanel.SetActive(false);
        TargetPanel.SetActive(true);
    }

    public void ReturnToComputerMainMenu()
    {
        MainMenuPanel.SetActive(true);
        BedAssignmentPanel.SetActive(false);
        ShelterDiversionPanel.SetActive(false);
        RestockFoodPanel.SetActive(false);
    }

    public void OpenFoodDisplay()
    {
        ShowPanel(FoodDisplayPanel);
    }

    public void CloseComputer()
    {
        HidePanel(ComputerPanel);
    }

    public void CloseFoodDisplay()
    {
        HidePanel(FoodDisplayPanel);
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



    public void CloseDialogue(CanvasGroup PrefabPanel)
    {
        PrefabPanel.alpha = 0;
        PrefabPanel.blocksRaycasts = false;
        PrefabPanel.interactable = false;

        SetDialogueActive(false);
        
        Debug.Log("Closed dialogue panel, other interactions are unlocked"); 
    }
}