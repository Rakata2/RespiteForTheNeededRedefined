using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class FoodDispenseManager : MonoBehaviour
{
    public ResourceDatabase resources;
    public FoodDisplay displayScript;
    public GameObject ClarificationWindow;
    public TMP_Text FoodTypeText;

    private string PendingFood;

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

    public void DispenseFood()
    {
        if(PendingFood == "Porridge" && resources.PorridgeStock > 0)
        {
            resources.PorridgeStock--;
        }
        else if (PendingFood == "Soup" && resources.SoupStock > 0)
        {
            resources.SoupStock--;
        }
        else if (PendingFood == "Sandwich" && resources.SandwichStock > 0)
        {
            resources.SandwichStock--;
        }

        displayScript.UpdateFoodUI();
        ClarificationWindow.SetActive(false);
        GameUIManager.instance.CloseFoodDisplay();
    }
}
