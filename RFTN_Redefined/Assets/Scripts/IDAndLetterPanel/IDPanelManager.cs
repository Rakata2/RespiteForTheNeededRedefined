using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using TMPro;
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
    public TMP_Text GovernmentStampCheck;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    public void DisplayID(IdentityProfile profile, bool IsValidGovID)
    {
        Name.text = profile.Name;
        DateOfBirth.text = profile.DateOfBirth;
        Gender.text = profile.Gender;
        DateIssued.text = profile.DateIssued;
        ExpiryDate.text = profile.ExpiryDate;

        if(IsValidGovID == true)
        {
            GovernmentStampCheck.text = "(Issued by government)";
            //there will be a logo here later to be placed at the asset
        }
        else
        {
            GovernmentStampCheck.text = "";
        }

        IDPanelContainer.SetActive(true);
    }

    public void ClosePanel()
    {
        IDPanelContainer.SetActive(false);
    }
}
