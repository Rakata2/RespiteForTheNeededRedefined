using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViolationManager : MonoBehaviour
{
    public int TotalViolations = 0;
    public int MaxViolations = 3;
    public static ViolationManager instance;

    public NPCMovement NPCAtCounter;

    private void Awake()
    {
        instance = this;
    }

    //[NEW]
    public void RegisterNPCAtCounter(NPCMovement NPC)
    {
        NPCAtCounter = NPC;
    }

    //[NEW]
    private bool IsNPCValid(NPCMovement NPC)
    {
        if(NPC == null || DatabaseManager.Instance == null || NPC.ChosenID == null)
        {
            return false;
        }

        if (NPC.IsFaceMissmatch) return false;
        

        bool InDatabase = DatabaseManager.Instance.IsNPCIsVisibleInDatabse(NPC.ChosenID);

        bool IsHospitalized = NPC.IsHospitalized || (!InDatabase && NPC.DatabaseExcuseChoice == 1);

        if(!InDatabase)
        {
            if (!IsHospitalized && NPC.DatabaseExcuseChoice != 2)
            {
                return false;
            }
        }

        if (IsHospitalized) return true;

        if (NPC.HasID && !NPC.PhysicalIDIsGovIssued) return false;
        if (NPC.HasLetter && !NPC.PhysicalLetterIsGovIssued) return false;
        if (NPC.HasApplication && !NPC.PhysicalApplicationIsGovIssued) return false;
        if (NPC.HasApplication && NPC.AppCircle == true && !NPC.HasID) return false;

        

        if (NPC.HasID && NPC.HasLetter) return true;
        if (NPC.HasID && NPC.HasApplication) return true;
        if (NPC.HasApplication && NPC.AppCircle == false && !NPC.HasID) return true;
        
        return false;
    }

    //[NEW]
    public void ProcessPlayerDecision(bool PlayerAccepted, NPCMovement CurrentNPC)
    {
        if(CurrentNPC == null) return;
        bool Truth = IsNPCValid(CurrentNPC);
        if(PlayerAccepted == true)
        {
            if(Truth == true)
            {
                CurrentNPC.TriggerReaction(NPCMovement.LeaveReaction.Accepted);
                
            }
            else
            {
                AddViolation();
                CurrentNPC.TriggerReaction(NPCMovement.LeaveReaction.Accepted);
            }
        }
        else
        {
            if(Truth == false)
            {
                CurrentNPC.TriggerReaction(NPCMovement.LeaveReaction.RejectedCorrectly);
            }
            else
            {
                AddViolation();
                CurrentNPC.TriggerReaction(NPCMovement.LeaveReaction.RejectIncorrectly);
            }
        }

        
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
