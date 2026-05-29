using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShelterDisplay : MonoBehaviour
{
    [Header("Database")]
    public ResourceDatabase resources;

    [Header("UI Text Fields")]
    public TMP_Text TotalBedText;
    public TMP_Text OccupiedBedText;
    public TMP_Text AvailableBedText;

    private void OnEnable()
    {
        UpdateShelterUI();
    }
    public void UpdateShelterUI()
    {
        if (resources == null) return;

        TotalBedText.text = resources.TotalBeds.ToString();
        OccupiedBedText.text = resources.OccupiedBeds.ToString();
        AvailableBedText.text = resources.AvailableBeds.ToString();
    }
}
