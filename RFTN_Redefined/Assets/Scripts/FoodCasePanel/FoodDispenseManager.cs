using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using Unity.VisualScripting;

public class FoodDispenseManager : MonoBehaviour
{
    public ResourceDatabase resources;
    public FoodDisplay displayScript;
    public GameObject ClarificationWindow;
    public TMP_Text FoodTypeText;
    public GameObject FoodCaseWarningWindow;

    private string PendingFood;

    public void HandleFoodDispenseClick(string foodName)
    {
        if (NPCMovement.CurrentClient == null)
        {
            ShowWarningWindow();
            return;
        }

        if (NPCMovement.CurrentClient.CurrentState == NPCMovement.NPCState.MovingToCenter || NPCMovement.CurrentClient.CurrentState == NPCMovement.NPCState.Interact)
        {
            ShowWarningWindow();
            return;
        }

        if (NPCMovement.CurrentClient.CurrentState == NPCMovement.NPCState.WaitingForDecision)
        {
            ShowClarificationWindow(foodName);
        }
    }
    public void DispenseFood()
    {
        if (PendingFood == "Porridge" && resources.PorridgeStock > 0)
        {
            //resources.PorridgeStock--;
            if (NPCMovement.CurrentClient != null) NPCMovement.CurrentClient.PlayerGivesPorridge();
        }
        else if (PendingFood == "Soup" && resources.SoupStock > 0)
        {
            //resources.SoupStock--;
            if (NPCMovement.CurrentClient != null) NPCMovement.CurrentClient.PlayerGivesSoup();
        }
        else if (PendingFood == "Sandwich" && resources.SandwichStock > 0)
        {
            //resources.SandwichStock--;
            if (NPCMovement.CurrentClient != null) NPCMovement.CurrentClient.PlayerGivesSandwich();
        }
        else
        {
            Debug.Log("Stock empty");
        }

        //displayScript.UpdateFoodUI();
        ClarificationWindow.SetActive(false);
        GameUIManager.instance.CloseFoodDisplay();
    }

    public void ShowClarificationWindow(string foodName)
    {
        PendingFood = foodName;
        FoodTypeText.text = $"Are you sure you want to dispense {foodName}?";
        ClarificationWindow.SetActive(true);
    }
    public void CancelAssignment()
    {
        ClarificationWindow.SetActive(false);
    }

    public void ShowWarningWindow()
    {
        FoodCaseWarningWindow.SetActive(true);
    }

    public void CloseWarningWindow()
    {
        FoodCaseWarningWindow.SetActive(false);
    }
    
}
