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
    public int PorridgeStock = 10;
    public int PorrdigeMax = 20;

    public int SoupStock = 10;
    public int SoupMax = 20;

    public int SandwichStock = 10;
    public int SandwichMax = 20;
}
