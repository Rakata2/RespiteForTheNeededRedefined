using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class DatabaseUI : MonoBehaviour
{

    //public static DatabaseUI Instance; //tentative
    //public List<IdentityProfile> AllProfiles;

    public ProfileUISlot LeftSlot;
    public ProfileUISlot RightSlot;
    public TMP_Text PageText;
    public Button LeftArrowButton;
    public Button RightArrowButton;

    private int CurrentPage = 0;
    //private int TotalPage = 4; //changed from 3 to 4
    //private List<IdentityProfile> ShuffledProfiles = new List<IdentityProfile>();



    void OnEnable()
    {
        CurrentPage = 0;
        UpdatePageDisplay();
    }



    public void PageForward()
    {
        if (CurrentPage < DatabaseManager.Instance.TotalPage - 1)
        {
            CurrentPage++;
            UpdatePageDisplay();
        }
    }

    public void PageBackward()
    {
        if (CurrentPage > 0)
        {
            CurrentPage--;
            UpdatePageDisplay();
        }
    }

    private void UpdatePageDisplay()
    {
        List<IdentityProfile> Profiles = DatabaseManager.Instance.ShuffledProfiles; 
        if (PageText != null)
        {
            PageText.text = $"Page {CurrentPage + 1} / {DatabaseManager.Instance.TotalPage}";
        }

        int LeftIndex = CurrentPage * 2;
        int RightIndex = LeftIndex + 1;

        if (LeftIndex < Profiles.Count)
        {
            LeftSlot.SlotContainer.SetActive(true);
            FillSlotData(Profiles[LeftIndex], LeftSlot);
        }
        else
        {
            LeftSlot.SlotContainer.SetActive(false);
        }

        if (RightIndex < Profiles.Count)
        {
            RightSlot.SlotContainer.SetActive(true);
            FillSlotData(Profiles[RightIndex], RightSlot);
        }
        else
        {
            RightSlot.SlotContainer.SetActive(false);
        }

        if (RightArrowButton != null)
        {
            RightArrowButton.interactable = (CurrentPage < DatabaseManager.Instance.TotalPage - 1);
        }
        if (LeftArrowButton != null)
        {
            LeftArrowButton.interactable = (CurrentPage > 0);
        }
    }

    private void FillSlotData(IdentityProfile Profile, ProfileUISlot UISlot)
    {
        if(Profile.Photo != null && UISlot.PhotoImage != null)
        {
            UISlot.PhotoImage.sprite = Profile.Photo;
        }

        if(UISlot.NameText != null) UISlot.NameText.text = $"{Profile.Name}";
        if(UISlot.DOBText != null) UISlot.DOBText.text = $"{Profile.DateOfBirth}";
        if(UISlot.GenderText != null) UISlot.GenderText.text = $"{Profile.Gender}";
    }


}

[System.Serializable]
public class ProfileUISlot
{
    public GameObject SlotContainer;
    public Image PhotoImage;
    public TMP_Text NameText;
    public TMP_Text DOBText;
    public TMP_Text GenderText;
}
