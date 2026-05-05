using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class TrayCLick : MonoBehaviour
{
    public GameObject Panel;
    private void OnMouseDown()
    {
        Panel.SetActive(true);
    }
}
