using UnityEngine;

public class DialogueButtonLink : MonoBehaviour
{
    public CanvasGroup myPanel;
    public GameObject ChatBubble;

    public void OnClick()
    {
        
        if(NPCMovement.CurrentClient != null)
        {
            if(NPCMovement.CurrentClient.CurrentState == NPCMovement.NPCState.Finished)
            {
                NPCMovement.CurrentClient.OnCloseDialogueClicked();
                return;
            }
        }

        GameUIManager.instance.CloseDialogue(myPanel);
        if(ChatBubble != null) ChatBubble.SetActive(false);
    }
}
