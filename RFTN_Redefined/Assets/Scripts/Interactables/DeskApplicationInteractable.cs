using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeskApplicationInteractable : MonoBehaviour
{
    private IdentityProfile NPCProfile;
    private bool IsGovIssued;
    private bool Circle;
    private int ReasonIndex;
    public SpriteRenderer ApplicationSpriteRenderer;
    public Sprite NormalApplicationSprite;
    public Sprite HoveredApplicationSprite;
    public AudioSource OpenPaper;

    public TMP_Text NameTextApplication;
    public TMP_Text DOBTextApplication;
    public string DisplayedNameApplication;
    public string DisplayedDOBApplication;
    public void ReceiveApplicationData(IdentityProfile IncomingProfile, bool IncomingGovStatus, int IncomingReasonIndex, bool IncomingCircle, string PrintedName, string PrintedDOB)
    {
        NPCProfile = IncomingProfile;
        IsGovIssued = IncomingGovStatus;
        ReasonIndex = IncomingReasonIndex;
        Circle = IncomingCircle;

        DisplayedNameApplication = PrintedName;
        DisplayedDOBApplication = PrintedDOB;

        if(NameTextApplication != null) NameTextApplication.text = DisplayedNameApplication;
        if(DOBTextApplication != null) DOBTextApplication.text = DisplayedDOBApplication;
    }

    

    private void OnMouseDown()
    {
        if (GameUIManager.instance.IsMouseBlocked()) return;


        if(OpenPaper != null) AudioSource.PlayClipAtPoint(OpenPaper.clip, Camera.main.transform.position);

        if (NPCProfile != null)
        {
            GameUIManager.instance.OpenApplication();
            ApplicationManager.instance.DisplayApplication(NPCProfile, IsGovIssued, ReasonIndex, Circle, DisplayedNameApplication, DisplayedDOBApplication);
        }
    }

    private void OnMouseEnter()
    {
        if(GameUIManager.instance.IsMouseBlocked())
        {
            ApplicationSpriteRenderer.sprite = NormalApplicationSprite;
            return;
        }

        ApplicationSpriteRenderer.sprite = HoveredApplicationSprite;
    }

    private void OnMouseExit()
    {
        ApplicationSpriteRenderer.sprite = NormalApplicationSprite;
    }

}
