using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FoodDialogueDatabase", menuName = "ScriptableObjects/FoodDialogueDatabase")]
public class FoodDialogueDatabase : ScriptableObject
{
    public List<string> SoupDialogues;
    public List<string> PorridgeDialogues;
    public List<string> SandwichDialogues;
}
