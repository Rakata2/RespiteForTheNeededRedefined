using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliciesUIManager : MonoBehaviour
{
    public GameObject LeftButton;
    public GameObject RightButton;
    public GameObject Page1;
    public GameObject Page2;

    public void NextPage()
    {
        Page1.SetActive(false);
        Page2.SetActive(true);
        RightButton.SetActive(false);
        LeftButton.SetActive(true);
    }

    public void PreviousPage()
    {
        Page1.SetActive(true);
        Page2.SetActive(false);
        RightButton.SetActive(true);
        LeftButton.SetActive(false);
    }

    
}
