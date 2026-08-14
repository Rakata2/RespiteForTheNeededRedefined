using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialTexts", menuName = "tutorial text list")]
public class TutorialTextLists : ScriptableObject
{
    public List<string> TextList = new List<string>();
}
