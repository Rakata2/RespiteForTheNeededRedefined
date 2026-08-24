using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DeskLetterInteractable : MonoBehaviour
{
    private IdentityProfile NPCProfile;
    private bool IsGovIssued;
    public SpriteRenderer LetterSpriteRenderer;
    public Sprite NormalLetterSprite;
    public Sprite HoveredLetterSprite;
    public AudioSource OpenPaper;

    public TMP_Text NickNameText;
    public string DisplayedNickName;



    public void ReceiveLetterData(IdentityProfile IncomingProfile, bool IncomingGovStatus, string PrintedNickName)
    {
        NPCProfile = IncomingProfile;
        IsGovIssued = IncomingGovStatus;
        DisplayedNickName = PrintedNickName;
        if(NickNameText != null) NickNameText.text = DisplayedNickName;
    }

    private void OnMouseDown()
    {
        if (GameUIManager.instance.IsMouseBlocked()) return;

        if(OpenPaper != null) AudioSource.PlayClipAtPoint(OpenPaper.clip, Camera.main.transform.position);

        if (NPCProfile != null)
        {
            GameUIManager.instance.OpenLetter();
            LetterPanelManager.instance.DisplayLetter(NPCProfile, IsGovIssued, DisplayedNickName);
        }
    }

    private void OnMouseEnter()
    {
        if(GameUIManager.instance.IsMouseBlocked())
        {
            LetterSpriteRenderer.sprite = NormalLetterSprite;
            return;
        }
        LetterSpriteRenderer.sprite = HoveredLetterSprite;
    }

    private void OnMouseExit()
    {
        LetterSpriteRenderer.sprite = NormalLetterSprite;
    }

}
