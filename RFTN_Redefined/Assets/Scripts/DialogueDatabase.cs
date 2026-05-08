using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "DialoogueDatabase", menuName = "ScriptableObjects/DialoogueDatabase")]
public class DialogueDatabase : ScriptableObject
{
    public List<string> ShelterDialogues;
    public List<string> SoupDialogues;
    public List<string> PorridgeDialogues;
    public List<string> SandwichDialogues;
}
