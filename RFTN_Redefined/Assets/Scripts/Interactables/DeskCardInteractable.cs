using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskCardInteractable : MonoBehaviour
{
    private IdentityProfile NPCProfile;
    public void ReceiveID(IdentityProfile IncomingProfile)
    {
        NPCProfile = IncomingProfile;
    }

    private void OnMouseDown()
    {
        if (GameUIManager.instance.IsMouseBlocked()) return;

        if(NPCProfile != null)
        {
            GameUIManager.instance.OpenIDCard();
            IDPanelManager.instance.DisplayID(NPCProfile);
        }
    }
}
