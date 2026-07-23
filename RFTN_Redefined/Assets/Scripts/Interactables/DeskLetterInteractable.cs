using System.Collections;
using System.Collections.Generic;
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

    public void ReceiveLetterData(IdentityProfile IncomingProfile, bool IncomingGovStatus)
    {
        NPCProfile = IncomingProfile;
        IsGovIssued = IncomingGovStatus;
    }

    private void OnMouseDown()
    {
        if (GameUIManager.instance.IsMouseBlocked()) return;

        if(OpenPaper != null) AudioSource.PlayClipAtPoint(OpenPaper.clip, Camera.main.transform.position);

        if (NPCProfile != null)
        {
            GameUIManager.instance.OpenLetter();
            LetterPanelManager.instance.DisplayLetter(NPCProfile, IsGovIssued);
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
