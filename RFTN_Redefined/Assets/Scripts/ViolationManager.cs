using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViolationManager : MonoBehaviour
{
    public int TotalViolations = 0;
    public int MaxViolations = 3;
    public static ViolationManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void AddViolation()
    {
        TotalViolations++;
        if(MailManager.Instance != null)
        {
            MailManager.Instance.ReceiveViolationMail($"Violation Detected ({TotalViolations}/3)", $"You got violation");
        }
        Debug.Log("Violations: " + TotalViolations + "/" + MaxViolations);
        if(TotalViolations >= MaxViolations)
        {
            TriggerGameOver();
        }
    }

    private void TriggerGameOver()
    {
        //gameover screen here
        Debug.Log("violations exceeded, game over");
    }
}
