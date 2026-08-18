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
    public CanvasGroup QuestionMark;
    public CanvasGroup DocumentInconsistency;
    public CanvasGroup Arrow;
    public CanvasGroup Reject;
    public CanvasGroup OfficeStructure;
    

    public GameObject TextPanel;
    public TMP_Text GuideText;
    public GameObject NextButton;

    public TutorialTextLists ListOfTexts;
    public float FadeSpeed = 1.5f;
    public float TypingSpeed = 0.05f;
    public string NextScene = "Level1";

    private int CurrentLineIndex = 0;

    private PersonMove PersonMoveScript;
    public float PersonMoveDistance;
    public float PersonMoveDuration;

    private void Start()
    {
        BlackScreen.alpha = 1f;
        CompanyLogo.alpha = 0f;
        PersonImage.alpha = 0f;
        DocumentsApproveImage.alpha = 0f;
        DatabasePresentImage.alpha = 0f;
        OfficeStructure.alpha = 0f;
        BlackScreen.gameObject.SetActive(true);
        TextPanel.SetActive(false);
        NextButton.SetActive(false);
        CompanyLogo.gameObject.SetActive(false);
        PersonImage.gameObject.SetActive(false);
        DocumentsApproveImage.gameObject.SetActive(false);
        DatabasePresentImage.gameObject.SetActive(false);
        QuestionMark.gameObject.SetActive(false);
        
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
            yield return null; 
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
        }
        else if (CurrentLineIndex == 3)
        {
            StartCoroutine(Sequence2Animation());
            StartCoroutine(Typewriter());
        }
        else if (CurrentLineIndex == 4)
        {
            QuestionMark.gameObject.SetActive(true);
            StartCoroutine(Typewriter());
        }
        else if (CurrentLineIndex == 5)
        {
            StartCoroutine(Typewriter());
            StartCoroutine(Sequence3());
        }
        else if(CurrentLineIndex == 6)
        {
            StartCoroutine(Sequence4());
        }
        else if (CurrentLineIndex < ListOfTexts.TextList.Count)
        {
            StartCoroutine(Typewriter());
        }
        else
        {
            Debug.Log("Moves to level 1");
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
        RectTransform PersonRect = PersonImage.GetComponent<RectTransform>();
        Vector2 StartPos = PersonRect.anchoredPosition;

        Vector2 TargetPos = new Vector2(StartPos.x - PersonMoveDistance, StartPos.y);

        float ElapsedTime = 0f;

        while (ElapsedTime < PersonMoveDuration)
        {
            ElapsedTime += Time.deltaTime;
            PersonRect.anchoredPosition = Vector2.Lerp(StartPos, TargetPos, ElapsedTime / PersonMoveDuration);
            yield return null;
        }
        PersonRect.anchoredPosition = TargetPos;

        yield return new WaitForSeconds(0.2f);

        DocumentsApproveImage.gameObject.SetActive(true);
        while (DocumentsApproveImage.alpha < 1)
        {
            DocumentsApproveImage.alpha += Time.deltaTime * FadeSpeed;
            yield return null;
        }
        DatabasePresentImage.gameObject.SetActive(true);
        while (DatabasePresentImage.alpha < 1)
        {
            DatabasePresentImage.alpha += Time.deltaTime * FadeSpeed;
            yield return null;
        }
    }

    private IEnumerator Sequence3()
    {
        QuestionMark.gameObject.SetActive(false);
        DocumentsApproveImage.gameObject.SetActive(false);
        DatabasePresentImage.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.3f);

        DocumentInconsistency.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Arrow.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Reject.gameObject.SetActive(true);
    }

    private IEnumerator Sequence4()
    {
        TextPanel.SetActive(false);
        NextButton.SetActive(false);
        GuideText.gameObject.SetActive(false);
        while (DocumentInconsistency.alpha > 0 || Arrow.alpha > 0 || Reject.alpha > 0 || PersonImage.alpha > 0)
        {
            float FadeAmount = Time.deltaTime * FadeSpeed;
            DocumentInconsistency.alpha -= FadeAmount;
            Arrow.alpha -= FadeAmount;
            Reject.alpha -= FadeAmount;
            PersonImage.alpha -= FadeAmount;
            yield return null;
        }

        DocumentInconsistency.gameObject.SetActive(false);
        Arrow.gameObject .SetActive(false);
        Reject.gameObject.SetActive(false);
        PersonImage.gameObject .SetActive(false);

        yield return new WaitForSeconds(0.4f);

        OfficeStructure.gameObject.SetActive(true);
        while (OfficeStructure.alpha < 1)
        {
            OfficeStructure.alpha += Time.deltaTime * FadeSpeed;
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        TextPanel.SetActive(true);
        GuideText.gameObject.SetActive(true);
        StartCoroutine(Typewriter());
    }


}
