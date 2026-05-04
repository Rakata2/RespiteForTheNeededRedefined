using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    public CanvasGroup TargetPanel;
    private void OnMouseDown()
    {
        Debug.Log("Attempting to open: " + TargetPanel.name);

        // Check if the Manager is blocking us
        if (GameUIManager.instance.IsAnyPanelOpen())
        {
            Debug.Log("Action blocked: A panel is already active.");
            return;
        }

        // Use the Manager's built-in function to handle the heavy lifting
        GameUIManager.instance.TogglePanel(TargetPanel, true);

        Debug.Log(TargetPanel.name + " should now be visible!");

    }
}
