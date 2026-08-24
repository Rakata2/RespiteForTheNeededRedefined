using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using TMPro;
using UnityEngine.UI;
using UnityEngine;


public class IDPanelManager : MonoBehaviour
{
    public static IDPanelManager instance;

    public GameObject IDPanelContainer;
    public TMP_Text Name;
    public TMP_Text DateOfBirth;
    public TMP_Text Gender;
    public TMP_Text DateIssued;
    public TMP_Text ExpiryDate;
    public Image GovernmentStampCheck;
    public Image PhotoImage;
    


    void Awake()
    {
        if (instance == null) instance = this;
    }

    public void DisplayID(IdentityProfile profile, bool IsValidGovID, Sprite FaceSprite, string PrintedName, string PrintedDOB)
    {
        if(Name != null) Name.text = PrintedName;
        if(DateOfBirth != null) DateOfBirth.text = PrintedDOB;
        Gender.text = profile.Gender;
        DateIssued.text = profile.DateIssued;
        ExpiryDate.text = profile.ExpiryDate;

        if (IsValidGovID == true)
        {
            GovernmentStampCheck.gameObject.SetActive(true);
        }
        else
        {
            GovernmentStampCheck.gameObject.SetActive(false);
        }

        if (profile != null && PhotoImage != null)
        {
            PhotoImage.sprite = profile.Photo;
        }

        if (PhotoImage != null && FaceSprite != null)
        {
            PhotoImage.sprite = FaceSprite; //do something about this later
        }

        IDPanelContainer.SetActive(true);
    }

    public void ClosePanel()
    {
        IDPanelContainer.SetActive(false);
    }
}
