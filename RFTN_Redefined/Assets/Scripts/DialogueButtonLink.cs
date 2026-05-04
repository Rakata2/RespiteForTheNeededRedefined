using UnityEngine;

public class DialogueButtonLink : MonoBehaviour
{
    public CanvasGroup myPanel;
    public GameObject ChatBubble;

    public void OnClick()
    {
        GameUIManager.instance.CloseDialogue(myPanel);
        ChatBubble.SetActive(false);
    }
}
