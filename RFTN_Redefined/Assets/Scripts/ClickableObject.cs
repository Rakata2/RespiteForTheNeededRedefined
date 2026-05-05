using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    public enum InteractionType
    {
        Computer,
        FoodDisplay
    }
    public InteractionType ObjectType;

    private void OnMouseDown()
    {
        
        if (ObjectType == InteractionType.Computer)
        {
            GameUIManager.instance.OpenComputer();
            Debug.Log("Computer clicked");
        }
        else if(ObjectType == InteractionType.FoodDisplay)
        {
            GameUIManager.instance.OpenFoodDisplay();
            Debug.Log("Food Display clicked"); 
        }
    }
}
