using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPanelManager : MonoBehaviour
{
    public static ActionPanelManager Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    
}
