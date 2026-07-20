using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LetterPanelManager : MonoBehaviour
{
    public static LetterPanelManager instance;

    public GameObject LetterPanelContainer;
    public TMP_Text Reasontext;
    public TMP_Text NickName;
    public Image GovernmentStampCheck;

    public List<ReasoningList> ReasoningDatabase;

    private void Awake()
    {
        instance = this;
    }

    public void DisplayLetter(IdentityProfile profile, bool IsValidGovID)
    {
        if (ReasoningDatabase != null && ReasoningDatabase.Count > 0)
        {
            int randomAssetIndex = Random.Range(0, ReasoningDatabase.Count);
            ReasoningList ChosenAsset = ReasoningDatabase[randomAssetIndex];

            if (ChosenAsset.Reasoningtext != null && ChosenAsset.Reasoningtext.Count > 0)
            {
                int randomTextIndex = Random.Range(0, ChosenAsset.Reasoningtext.Count);
                Reasontext.text = ChosenAsset.Reasoningtext[randomTextIndex];
            }
        }
        NickName.text = profile.NickName;
        if (IsValidGovID == true)
        {
            // Fixed: Image does not have a 'text' property. You likely want to show/hide the stamp or change its sprite.
            GovernmentStampCheck.gameObject.SetActive(true);
        }
        else
        {
            GovernmentStampCheck.gameObject.SetActive(false);
        }

        LetterPanelContainer.SetActive(true);
    }

    public void ClosePanel()
    {
        LetterPanelContainer.SetActive(false);
    }
    
}
