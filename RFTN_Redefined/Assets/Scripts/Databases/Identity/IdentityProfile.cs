using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[CreateAssetMenu(fileName = "New ID Profile", menuName = "ID System/Identity Profile")]
public class IdentityProfile : ScriptableObject
{
    public string Name;
    public string DateOfBirth;
    public string Gender;
    public string DateIssued;
    public string ExpiryDate;
    //public Sprite Photo; will be used later

    [Header("Fake ID check")]
    public bool IsGovernmentIssued = true;
    
}
