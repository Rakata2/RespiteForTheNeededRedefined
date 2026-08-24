using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New ID Profile", menuName = "ID System/Identity Profile")]
public class IdentityProfile : ScriptableObject
{
    public string Name;
    public string NickName;
    public string DateOfBirth;
    public string Gender;
    public string DateIssued;
    public string ExpiryDate;
    public Sprite Photo;

    [Header("Fake ID check")]
    public bool IsGovernmentIssued = true;
    
}
