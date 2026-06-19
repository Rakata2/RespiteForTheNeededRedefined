using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskApplicationInteractable : MonoBehaviour
{
    private IdentityProfile NPCProfile;
    private bool IsGovIssued;
    private bool Circle;
    private int ReasonIndex;
    public void ReceiveApplicationData(IdentityProfile IncomingProfile, bool IncomingGovStatus, int IncomingReasonIndex, bool IncomingCircle)
    {
        NPCProfile = IncomingProfile;
        IsGovIssued = IncomingGovStatus;
        ReasonIndex = IncomingReasonIndex;
        Circle = IncomingCircle;
    }

    private void OnMouseDown()
    {
        if (GameUIManager.instance.IsMouseBlocked()) return;

        if (NPCProfile != null)
        {
            GameUIManager.instance.OpenApplication();
            ApplicationManager.instance.DisplayApplication(NPCProfile, IsGovIssued, ReasonIndex, Circle);
        }
    }
}
