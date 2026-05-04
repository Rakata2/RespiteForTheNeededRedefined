using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ClosePanelButton : MonoBehaviour
{
    public CanvasGroup PanelToClose;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnCloseButtonClicked()
    {
        GameUIManager.instance.TogglePanel(PanelToClose, false);
    }
}
