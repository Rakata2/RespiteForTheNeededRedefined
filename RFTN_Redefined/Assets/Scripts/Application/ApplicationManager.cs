using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
    public static ApplicationManager instance;

    public GameObject ApplicationPanelContainer;
    public TMP_Text Name;
    public TMP_Text DOB;
    public Image[] ReasoningChecks;
    public Image CircleYes;
    public Image CircleNo;
    public TMP_Text GovernmentStampCheck;

    private void Awake()
    {
        instance = this;
    }

    public void DisplayApplication(IdentityProfile profile, bool IsValidGovID, int ReasonIndex, bool Circle)
    {
        Name.text = profile.Name;
        DOB.text = profile.DateOfBirth;

        CircleYes.enabled = Circle;
        CircleNo.enabled = !Circle;

        for (int i = 0; i < ReasoningChecks.Length; i++)
        {
            ReasoningChecks[i].enabled = (i == ReasonIndex);
        }

        if (IsValidGovID == true)
        {
            GovernmentStampCheck.text = "(Issued by government)";
            //there will be a logo here later to be placed at the asset
        }
        else
        {
            GovernmentStampCheck.text = "";
        }

        ApplicationPanelContainer.SetActive(true);
    }

    public void ClosePanel()
    {
        ApplicationPanelContainer.SetActive(false);
    }
}
