using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableObject : MonoBehaviour
{
    public enum InteractionType
    {
        Computer
    }
    public InteractionType ObjectType;

    private void OnMouseDown()
    {
        if (GameUIManager.instance.IsMouseBlocked())
        {
            return; 
        }

        if (ObjectType == InteractionType.Computer)
        {
            GameUIManager.instance.OpenComputer();
            //Debug.Log("Computer clicked");
        }
        
    }
}
