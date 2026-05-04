using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager instance; //singleton pattern

    public CanvasGroup CounterTopFoodDisplayPanel;
    public CanvasGroup ComputerPanel;
    //public CanvasGroup DialoguePanel;
    private bool isDialogueActive = false;

    private void Awake()
    {
        instance = this;
    }

    public void SetDialogueActive(bool active)
    {
        isDialogueActive = active;
    }

    public bool IsAnyPanelOpen()
    {
        Debug.Log("Dialogue Active: " + isDialogueActive);  
        return CounterTopFoodDisplayPanel.blocksRaycasts || ComputerPanel.blocksRaycasts || isDialogueActive;
    }

    public void TogglePanel(CanvasGroup panel, bool open)
    {
        panel.alpha = open ? 1 : 0;
        panel.interactable = open;
        panel.blocksRaycasts = open;
    }

    public void CloseDialogue(CanvasGroup PrefabPanel)
    {
        PrefabPanel.alpha = 0;
        PrefabPanel.blocksRaycasts = false;
        PrefabPanel.interactable = false;

        GameUIManager.instance.SetDialogueActive(false);
        
        Debug.Log("Closed dialogue panel, other interactions are unlocked"); 
    }
}