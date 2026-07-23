using UnityEditor.Experimental.GraphView;
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
    public GameObject AlertedSprite;

    private bool IsAlerted = false;

    public static ClickableObject instance;
    public AudioSource ClickingSound;

    private void Awake()
    {
        if(ObjectType == InteractionType.Computer)
        {
            instance = this;
        }
        
    }

    private void Start()
    {
        AlertedSprite.SetActive(false);
    }

    public void TriggerComputerAlert()
    {
        if(ObjectType == InteractionType.Computer && ComputerSpriteRenderer != null)
        {
            IsAlerted = true;
            AlertedSprite.SetActive(true);
        }
    }

    public void ClearComputerAlert()
    {
        IsAlerted = false;
        if(AlertedSprite != null) AlertedSprite.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (GameUIManager.instance.IsMouseBlocked())
        {
            return; 
        }

        if (ClickingSound != null) ClickingSound.Play();

        if(ObjectType == InteractionType.Computer && GameUIManager.instance.IsComputerMinimized())
        {
            return;
        }
        if (ObjectType == InteractionType.Computer)
        {
            GameUIManager.instance.OpenComputer();
        }
        ClearComputerAlert();
        
    }

    private void OnMouseEnter()
    {
        if(GameUIManager.instance.IsMouseBlocked() || (ObjectType == InteractionType.Computer && GameUIManager.instance.IsComputerMinimized()))
        {
            if (IsAlerted)
            {
                AlertedSprite.SetActive(true);
            }
            else if(AlertedSprite != null)
            {
                AlertedSprite.SetActive(false);
            }
                ComputerSpriteRenderer.sprite = NormalComputerSprite;
            return;
        }
        
        ComputerSpriteRenderer.sprite = HoveredSprite; 

        if(IsAlerted)
        {
            AlertedSprite.SetActive(true);
        }
        else if(AlertedSprite != null)
        {
            AlertedSprite.SetActive(false);
        }
    }

    private void OnMouseExit()
    {
        ComputerSpriteRenderer.sprite = NormalComputerSprite;
        if(IsAlerted)
        {
            AlertedSprite.SetActive(true);
            
        }
        else if(AlertedSprite != null)
        {
            AlertedSprite.SetActive(false);
        }
    }
}
