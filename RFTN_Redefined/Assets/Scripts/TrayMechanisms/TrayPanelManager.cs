using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrayPanelManager : MonoBehaviour
{
    public List<GameObject> AllBigViewItems;

    public void PrepareTrayItems(List<GameObject> NPCItems)
    { 
        foreach(GameObject Item in AllBigViewItems)
        {
            Item.SetActive(false);
        }

        foreach (GameObject ItemToTurnOn in NPCItems)
        {
            ItemToTurnOn.SetActive(true);
        }
    }

}
