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
    public Sprite HoveredSprite;
    public Sprite AlertedComputerSprite;
    public Sprite AlertedHoverComputerSprite;

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

    private void OnMouseEnter()
    {
        if(GameUIManager.instance.IsMouseBlocked())
        {
            ComputerSpriteRenderer.sprite = NormalComputerSprite;
        }
        else
        {
            if(IsAlerted == true)
            {
                ComputerSpriteRenderer.sprite = AlertedHoverComputerSprite;
            }
            else
            {
                ComputerSpriteRenderer.sprite = HoveredSprite;
            }
        }
    }

    private void OnMouseExit()
    {
        if(IsAlerted == true)
        {
            ComputerSpriteRenderer.sprite = AlertedComputerSprite;
        }
        else
        {
            ComputerSpriteRenderer.sprite= NormalComputerSprite;
        }
    }
}
