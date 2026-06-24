using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCResponse", menuName = "ScriptableObjects/Dialogue/NPCResponse")]
public class NPCResponse : ScriptableObject
{
    public List<string> Accept; //simply lets them in, 3 dialogues

    [Header("Question ID")]
    public List<string> QuestionIDPass; //ID is there and government issued, only needed 1
    public List<string> QuestionIDFailed; //id is not government issued, only needed 1
    public List<string> QuestionIDNotThere; //[NEW] Id is not even there, only needed 1
    public List<string> QuestionIDNotThereApplicationPassed; // [NEW] npc circled no in ID, only needed 1
    public List<string> QuestionIDNotThereApplicationFailed; //[NEW] NPC circled yes in ID but the ID isn't actually there, only needed 1
    public List<string> QuestionIDNotThereLetter; //[NEW] npc circled no in ID, only needed 1
    public List<string> QuestionFakeID; //NPC will say something about this and leave quickly, 2 dialogues 
    [Header("Question Letter")]
    public List<string> QuestionLetterPassed; //Letter is government issued, only needed 1
    public List<string> QuestionLetterFailed; //letter is not government issued, only needed 1
    [Header("Question Application")]
    public List<string> QuestionApplicationPassed; //application is government issued, only needed 1
    public List<string> QuestionApplicationFailed; //application is not government issued, only needed 1
    public List<string> QuestionApplicationIDFailed; //NPC circled yes in ID, but the id isn't actually there, only needed 1
    [Header("Question database")]
    public List<string> QuestionDataPassed; //NPC is on database, only needed 1
    public List<string> QuestionDataFailedHospitalReasoning; //[NEW] NPC is not on database, gives out hospital reasoning, only needed 1
    public List<string> QuestionDataFailedViolentSituationReasoning; //[NEW] NPC is not on database, gives out violent situation reasoning, only needed 1
    public List<string> QuestionDataFailed;//NPC is not on database because they didn't know they need to register, only needed 1

    [Header("Rejection")]
    public List<string> RejectApplicationConfirmed; //their letter is not gov issued and they needed an application
    public List<string> RejectApplicationFirstUnwanted; //their letter is already gov issued
    public List<string> RejectApplicationSecondUnwanted; //they get angry and leave here
    public List<string> RejectFirstComplete;//reject but npc has everything, reusable with npc that is not on database
    public List<string> RejectSecondComplete; //They get sad and leave
    public List<string> RejectFirstIncomplete; //reject but npc does not have letter gov issued. Reusable with no letter, or injured NPC with incomplete document(s)
    public List<string> RejectSecondIncomplete; //They get sad and leave

    [Header("Multiple Mistakes")]
    public List <string> MultipleMistakes; //2 mistakes and you get a violation
    
}
