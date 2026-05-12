using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class RestockFoodManager : MonoBehaviour
{
    [Header("Selection settings")]
    public List<string> SelectedFoods = new List<string>();

    public Sprite DefaultPorridge;
    public Sprite DefaultSoup;
    public Sprite DefaultSandwich;

    public Sprite SelectedPorridge;
    public Sprite SelectedSoup;
    public Sprite SelectedSandwich;

    [Header("UI reference")]
    public GameObject ClarificationWindow;
    public TMP_Text PopupBodyText;

    public Image PorridgeImage;
    public Image SoupImage;
    public Image SandwichImage;

    public void ToggleFood(string needName)
    {
        if (SelectedFoods.Contains(needName))
        {
            SelectedFoods.Remove(needName);
            UpdateVisual(needName, false);
        }
        else
        {
            SelectedFoods.Add(needName);
            UpdateVisual(needName, true);
        }
    }

    private void UpdateVisual(string needName, bool active)
    {
        if (needName == "Porridge")
            PorridgeImage.sprite = active ? SelectedPorridge : DefaultPorridge;
        if (needName == "Soup")
            SoupImage.sprite = active ? SelectedSoup : DefaultSoup;
        if (needName == "Sandwich")
            SandwichImage.sprite = active ? SelectedSandwich : DefaultSandwich;
    }

    public void ClearAll()
    {
        SelectedFoods.Clear();
        PorridgeImage.sprite = DefaultPorridge;
        SoupImage.sprite = DefaultSoup;
        SandwichImage.sprite = DefaultSandwich;
    }
    public void ShowClarificationWindow()
    {
        string FoodsDisplay = SelectedFoods.Count == 0 ? "None" : string.Join(", ", SelectedFoods);
        PopupBodyText.text = $"Foods selected: {FoodsDisplay}";
        ClarificationWindow.SetActive(true);
    }

    public void CancelAssignment()
    {
        ClarificationWindow.SetActive(false);
    }

    public void GiveFood()
    {
        ClarificationWindow.SetActive(false);
        GameUIManager.instance.CloseComputer();
        ClearAll();
    }
}
