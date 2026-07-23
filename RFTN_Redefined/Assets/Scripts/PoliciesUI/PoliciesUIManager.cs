using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoliciesUIManager : MonoBehaviour
{
    public GameObject LeftButton;
    public GameObject RightButton;
    public GameObject Page1;
    public GameObject Page2;
    public GameObject Page3;

    private int CurrentPage = 1;


    private void OnEnable()
    {
        PageOne();
    }

    public void PageOne()
    {
        CurrentPage = 1;
        UpdatePageDisplay();
        
    }

    
    public void PageTwo()
    {
        CurrentPage = 2;
        UpdatePageDisplay();
    }

    public void PageThree()
    {
        CurrentPage = 3;
        UpdatePageDisplay();
    }

    public void NextPage()
    {
        if(CurrentPage < 3)
        {
            CurrentPage++;
            UpdatePageDisplay();
        }
    }

    public void PreviousPage()
    {

        Debug.Log("Left arrow clicked! Current page was: " + CurrentPage);
        if (CurrentPage > 1)
        {
            CurrentPage--;
            UpdatePageDisplay();
        }
    }

    public void UpdatePageDisplay()
    {
        if(Page1 != null) Page1.SetActive(CurrentPage == 1);
        if(Page2 != null) Page2.SetActive(CurrentPage == 2);
        if(Page3 != null) Page3.SetActive(CurrentPage == 3);

        if(LeftButton != null) LeftButton.SetActive(CurrentPage > 1);
        if(RightButton != null) RightButton.SetActive(CurrentPage < 3);
    }

    
}
