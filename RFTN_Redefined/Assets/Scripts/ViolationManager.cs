using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViolationManager : MonoBehaviour
{
    public int TotalViolations = 0;
    public int MaxViolations = 3;
    public static ViolationManager instance;

    public NPCMovement NPCAtCounter;

    public ViolationList ViolationDatabase;

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
    private ViolationType GetViolationReason(NPCMovement NPC)
    {
        if(NPC == null || DatabaseManager.Instance == null || NPC.ChosenID == null)
        {
            return ViolationType.IncompleteDocument;
        }

        if (NPC.IsFaceMissmatch) return ViolationType.FakeID;
        

        bool InDatabase = DatabaseManager.Instance.IsNPCIsVisibleInDatabse(NPC.ChosenID);

        bool IsHospitalized = NPC.IsHospitalized;

        if(!InDatabase)
        {
            if (!IsHospitalized && NPC.DatabaseExcuseChoice == 0)
            {
                return ViolationType.DatabaseMissing;
            }
        }

        if (IsHospitalized) return ViolationType.None;

        if (NPC.HasID && !NPC.PhysicalIDIsGovIssued) return ViolationType.InvalidDocument;
        if (NPC.HasLetter && !NPC.PhysicalLetterIsGovIssued) return ViolationType.InvalidDocument;
        if (NPC.HasApplication && !NPC.PhysicalApplicationIsGovIssued) return ViolationType.InvalidDocument;
        if (NPC.HasApplication && NPC.AppCircle == true && !NPC.HasID) return ViolationType.ApplicationMissingID;

        if (NPC.HasID && NPC.HasLetter) return ViolationType.None;
        if (NPC.HasID && NPC.HasApplication) return ViolationType.None;
        if (NPC.HasApplication && NPC.AppCircle == false && !NPC.HasID) return ViolationType.None;
        
        return ViolationType.IncompleteDocument;
    }
    public void ProcessPlayerDecision(bool PlayerAccepted, NPCMovement CurrentNPC)
    {
        if(CurrentNPC == null) return;
        ViolationType violation = GetViolationReason(CurrentNPC);
        bool IsNPCValid = (violation == ViolationType.None);
        if (PlayerAccepted == true)
        {
            if(IsNPCValid)
            {
                CurrentNPC.TriggerReaction(NPCMovement.LeaveReaction.Accepted);
                
            }
            else
            {
                AddViolation(violation);
                CurrentNPC.TriggerReaction(NPCMovement.LeaveReaction.Accepted);
            }
        }
        else
        {
            if(!IsNPCValid)
            {
                CurrentNPC.TriggerReaction(NPCMovement.LeaveReaction.RejectedCorrectly);
            }
            else
            {
                if(CurrentNPC.IsHospitalized)
                {
                    AddViolation(ViolationType.RejectHospitalized);
                }
                else
                {
                    AddViolation(ViolationType.WronglyRejectedValidNPC);
                }

                CurrentNPC.TriggerReaction(NPCMovement.LeaveReaction.RejectIncorrectly);
            }
        }

        
    }

    public void AddViolation(ViolationType ReasonForViolation)
    {
        TotalViolations++;
        if (MailManager.Instance != null && ViolationDatabase != null)
        {
            var TextData = ViolationDatabase.GetViolationEntry(ReasonForViolation);
            MailManager.Instance.ReceiveViolationMail($"Violation Detected ({TotalViolations}/3): {TextData.SubjectLine}", TextData.Description);
        }
        Debug.Log("Violations: " + TotalViolations + "/" + MaxViolations);
        if(TotalViolations >= MaxViolations)
        {
            if(GameUIManager.instance != null)
            {
                GameUIManager.instance.LockGame();
            }

            if(ObjectiveManager.instance != null)
            {
                ObjectiveManager.instance.TriggerGameOver();
            }
        }
    }
}
