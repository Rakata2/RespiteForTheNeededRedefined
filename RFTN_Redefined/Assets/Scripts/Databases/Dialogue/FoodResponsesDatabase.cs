using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FoodResponsesDatabase", menuName = "ScriptableObjects/FoodResponsesDatabase")]
public class FoodResponsesDatabase : ScriptableObject
{
    public List<string> FoodThankYou;
    public List<string> FoodCorrectionPorridge;
    public List<string> FoodCorrectionSoup;
    public List<string> FoodCorrectionSandwich;
    public List<string> FoodSecondDecline;
}
