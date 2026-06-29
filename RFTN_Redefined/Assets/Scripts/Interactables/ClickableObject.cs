using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableObject : MonoBehaviour
{
    public enum InteractionType
    {
        Computer
    }
    public InteractionType ObjectType;

    public SpriteRenderer ComputerSpriteRenderer;
    public Sprite NormalComputerSprite;
    public Sprite AlertedComputerSprite;

    private bool IsAlerted = false;

    public void TriggerComputerAlert()
    {
        if(ObjectType == InteractionType.Computer && ComputerSpriteRenderer != null)
        {
            IsAlerted = true;
            ComputerSpriteRenderer.sprite = AlertedComputerSprite;
        }
    }

    private void OnMouseDown()
    {
        if (GameUIManager.instance.IsMouseBlocked())
        {
            return; 
        }

        if (ObjectType == InteractionType.Computer)
        {
            GameUIManager.instance.OpenComputer();

            if(IsAlerted && ComputerSpriteRenderer != null)
            {
                IsAlerted = false;
                ComputerSpriteRenderer.sprite = NormalComputerSprite;
            }
        }
        
    }
}
