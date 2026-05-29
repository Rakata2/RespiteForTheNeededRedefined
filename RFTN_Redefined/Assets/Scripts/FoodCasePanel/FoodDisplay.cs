using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class FoodDisplay : MonoBehaviour
{
    [Header("Database")]
    public ResourceDatabase resources;

    [Header("Food")]
    public TMP_Text PorridgeText;
    public TMP_Text SoupText;
    public TMP_Text SandwichText;

    private void OnEnable()
    {
        UpdateFoodUI();
    }
    public void UpdateFoodUI()
    {
        if (resources == null) return;

        PorridgeText.text = $"{resources.PorridgeStock} / {resources.PorrdigeMax}";
        SoupText.text = $"{resources.SoupStock} / {resources.SoupMax}";
        SandwichText.text = $"{resources.SandwichStock} / {resources.SandwichMax}";
    }
}
