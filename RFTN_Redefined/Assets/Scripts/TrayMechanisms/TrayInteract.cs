using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrayInteract : MonoBehaviour
{
    public SpriteRenderer Tray;
    public Color NormalColor = Color.white;

    [ColorUsage(true, true)]
    public Color HoverColor = new Color(0.8f, 0.8f, 0.8f);

    public Color ClickedColor = new Color(0.5f, 0.5f, 0.5f);
    public AudioSource OpenTray;
    public float NormalPitch = 1.0f;
    public float LowPitch = 0.7f;

    void Start()
    {
        SetColor(NormalColor);
    }

    private void OnMouseDown()
    {
        if (GameUIManager.instance.IsMouseBlocked()) return;
        OpenTray.Play();
        GameUIManager.instance.OpenTray();
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
        if(Tray != null)
        {
            Color newColor = TargetColor;
            newColor.a = Tray.color.a;
            Tray.color = newColor;
        }
    }
}
