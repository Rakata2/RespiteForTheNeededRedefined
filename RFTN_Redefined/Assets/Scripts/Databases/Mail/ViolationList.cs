using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ViolationType
{
    None,
    FakeID, //accepting a fake id
    InvalidDocument, //accepting documents that are not valid (not issued by the government)
    IncompleteDocument, //accepting documents that are incomplete (missing information)
    ApplicationMissingID, //accepting an application that circled yes but no ID
    RejectHospitalized, //rejecting a hospitalized NPC
    WronglyRejectedValidNPC, //rejecting a valid NPC

}
[CreateAssetMenu(fileName = "NewViolationList", menuName = "Violation List")]
public class ViolationList : ScriptableObject
{
    [System.Serializable]
    public struct ViolationEntry
    {
        public ViolationType Type;
        public string SubjectLine;
        [TextArea(3, 5)]
        public string Description;
    }

    public List<ViolationEntry> Violations = new List<ViolationEntry>();

    public ViolationEntry GetViolationEntry(ViolationType type)
    {
        foreach (var violation in Violations)
        {
            if (violation.Type == type)
            {
                return violation;
            }
        }
        return new ViolationEntry { Type = ViolationType.None, SubjectLine = "Unknown Violation", Description = "No description available." };
    }

}
