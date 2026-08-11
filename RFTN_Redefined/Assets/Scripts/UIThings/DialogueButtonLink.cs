using UnityEngine;

public class DialogueButtonLink : MonoBehaviour
{
    public CanvasGroup myPanel;
    public GameObject ChatBubble;
    //public NPCMovement NPCMovementScript;

    public void OnClick()
    {
        
        if(NPCMovement.CurrentClient != null)
        {
            if(NPCMovement.CurrentClient.CurrentState == NPCMovement.NPCState.Finished)
            {
                NPCMovement.CurrentClient.OnNextButtonClicked();
                return;
            }
        }

        GameUIManager.instance.CloseDialogue(myPanel);
        if(ChatBubble != null) ChatBubble.SetActive(false);

        //new block
        if(NPCMovement.CurrentClient != null && NPCMovement.CurrentClient.HasTrayMechanic)
        {
            if(GameUIManager.instance.TrayPanelManagerScript != null)
            {
                GameUIManager.instance.TrayPanelManagerScript.PrepareTrayItem(NPCMovement.CurrentClient.CurrentNPCItems);
            }
        }
    }
}
