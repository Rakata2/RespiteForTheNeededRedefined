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

    public void DisplayID(IdentityProfile profile)
    {
        Name.text = profile.Name;
        DateOfBirth.text = profile.DateOfBirth;
        Gender.text = profile.Gender;
        DateIssued.text = profile.DateIssued;
        ExpiryDate.text = profile.ExpiryDate;

        if(profile.IsGovernmentIssued)
        {
            GovernmentStampCheck.text = "(Issued by government)";
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
