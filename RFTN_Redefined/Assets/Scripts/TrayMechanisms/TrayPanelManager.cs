using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrayPanelManager : MonoBehaviour
{
    public Image[] BigItemSlots;

    public void PrepareTrayItem(List<ItemList.ItemEntry> NPCItems)
    {
        for(int i = 0; i<3; i++)
        {
            if(i < NPCItems.Count)
            {
                if(i < NPCItems.Count)
                {
                    BigItemSlots[i].sprite = NPCItems[i].BigSprite;
                    BigItemSlots[i].gameObject.SetActive(true);
                }
                else
                {
                    BigItemSlots[i].gameObject.SetActive(false);
                }
            }
        }
    }

    

}
