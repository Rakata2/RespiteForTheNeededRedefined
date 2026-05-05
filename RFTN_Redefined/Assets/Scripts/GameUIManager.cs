using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager instance;
    [SerializeField] private CanvasGroup ComputerPanel;
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

    public void SetDialogueActive(bool active)
    {
        isDialogueActive = active;
    }

    public void OpenComputer()
    {
        ComputerPanel.gameObject.SetActive(true);
        UIBlocker.SetActive(true);
    }

    public void OpenFoodDisplay()
    {
        FoodDisplayPanel.gameObject.SetActive(true);
        UIBlocker.SetActive(true);
    }

    public void CloseComputer()
    {
        ComputerPanel.gameObject.SetActive(false);
        UIBlocker.SetActive(false);
    }

    public void CloseFoodDisplay()
    {
        FoodDisplayPanel.gameObject.SetActive(false);
        UIBlocker.SetActive(false);
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