using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyNoteInteract : MonoBehaviour
{
    public SpriteRenderer StickyNote;
    public Color NormalColor = Color.white;

    [ColorUsage(true, true)]
    public Color HoverColor = new Color(0.8f, 0.8f, 0.8f);

    public Color ClickedColor = new Color(0.5f, 0.5f, 0.5f);
    public GameObject StickyNoteAlert;
    public AudioSource OpenPaper;
    public float NormalPitch = 1.0f;
    public float LowPitch = 0.7f;
    // Start is called before the first frame update
    void Start()
    {
        SetColor(NormalColor);
        StickyNoteAlert.SetActive(true);
    }

    private void OnMouseDown()
    {
        if (GameUIManager.instance.IsMouseBlocked()) return;
        OpenPaper.Play();
        StickyNoteAlert.SetActive(false);
        GameUIManager.instance.OpenStickyNote();
        SetColor(ClickedColor);
    }

    private void OnMouseEnter()
    {
        if (GameUIManager.instance.IsMouseBlocked())
        {
            SetColor(NormalColor);
            return;
        }

        SetColor(HoverColor);
    }

    private void OnMouseExit()
    {
        SetColor(NormalColor);
    }

    private void SetColor(Color TargetColor)
    {
        if(StickyNote != null)
        {
            Color newColor = TargetColor;
            newColor.a = StickyNote.color.a;
            StickyNote.color = newColor;
        }
    }
}
