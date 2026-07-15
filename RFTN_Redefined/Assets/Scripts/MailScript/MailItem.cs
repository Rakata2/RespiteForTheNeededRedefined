using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class MailItem : MonoBehaviour
{
    public TMP_Text SubjectText;
    public TMP_Text DescriptionText;
    public GameObject BodyContainer; //this is the dropdown

    private bool IsExpanded = false;

    public void SetupMail(string Subject, string Description)
    {
        SubjectText.text = Subject;
        DescriptionText.text = Description;
        BodyContainer.SetActive(false);
    }

    public void ToggleDropDown()
    {
        IsExpanded = !IsExpanded;
        BodyContainer.SetActive(IsExpanded);
        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
    }

    public void ForceClose()
    {
        if(IsExpanded)
        {
            IsExpanded = false;
            BodyContainer.SetActive(false);

            if(transform.parent != null)
            {
                UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
            }
        }
    }
}
