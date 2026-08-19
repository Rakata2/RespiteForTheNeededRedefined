using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using JetBrains.Annotations;
using UnityEditor;


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
    public CanvasGroup PairedDocument;
    public CanvasGroup IDDocument;
    public CanvasGroup LetterDocument;
    public CanvasGroup ApplicationDocument;
    public CanvasGroup ApplicationText;
    public CanvasGroup EmptyApplication;
    public CanvasGroup EmptyApplicationText;
    public CanvasGroup SkipButton;


    public GameObject SkipButtonPopup;
    public GameObject TextPanel;
    public TMP_Text GuideText;
    public GameObject NextButton;
    public TutorialTextLists ListOfTexts;
    public float FadeSpeed = 0.4f;
    public float TypingSpeed = 0.05f;
    public string NextScene = "Level1";
    private int CurrentLineIndex = 0;
    private PersonMove PersonMoveScript;
    public float PersonMoveDistance;
    public float PersonMoveDuration;

    [Header("Animation")]
    public Animator FlashingObjectAnimator;
    public Animator FlashingComputer;
    public Animator FlashingIDDocument;
    public Animator FlashingPaperDocument;
    public Animator FlashingApplication;
    public Animator FlashingPerson;
    public Animator FlashingEmptyApplication;

    private void Start()
    {
        BlackScreen.alpha = 1f;
        CompanyLogo.alpha = 0f;
        PersonImage.alpha = 0f;
        DocumentsApproveImage.alpha = 0f;
        DatabasePresentImage.alpha = 0f;
        OfficeStructure.alpha = 0f;
        PairedDocument.alpha = 0f;
        ApplicationDocument.alpha = 0f;
        IDDocument.alpha = 1f;
        LetterDocument.alpha = 1f;
        ApplicationText.alpha = 0f;
        EmptyApplication.alpha = 0f;
        EmptyApplicationText.alpha = 0f;
        SkipButton.alpha = 0f;
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

        SkipButton.gameObject.SetActive(true);
        while(SkipButton.alpha < 1)
        {
            SkipButton.alpha += Time.deltaTime * FadeSpeed;
            yield return null;
        }
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
        else if(CurrentLineIndex == 7)
        {
            FlashingObjectAnimator.SetBool("IsFlashing", true);
            StartCoroutine(Typewriter());
        }
        else if(CurrentLineIndex == 9)
        {
            FlashingObjectAnimator.SetBool("IsFlashing", false);
            FlashingComputer.SetBool("IsFlashing", true);
            StartCoroutine(Typewriter());
        }
        else if(CurrentLineIndex == 12)
        {
            FlashingComputer.SetBool("IsFlashing", false);
            FlashingIDDocument.SetBool("IsFlashing", true);
            FlashingPaperDocument.SetBool("IsFlashing", true);
            StartCoroutine(Typewriter());
        }
        else if(CurrentLineIndex == 13)
        {
            StartCoroutine(PairedDocumentAnimator());
            StartCoroutine(Typewriter());
        }
        else if(CurrentLineIndex == 14)
        {
            StartCoroutine(IDLetterToApplicationAnimation());
            StartCoroutine(Typewriter());
        }
        else if(CurrentLineIndex == 15)
        {
            FlashingPerson.SetBool("IsFlashing", true);
            FlashingApplication.SetBool("IsFlashing", false);
            StartCoroutine(ApplicationTextDisappear());
            StartCoroutine (Typewriter());
        }
        else if(CurrentLineIndex == 17)
        {
            FlashingPerson.SetBool("IsFlashing", false);
            StartCoroutine(EmptyApplicationGiven());
            StartCoroutine(Typewriter());
        }
        else if(CurrentLineIndex == 18)
        {
            FlashingEmptyApplication.SetBool("IsFlashing", false);
            StartCoroutine(Typewriter());
        }
        else if(CurrentLineIndex == 20)
        {
            StartCoroutine(Sequence5());
        }
        else if (CurrentLineIndex < ListOfTexts.TextList.Count)
        {
            StartCoroutine(Typewriter());
        }
        else
        {
            StartCoroutine(ClosingTransition());
            Debug.Log("Moves to level 1");
            SceneManager.LoadScene(NextScene);
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

        yield return new WaitForSeconds(0.2f);

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
        yield return new WaitForSeconds(0.2f);

        DocumentInconsistency.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        Arrow.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.4f);
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

        yield return new WaitForSeconds(0.2f);

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

    private IEnumerator PairedDocumentAnimator()
    {
        PairedDocument.gameObject.SetActive(true);
        while (PairedDocument.alpha < 1)
        {
            PairedDocument.alpha += Time.deltaTime * FadeSpeed;
            yield return null;
        }
    }

    private IEnumerator IDLetterToApplicationAnimation()
    {
        while (IDDocument.alpha > 0 || LetterDocument.alpha > 0 || PairedDocument.alpha > 0)
        {
            float FadeAmount = Time.deltaTime * FadeSpeed;
            IDDocument.alpha -= FadeAmount;
            LetterDocument.alpha -= FadeAmount;
            PairedDocument.alpha -= FadeAmount;
            yield return null;
        }
        FlashingIDDocument.SetBool("IsFlashing", false);
        FlashingPaperDocument.SetBool("IsFlashing", false);
        IDDocument.gameObject.SetActive(false);
        LetterDocument.gameObject.SetActive(false);
        PairedDocument.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.2f);

        ApplicationDocument.gameObject.SetActive(true);
        ApplicationText.gameObject.SetActive(true);
        while (ApplicationDocument.alpha < 1 || ApplicationText.alpha < 1)
        {
            float FadeAmount = Time.deltaTime * FadeSpeed;
            ApplicationDocument.alpha += FadeAmount;
            ApplicationText.alpha += FadeAmount;
            yield return null;
        }
        FlashingApplication.SetBool("IsFlashing", true);
    }

    private IEnumerator ApplicationTextDisappear()
    {
        while (ApplicationText.alpha > 0)
        {
            ApplicationText.alpha -= Time.deltaTime * FadeSpeed;
            yield return null;
        }
        ApplicationText.gameObject.SetActive(false);
    }

    private IEnumerator EmptyApplicationGiven()
    {
        while(ApplicationDocument.alpha > 0)
        {
            ApplicationDocument.alpha -= Time.deltaTime * FadeSpeed;
            yield return null;
        }
        ApplicationText.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.2f);

        EmptyApplication.gameObject.SetActive(true);
        while (EmptyApplication.alpha < 1)
        {
            EmptyApplication.alpha += Time.deltaTime * FadeSpeed;
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        EmptyApplicationText.gameObject.SetActive(true);
        while (EmptyApplicationText.alpha < 1)
        {
            EmptyApplicationText.alpha += Time.deltaTime * FadeSpeed;
            yield return null;
        }
        FlashingEmptyApplication.SetBool("IsFlashing", true);
    }
    private IEnumerator Sequence5()
    {
        TextPanel.SetActive(false);
        NextButton.SetActive(false);
        GuideText.gameObject.SetActive(false);
        while (OfficeStructure.alpha > 0)
        {
            OfficeStructure.alpha -= Time.deltaTime * FadeSpeed;
            yield return null;
        }
        OfficeStructure.gameObject.SetActive(false);
        while(EmptyApplicationText.alpha > 0)
        {
            EmptyApplicationText.alpha -= Time.deltaTime * FadeSpeed;
            yield return null;
        }
        EmptyApplicationText.gameObject.SetActive(false);
        
        CompanyLogo.gameObject.SetActive(true);
        while (CompanyLogo.alpha < 1)
        {
            CompanyLogo.alpha += Time.deltaTime * FadeSpeed;
            yield return null;
        }
        TextPanel.SetActive(true);
        GuideText.gameObject.SetActive(true);
        StartCoroutine(Typewriter());
    }
    private IEnumerator ClosingTransition()
    {
        BlackScreen.gameObject.SetActive(true);
        while (BlackScreen.alpha < 1)
        {
            BlackScreen.alpha += Time.deltaTime * FadeSpeed;
            yield return null;
        }
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(NextScene);
    }
    public void SkipButtonClicked()
    {
        SkipButtonPopup.SetActive(true);
        Time.timeScale = 0f;
    }
    public void SkipCancelled()
    {
        SkipButtonPopup.SetActive(false);
        Time.timeScale = 1f;
    }
    public void SkipConfirmed()
    {
        SkipButtonPopup.SetActive(false);
        Time.timeScale = 1f;
        StartCoroutine(ClosingTransition());
    }

}
