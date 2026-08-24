using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public int CurrentLevel = 1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ApplyLevelRules(NPCMovement NPCMovementScript)
    {
        if (CurrentLevel == 1)
        {
            NPCMovementScript.AllowNameAndDOBMissmatch = false;
            NPCMovementScript.HasTrayMechanic = false;
            NPCMovementScript.CurrentNPCItems = new List<ItemList.ItemEntry>();
        }
        else if (CurrentLevel == 2)
        {
            NPCMovementScript.AllowNameAndDOBMissmatch = true;
            NPCMovementScript.HasTrayMechanic = false;
            NPCMovementScript.CurrentNPCItems = new List<ItemList.ItemEntry>();
        }
        else if (CurrentLevel == 3)
        {
            NPCMovementScript.AllowNameAndDOBMissmatch = false;
            NPCMovementScript.HasTrayMechanic = true;
            NPCMovementScript.CurrentNPCItems = new List<ItemList.ItemEntry>();
        }
        else if(CurrentLevel == 4)
        {
            NPCMovementScript.AllowNameAndDOBMissmatch = true;
            NPCMovementScript.HasTrayMechanic = true;
            NPCMovementScript.CurrentNPCItems = new List<ItemList.ItemEntry>();
        }
    }
}
