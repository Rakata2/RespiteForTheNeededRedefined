using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{

    public static DatabaseManager Instance; //tentative
    public List<IdentityProfile> AllProfiles;

    public ProfileUISlot LeftSlot;
    public ProfileUISlot RightSlot;
    public TMP_Text PageText;
    public Button LeftArrowButton;
    public Button RightArrowButton;

    private int CurrentPage = 0;
    private int TotalPage = 4; //changed from 3 to 4
    private List<IdentityProfile> ShuffledProfiles = new List<IdentityProfile>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    } //tentative

    void Start()
    {
        ShuffleDatabase();
        CurrentPage = 0;
        UpdatePageDisplay();
    }

    private void ShuffleDatabase()
    {
        ShuffledProfiles = new List<IdentityProfile>(AllProfiles);
        for (int i = 0; i < ShuffledProfiles.Count; i++)
        {
            IdentityProfile temp = ShuffledProfiles[i];
            int randomIndex = Random.Range(i, ShuffledProfiles.Count);
            ShuffledProfiles[i] = ShuffledProfiles[randomIndex];
            ShuffledProfiles[randomIndex] = temp;
        }
    }

    public void PageForward()
    {
        if (CurrentPage < TotalPage - 1)
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
        if (PageText != null)
        {
            PageText.text = $"Page {CurrentPage + 1} / {TotalPage}";
        }

        int LeftIndex = CurrentPage * 2;
        int RightIndex = LeftIndex + 1;

        if (LeftIndex < ShuffledProfiles.Count)
        {
            LeftSlot.SlotContainer.SetActive(true);
            FillSlotData(ShuffledProfiles[LeftIndex], LeftSlot);
        }
        else
        {
            LeftSlot.SlotContainer.SetActive(false);
        }

        if (RightIndex < ShuffledProfiles.Count)
        {
            RightSlot.SlotContainer.SetActive(true);
            FillSlotData(ShuffledProfiles[RightIndex], RightSlot);
        }
        else
        {
            RightSlot.SlotContainer.SetActive(false);
        }

        if (RightArrowButton != null)
        {
            RightArrowButton.interactable = (CurrentPage < TotalPage - 1);
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

    public bool IsNPCIsVisibleInDatabse(IdentityProfile profile)
    {
        if(profile == null) return false;

        int MaxPickedSlots = TotalPage * 2; //4 * 2 = 8, 8 max data, no more

        for(int i = 0; i< MaxPickedSlots; i++)
        {
            if (i >= ShuffledProfiles.Count) break;

            if (ShuffledProfiles[i] == profile) return true;
        }

        return false;
    } 

    //fucking around and finding out, this is tentative
    

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
