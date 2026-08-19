using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameDataManager : MonoBehaviour
{
    public Button[] LevelButtons;

    private void Start()
    {
        int highestLevel = PlayerPrefs.GetInt("HighestLevel", 1);

        for (int i = 0; i < LevelButtons.Length; i++)
        {
            if (i + 1 > highestLevel)
            {
                LevelButtons[i].interactable = false;
            }
            else
            {
                LevelButtons[i].interactable = true;
            }
        }
    }

    public string GetSceneFromLevelOne()
    {
        int hasPlayedTutorial = PlayerPrefs.GetInt("HasPlayedTutorial", 0);
        if (hasPlayedTutorial == 0)
        {
            PlayerPrefs.SetInt("HasPlayedTutorial", 1);
            return "Tutorial";
        }
        else
        {
            return "Level1";
        }
    }
}
