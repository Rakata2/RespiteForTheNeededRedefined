using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MouseDetective : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        // This is the "New Input System" way to check for a left click
        if (Pointer.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Mouse.current.position.ReadValue();

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            if (results.Count > 0)
            {
                // This will tell us EXACTLY what object is under your mouse
                Debug.Log("Mouse hit: " + results[0].gameObject.name);
            }
            else
            {
                Debug.Log("Mouse hit: NOTHING (Physics/Raycaster issue)");
            }
        }
    }
}
