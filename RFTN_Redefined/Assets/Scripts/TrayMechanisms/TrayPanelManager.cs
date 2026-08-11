using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class TrayPanelManager : MonoBehaviour
{
    public ItemInteract[] BigItemSlots;

    

    [Header("Popup stuff")]
    public GameObject NoItemPoppup;
    public GameObject ItemPoppup;
    public TMPro.TextMeshProUGUI ItemsAcceptedText;
    private bool HasItemsLoaded = false;

    public void PrepareTrayItem(List<ItemList.ItemEntry> NPCItems)
    {
        HasItemsLoaded = true;
        for(int i = 0; i<3; i++)
        {
            if(i < NPCItems.Count)
            {
                if(i < NPCItems.Count)
                {
                    BigItemSlots[i].SetupSlot(NPCItems[i]); 
                }
                else
                {
                    BigItemSlots[i].gameObject.SetActive(false);
                }
            }
        }
    }

    public void ClearTray()
    {
        HasItemsLoaded = false;
        for (int i = 0; i<BigItemSlots.Length;i++)
        {
            if(BigItemSlots[i] != null)
            {
                BigItemSlots[i].IsSelected = false;
                BigItemSlots[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnClearSelectionClicked()
    {
        for (int i = 0; i < BigItemSlots.Length; i++)
        {
            if (BigItemSlots[i].gameObject.activeSelf)
            {
                BigItemSlots[i].ForceDeselect();
            }
        }
    }

    public void OnConfirmSelectionClicked()
    {
        if(!HasItemsLoaded)
        {
            NoItemPoppup.SetActive(true);
        }
        else
        {
            ItemPoppup.SetActive(true);
            int SelectedCount = 0;
            for(int i = 0; i < BigItemSlots.Length;i++)
            {
                if(BigItemSlots[i].IsSelected)
                {
                    SelectedCount++;
                }
            }
            ItemsAcceptedText.text = SelectedCount.ToString();
        }
    }

    public void CloseNoItemPopup()
    {
        NoItemPoppup.SetActive(false);
    }

    public void CloseItemPopup()
    {
        ItemPoppup.SetActive(false);
    }

    public void FinalConfirm()
    {
        ItemPoppup.SetActive(false);
        gameObject.SetActive(false);
        if(NPCMovement.CurrentClient != null)
        {
            NPCMovement.CurrentClient.FinishTrayInteractionAndLeave();
        }
        ClearTray();
    }


}
