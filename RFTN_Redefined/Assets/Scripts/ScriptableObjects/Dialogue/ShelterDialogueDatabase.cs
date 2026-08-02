using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "ShelterDialogueDatabase", menuName = "ScriptableObjects/ShelterDialogueDatabase")]
public class ShelterDialogueDatabase : ScriptableObject
{
    [Header("Shelter Dialogues")]
    public List<string> ShelterDialogues;
    public List<string> ShelterMedicalNeeds;
    public List<string> ShelterIsolationNeeds;
    public List<string> ShelterBehavioralNeeds;
}
