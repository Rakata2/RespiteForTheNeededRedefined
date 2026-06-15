using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCResponseCompleteData", menuName = "ScriptableObjects/Dialogue/NormalShelterNPCResponse")]
public class NPCResponseCompleteData : ScriptableObject
{
    [Header("NPC with complete data")]
    public List<string> ShelterThankYou; //if they simply press accept
    public List<string> QuestionID;
    public List<string> QuestionLetter;
    public List<string> QuestionData;
    public List<string> RejectFirstApplication;
    public List<string> RejectSecondApplication;
    public List<string> RejectFirst;
    public List<string> RejectSecond;
    public List<string> MultipleMistakes;
    
}
