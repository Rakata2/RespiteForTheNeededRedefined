using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedAssignmentManager : MonoBehaviour
{
    public ResourceDatabase resources;
    public ShelterDisplay displayScript;
    public GameObject ClarificationWindow;
    public GameObject BedAssignmentWarningWindow;
    public GameObject DenyShelterWindow;

    public void ShowClarificationWindow()
    {
        ClarificationWindow.SetActive(true);
    }

    public void CancelAssignment()
    {
        ClarificationWindow.SetActive(false);
    }

    public void CloseWarningWindow()
    {
        BedAssignmentWarningWindow.SetActive(false);
    }

    public void ShowWarningWindow()
    {
        BedAssignmentWarningWindow.SetActive(true);
    }

    public void ShowDenyWindow()
    {
        DenyShelterWindow.SetActive(true);
    }

    public void CancelDenyWindow()
    {
        DenyShelterWindow.SetActive(false);
    }

    public void HandleAssignBedClick()
    {
        if (NPCMovement.CurrentClient == null)
        {      
            ShowWarningWindow();
            return;
        }

        if(NPCMovement.CurrentClient.CurrentState == NPCMovement.NPCState.MovingToCenter || NPCMovement.CurrentClient.CurrentState == NPCMovement.NPCState.Interact)
        {
            ShowWarningWindow();
            return;
        }

        if (NPCMovement.CurrentClient.CurrentState == NPCMovement.NPCState.WaitingForDecision)
        {
            ShowClarificationWindow();
        }
    }

    public void HandleDenyShelterClick()
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
            ShowDenyWindow();
        }
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

    public void Deny()
    {
        ClarificationWindow.SetActive(false);
        GameUIManager.instance.CloseComputer();
    }
}
