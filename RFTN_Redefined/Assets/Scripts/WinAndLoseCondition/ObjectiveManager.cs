using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [Header("Shift summary data")]
    public int CurrentViolations = 0;
    public int MaxViolations = 3;
    public float CurrentAccuracy = 100f;

    [Header("Daily pay")]
    public float BaseWage;
    public float MaxEfficiencyBonus;
    public float PenaltyPerViolation;

    [Header("Result Screen UI Elements")]
    public TMP_Text ShiftStatus; //to be changed into an image later on
    public TMP_Text AcceptedApplicants;
    public TMP_Text ViolationsIncurred;
    public TMP_Text BaseWageAmount;
    public TMP_Text EfficiencyBonus;
    public TMP_Text ViolationPenalties;
    public TMP_Text TotalPayCheck;



    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void TriggerGameOver()
    {
        HasFailed = true;
    }

    public void DeductAccuracy()
    {
        if(CurrentAccuracy >= 10f)
        {
            CurrentAccuracy -= 10f;
        }
    }

    public void ViolationAddedToPayCheck()
    {
        CurrentViolations++;
    }

    public void CalculateAndShowResultScreen()
    {
        float FinalEfficiencyBonus = (CurrentAccuracy / 100f) * MaxEfficiencyBonus;
        float TotalViolationPenalty = CurrentViolations * PenaltyPerViolation;
        float FinalTotalPay = BaseWage + FinalEfficiencyBonus - TotalViolationPenalty;

        if(CurrentAccepted >= TargetAccepted && CurrentViolations < MaxViolations)
        {
            ShiftStatus.text = "Completed";
        }
        else
        {
            ShiftStatus.text = "Failed";
        }

        AcceptedApplicants.text = CurrentAccepted + " / " + TargetAccepted;
        ViolationsIncurred.text = CurrentViolations + " / " + MaxViolations;
        BaseWageAmount.text = "$" + BaseWage.ToString("0");
        EfficiencyBonus.text = "$" + FinalEfficiencyBonus.ToString("0");
        ViolationPenalties.text = "$" + TotalViolationPenalty.ToString("0");
        TotalPayCheck.text = "$" + FinalTotalPay.ToString("0");
        
    }

    public void EvaluatePlayerDecision(bool PlayerAccepted, bool NPCWasValid)
    {
        if(PlayerAccepted && NPCWasValid)
        {
            CurrentAccepted++;
        }
        else if(PlayerAccepted && !NPCWasValid)
        {
            ViolationAddedToPayCheck();
        }
        else if(!PlayerAccepted && NPCWasValid)
        {
            ViolationAddedToPayCheck();
        }
        else if(!PlayerAccepted && !NPCWasValid)
        {

        }

        if(CurrentAccepted >= TargetAccepted || CurrentViolations >= MaxViolations)
        {
            EndShift();
        }
    }

    private void EndShift()
    {
        IsShiftOver = true;

        if(ResultsWindow != null) ResultsWindow.SetActive(true);
        CalculateAndShowResultScreen();

        Debug.Log("Shift Ended");
    }
}
