using UnityEngine;

public class Buttonpress : MonoBehaviour
{
    public GameObject panel;
    private void OnMouseDown()
    {
        Debug.Log("pressed");
        panel.SetActive(true);
    }
}
