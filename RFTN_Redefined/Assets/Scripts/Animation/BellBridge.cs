using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellBridge : MonoBehaviour
{
    public static Animator instance;

    private void Awake()
    {
        instance = GetComponent<Animator>();
    }
}
