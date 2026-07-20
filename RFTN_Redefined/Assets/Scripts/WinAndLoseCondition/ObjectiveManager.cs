using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager instance;
    public int TargetAccepted = 10;
    public int CurrentAccepted = 0;

    public bool IsShiftOver = false;

    public GameObject ResultsWindow;
    public bool HasFailed = false;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void TriggerGameOver()
    {
        HasFailed = true;
    }

    public void OnNPCDestroyed(bool WasAccepted)
    {
        if(WasAccepted && !HasFailed)
        {
            CurrentAccepted++;
            Debug.Log("NPC Accepted, total: " + CurrentAccepted + "/" + TargetAccepted);
        }

        if((CurrentAccepted >= TargetAccepted || HasFailed) && !IsShiftOver)
        {
            EndShift();
        }
    }

    private void EndShift()
    {
        IsShiftOver = true;

        if(ResultsWindow != null) ResultsWindow.SetActive(true);

        Debug.Log("Shift Ended");
    }
}
