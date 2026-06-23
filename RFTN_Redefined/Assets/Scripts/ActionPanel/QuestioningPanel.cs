using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestioningPanel : MonoBehaviour
{
    public static QuestioningPanel Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void OnQuestionButtonClicked(string Topic)
    {
        if(NPCMovement.CurrentClient != null)
        {
            if(Topic == "Letter/Application")
            {
                if(NPCMovement.CurrentClient.HasApplication)
                {
                    NPCMovement.CurrentClient.EvaluateQuestion("Application");
                }
                else if(NPCMovement.CurrentClient.HasLetter)
                {
                    NPCMovement.CurrentClient.EvaluateQuestion("Letter");
                }
                else
                {
                    NPCMovement.CurrentClient.EvaluateQuestion("ID");
                }
            }
            else
            {
                NPCMovement.CurrentClient.EvaluateQuestion(Topic);
            }
        }
        else
        {
            Debug.LogWarning("No npc at desk");
        }
    }
}
