using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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

    public TMP_Text NameTextID;
    public TMP_Text DOBTextID;
    public UnityEngine.UI.Image FaceRenderer;

    public string DisplayedNameID;
    public string DisplayedDOBID;

    public void ReceiveID(IdentityProfile IncomingProfile, bool IncomingGovStatus, Sprite IncomingFace, string PrintedName, string PrintedDOB)
    {
        NPCProfile = IncomingProfile;
        IsGovIssued = IncomingGovStatus;
        CardFaceSprite = IncomingFace;
        DisplayedNameID = PrintedName;
        DisplayedDOBID = PrintedDOB;
        if (NameTextID != null) NameTextID.text = DisplayedNameID;
        if(DOBTextID != null) DOBTextID.text = DisplayedDOBID;
        if (FaceRenderer != null) FaceRenderer.sprite = CardFaceSprite;
    }

    private void OnMouseDown()
    {
        if (GameUIManager.instance.IsMouseBlocked()) return;

        if (OpenCard != null) AudioSource.PlayClipAtPoint(OpenCard.clip, Camera.main.transform.position);

        if(NPCProfile != null)
        {
            GameUIManager.instance.OpenIDCard();
            IDPanelManager.instance.DisplayID(NPCProfile, IsGovIssued, CardFaceSprite, DisplayedNameID, DisplayedDOBID);
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
