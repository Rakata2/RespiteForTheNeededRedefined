using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class CanvasCameraAssigner : MonoBehaviour
{
    void Start()
    {
        Canvas canvas = GetComponent<Canvas>();

        if(canvas != null)
        {
            canvas.worldCamera = Camera.main;
        }
    }
}
