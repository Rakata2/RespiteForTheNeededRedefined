using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LetterPanelManager : MonoBehaviour
{
    public static LetterPanelManager instance;

    public GameObject LetterPanelContainer;
    public TMP_Text Reasontext;
    public TMP_Text NickName;
    public TMP_Text GovernmentStampCheck;

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
        if(IsValidGovID == true)
        {
            GovernmentStampCheck.text = "(Issued by government)";
        }
        else
        {
            GovernmentStampCheck.text = "";
        }

        LetterPanelContainer.SetActive(true);
    }

    public void ClosePanel()
    {
        LetterPanelContainer.SetActive(false);
    }
    
}
