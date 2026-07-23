using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public AudioSource OpenCard;

    public void ReceiveID(IdentityProfile IncomingProfile, bool IncomingGovStatus, Sprite IncomingFace)
    {
        NPCProfile = IncomingProfile;
        IsGovIssued = IncomingGovStatus;
        CardFaceSprite = IncomingFace;
    }

    private void OnMouseDown()
    {
        if (GameUIManager.instance.IsMouseBlocked()) return;

        if (OpenCard != null) AudioSource.PlayClipAtPoint(OpenCard.clip, Camera.main.transform.position);

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
