using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInteract : MonoBehaviour
{
    public Image SlotImage;
    private ItemList.ItemEntry AssignedItem;

    public bool IsSelected = false;

    public void SetupSlot(ItemList.ItemEntry item)
    {
        AssignedItem = item;
        IsSelected = false;
        SlotImage.sprite = AssignedItem.BigSprite;
        gameObject.SetActive(true);
    }

    public void OnSlotClicked()
    {
        IsSelected = !IsSelected;
        if(IsSelected)
        {
            SlotImage.sprite = AssignedItem.OutlinedSprite;
        }
        else
        {
            SlotImage.sprite = AssignedItem.BigSprite;
        }
    }

    public void ForceDeselect()
    {
        IsSelected = false;
        if (AssignedItem.BigSprite != null) //something here
        {
            SlotImage.sprite = AssignedItem.BigSprite;
        }
    }
}

