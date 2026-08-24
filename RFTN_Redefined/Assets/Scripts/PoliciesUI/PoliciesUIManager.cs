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
    public GameObject Page4;

    private int CurrentPage = 1;


    private void OnEnable()
    {
        PageOne();
    }

    private int GetMaxPages()
    {
        if (LevelManager.instance != null && LevelManager.instance.CurrentLevel >= 3)
        {
            return 4;
        }
        return 3;
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

    public void PageFour()
    {
        if (GetMaxPages() == 4)
        {
            CurrentPage = 4;
            UpdatePageDisplay();
        }
    }

    public void NextPage()
    {
        if(CurrentPage < GetMaxPages())
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
        int MaxPages = GetMaxPages();

        if(CurrentPage> MaxPages) CurrentPage = MaxPages;

        if(Page1 != null) Page1.SetActive(CurrentPage == 1);
        if(Page2 != null) Page2.SetActive(CurrentPage == 2);
        if(Page3 != null) Page3.SetActive(CurrentPage == 3);
        if(Page4 != null) Page4.SetActive(CurrentPage == 4);

        if(LeftButton != null) LeftButton.SetActive(CurrentPage > 1);
        if(RightButton != null) RightButton.SetActive(CurrentPage < MaxPages);
    }

    
}
