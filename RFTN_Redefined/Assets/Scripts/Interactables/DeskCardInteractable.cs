using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskCardInteractable : MonoBehaviour
{
    private IdentityProfile NPCProfile;
    private bool IsGovIssued;
    private Sprite CardFaceSprite;
    public void ReceiveID(IdentityProfile IncomingProfile, bool IncomingGovStatus, Sprite IncomingFace)
    {
        NPCProfile = IncomingProfile;
        IsGovIssued = IncomingGovStatus;
        CardFaceSprite = IncomingFace;
    }

    private void OnMouseDown()
    {
        if (GameUIManager.instance.IsMouseBlocked()) return;

        if(NPCProfile != null)
        {
            GameUIManager.instance.OpenIDCard();
            IDPanelManager.instance.DisplayID(NPCProfile, IsGovIssued, CardFaceSprite);
        }
    }
}
