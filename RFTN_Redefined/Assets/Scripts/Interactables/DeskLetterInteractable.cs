using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeskLetterInteractable : MonoBehaviour
{
    private IdentityProfile NPCProfile;
    private bool IsGovIssued;
    
    public void ReceiveLetterData(IdentityProfile IncomingProfile, bool IncomingGovStatus)
    {
        NPCProfile = IncomingProfile;
        IsGovIssued = IncomingGovStatus;
    }

    private void OnMouseDown()
    {
        if (GameUIManager.instance.IsMouseBlocked()) return;

        if (NPCProfile != null)
        {
            GameUIManager.instance.OpenLetter();
            LetterPanelManager.instance.DisplayLetter(NPCProfile, IsGovIssued);
        }
    }

}
