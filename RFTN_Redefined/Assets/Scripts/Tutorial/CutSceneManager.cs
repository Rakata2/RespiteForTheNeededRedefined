using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


public class CutSceneManager : MonoBehaviour
{
    public CanvasGroup BlackScreen;
    [Header("images")]
    public CanvasGroup CompanyLogo;
    public CanvasGroup PersonImage;
    public CanvasGroup DocumentsApproveImage;
    public CanvasGroup DatabasePresentImage;
    
    public GameObject TextPanel;
    public TMP_Text GuideText;
    public GameObject NextButton;

    public TutorialTextLists ListOfTexts;
    public float FadeSpeed = 1.5f;
    public float TypingSpeed = 0.05f;
    public string NextScene = "Level1";

    private int CurrentLineIndex = 0;

    private PersonMove PersonMoveScript;

    private void Start()
    {
        BlackScreen.alpha = 1f;
        CompanyLogo.alpha = 0f;
        PersonImage.alpha = 0f;
        DocumentsApproveImage.alpha = 0f;
        DatabasePresentImage.alpha = 0f;
        BlackScreen.gameObject.SetActive(true);
        TextPanel.SetActive(false);
        NextButton.SetActive(false);
        CompanyLogo.gameObject.SetActive(false);
        PersonImage.gameObject.SetActive(false);
        DocumentsApproveImage.gameObject.SetActive(false);
        DatabasePresentImage.gameObject.SetActive(false);
        GuideText.text = "";

        StartCoroutine(Sequence1());
    }

    private IEnumerator Sequence1()
    {

        yield return new WaitForSeconds(0.5f);

        //black screen disappearing
        while (BlackScreen.alpha > 0)
        {
            BlackScreen.alpha -= Time.deltaTime * FadeSpeed;
            yield return null; //nunggu frame berikutnya
        }
        BlackScreen.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        CompanyLogo.gameObject.SetActive(true);
        while (CompanyLogo.alpha < 1)
        {
            CompanyLogo.alpha += Time.deltaTime * FadeSpeed;
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);
        TextPanel.SetActive(true);
        StartCoroutine(Typewriter());
    }

    private IEnumerator Typewriter()
    {
        GuideText.text = "";
        NextButton.SetActive(false);

        foreach (char letter in ListOfTexts.TextList[CurrentLineIndex].ToCharArray())
        {
            GuideText.text += letter;
            yield return new WaitForSeconds(TypingSpeed);
        }
        NextButton.SetActive(true);
    }

    public void OnNextButtonClicked()
    {
        CurrentLineIndex++;
        if (CurrentLineIndex == 2)
        {
            StartCoroutine(Sequence2());
            CurrentLineIndex++;
        }
        else if (CurrentLineIndex < ListOfTexts.TextList.Count)
        {
            StartCoroutine(Typewriter());
        }
        else
        {
            SceneManager.LoadScene(NextScene);
        }

        if (CurrentLineIndex == 3)
        {
            //start animation coroutine here
            //add current index line ????
        }
    }

    private IEnumerator Sequence2()
    {
        TextPanel.SetActive(false);
        NextButton.SetActive(false);
        GuideText.gameObject.SetActive(false);
        
        while (CompanyLogo.alpha > 0)
        {
            CompanyLogo.alpha -= Time.deltaTime * FadeSpeed;
            yield return null;
        }
        CompanyLogo.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.3f);

        PersonImage.gameObject.SetActive(true);
        while (PersonImage.alpha < 1)
        {
            PersonImage.alpha += Time.deltaTime * FadeSpeed;
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        TextPanel.SetActive(true);
        GuideText.gameObject.SetActive(true);
        StartCoroutine(Typewriter());
    }

    private IEnumerator Sequence2Animation()
    {
        PersonMoveScript.MovePerson();
        yield return new WaitForSeconds(0.1f);


        while()

    }
}
