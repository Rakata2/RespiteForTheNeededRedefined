using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NormalShelterNPCResponse", menuName = "ScriptableObjects/Dialogue/NormalShelterNPCResponse")]
public class NormalShelterNPCResponse : ScriptableObject
{
    public List<string> ShelterThankYou;
    public List<string> ShelterDiversion;
    public List<string> ShelterFirstRejection;
    public List<string> ShelterSecondRejection;
    public List<string> ShelterDiversionUnwanted; //when the npc doesn't doesn't need to be diverted, no matter the needs
    public List<string> ShelterDiversionUnwantedSecondTime; //they will be angry and leave
    public List<string> MultipleMistakes; //if player makes 2 different mistakes

}
