using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceDatabase", menuName = "ScriptableObjects/ResourceDatabase")]
public class ResourceDatabase : ScriptableObject
{
    [Header("Shelter")]
    public int TotalBeds = 30;
    public int OccupiedBeds = 5;
    public int AvailableBeds => TotalBeds - OccupiedBeds;

    [Header("Food")]
    public int PorrdigeStock = 20;
    public int SoupStock = 20;
    public int SandwichStock = 20;  
}
