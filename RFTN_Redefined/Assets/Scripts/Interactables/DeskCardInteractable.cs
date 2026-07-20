using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskCardInteractable : MonoBehaviour
{
    private IdentityProfile NPCProfile;
    private bool IsGovIssued;
    private Sprite CardFaceSprite;
    public SpriteRenderer IDCardSpriteRenderer;
    public Color NormalColor = Color.white;

    [ColorUsage(true, true)]
    public Color HoveredColor;

    public Color ClickedColor;

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

        SetColor(ClickedColor);
    }

    private void OnMouseEnter()
    {
        if (GameUIManager.instance.IsMouseBlocked())
        {
            SetColor(NormalColor);
            return;
        }

        SetColor(HoveredColor);
    }

    private void OnMouseExit()
    {
        SetColor(NormalColor);
    }

    private void SetColor(Color TargetColor)
    {
        if (IDCardSpriteRenderer != null)
        {
            Color newColor = TargetColor;
            newColor.a = IDCardSpriteRenderer.color.a;
            IDCardSpriteRenderer.color = newColor;
        }
    }

}
