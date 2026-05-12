using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedAssignmentManager : MonoBehaviour
{
    public ResourceDatabase resources;
    public ShelterDisplay displayScript;
    public GameObject ClarificationWindow;
    
    public void ShowClarificationWindow()
    {
        ClarificationWindow.SetActive(true);
    }

    public void CancelAssignment()
    {
        ClarificationWindow.SetActive(false);
    }

    public void AssignBed()
    {
        
        if (resources.AvailableBeds > 0)
        {
            resources.OccupiedBeds++;
            displayScript.UpdateShelterUI();
        }
        ClarificationWindow.SetActive(false);
        GameUIManager.instance.CloseComputer();
    }
}
