using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCResponse", menuName = "ScriptableObjects/Dialogue/NPCResponse")]
public class NPCResponse : ScriptableObject
{
    public List<string> Accept;

    public List<string> QuestionIDPass;
    public List<string> QuestionIDFailed;
    public List<string> QuestionFakeID; //they will get caught and leave
    public List<string> QuestionLetterPassed;
    public List<string> QuestionLetterFailed;
    public List<string> QuestionDataPassed;
    public List<string> QuestionDataFailed;

    public List <string> RejectApplicationConfirmed; //their letter is not gov issued and they needed an application
    public List <string> RejectApplicationFirstUnwanted; //their letter is already gov issued
    public List <string> RejectApplicationSecondUnwanted; //they get angry and leave here
    public List<string> RejectFirstComplete;//reject but npc has everything, reusable with npc that is not on database
    public List <string> RejectSecondComplete; //They get sad and leave
    public List<string> RejectFirstIncomplete; //reject but npc does not have letter gov issued. Reusable with no letter, or injured NPC with incomplete document(s)
    public List<string> RejectSecondIncomplete; //They get sad and leave

    public List <string> MultipleMistakes; //2 mistakes and you get a violation
    
}
