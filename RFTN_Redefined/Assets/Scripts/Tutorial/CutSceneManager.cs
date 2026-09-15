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

    public bool IsTyping = false;
    public bool SkipTyping = false;

    public AudioSource TypewriterSoundSource;
    public AudioClip TypewriterSoundClip;

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

    private void Update()
    {
        if (Input.GetMouseButton(0) && IsTyping)
        {
            SkipTyping = true;
        }
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

        IsTyping = true;
        SkipTyping = false;
        foreach (char letter in ListOfTexts.TextList[CurrentLineIndex].ToCharArray())
        {
            if(SkipTyping)
            {
                GuideText.text = ListOfTexts.TextList[CurrentLineIndex];
                break;
            }
            GuideText.text += letter;
            if (TypewriterSoundSource != null && TypewriterSoundClip != null)
            {
                TypewriterSoundSource.PlayOneShot(TypewriterSoundClip);
            }
            yield return new WaitForSeconds(TypingSpeed);
        }
        IsTyping = false;
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
            if (SkipTyping) break;
            ElapsedTime += Time.deltaTime;
            PersonRect.anchoredPosition = Vector2.Lerp(StartPos, TargetPos, ElapsedTime / PersonMoveDuration);
            yield return null;
        }
        if (!SkipTyping) yield return new WaitForSeconds(FadeSpeed);
        DocumentsApproveImage.gameObject.SetActive(true);
        while (DocumentsApproveImage.alpha < 1)
        {
            if (SkipTyping) break;
            DocumentsApproveImage.alpha += Time.deltaTime * FadeSpeed;
            yield return null;
        }
        DatabasePresentImage.gameObject.SetActive(true);
        while (DatabasePresentImage.alpha < 1)
        {
            if (SkipTyping) break;
            DatabasePresentImage.alpha += Time.deltaTime * FadeSpeed;
            yield return null;
        }
        PersonRect.anchoredPosition = TargetPos;
        DocumentsApproveImage.gameObject.SetActive(true);
        DocumentsApproveImage.alpha = 1f;
        DatabasePresentImage.gameObject.SetActive(true);
        DatabasePresentImage.alpha = 1f;
        SkipTyping = false;
    }

    private IEnumerator Sequence3()
    {
        
        QuestionMark.gameObject.SetActive(false);
        DocumentsApproveImage.gameObject.SetActive(false);
        DatabasePresentImage.gameObject.SetActive(false);
        //yield return new WaitForSeconds(0.2f);

        float timer = 0f;
        while (timer < 0.2f && !SkipTyping)
        {
            if (SkipTyping) break;
            timer += Time.deltaTime;
            yield return null;
        }
        if (!SkipTyping) DocumentInconsistency.gameObject.SetActive(true);

        
        timer = 0f;
        while (timer < 0.4f && !SkipTyping)
        {
            if (SkipTyping) break;
            timer += Time.deltaTime;
            yield return null;
        }
        if (!SkipTyping) Arrow.gameObject.SetActive(true);
        
        while (timer < 0.4f && !SkipTyping)
        {
            if (SkipTyping) break;
            timer += Time.deltaTime;
            yield return null;
        }
        if (!SkipTyping) Reject.gameObject.SetActive(true);
        

        DocumentInconsistency.gameObject.SetActive(true);
        Arrow.gameObject.SetActive(true);
        Reject.gameObject.SetActive(true);
        SkipTyping = false;
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
            if (SkipTyping) break;
            PairedDocument.alpha += Time.deltaTime * FadeSpeed;
            yield return null;
        }

        PairedDocument.alpha = 1f;
        SkipTyping = false;
    }

    private IEnumerator IDLetterToApplicationAnimation()
    {
        while (IDDocument.alpha > 0 || LetterDocument.alpha > 0 || PairedDocument.alpha > 0)
        {
            if (SkipTyping) break;
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

        if(!SkipTyping)yield return new WaitForSeconds(0.2f);

        ApplicationDocument.gameObject.SetActive(true);
        ApplicationText.gameObject.SetActive(true);
        while (ApplicationDocument.alpha < 1 || ApplicationText.alpha < 1)
        {
            if (SkipTyping) break;
            float FadeAmount = Time.deltaTime * FadeSpeed;
            ApplicationDocument.alpha += FadeAmount;
            ApplicationText.alpha += FadeAmount;
            yield return null;
        }

        ApplicationDocument.alpha = 1f;
        ApplicationText.alpha = 1f;
        FlashingApplication.SetBool("IsFlashing", true);
        SkipTyping = false;
    }

    private IEnumerator ApplicationTextDisappear()
    {
        while (ApplicationText.alpha > 0)
        {
            if (SkipTyping) break;
            ApplicationText.alpha -= Time.deltaTime * FadeSpeed;
            yield return null;
        }
        ApplicationText.gameObject.SetActive(false);
        SkipTyping = false;
    }

    private IEnumerator EmptyApplicationGiven()
    {
        while(ApplicationDocument.alpha > 0)
        {
            if (SkipTyping) break;
            ApplicationDocument.alpha -= Time.deltaTime * FadeSpeed;
            yield return null;
        }
        ApplicationText.gameObject.SetActive(false);

        if (!SkipTyping) yield return new WaitForSeconds(FadeSpeed);
        

        if(!SkipTyping)yield return new WaitForSeconds(0.2f);

        EmptyApplication.gameObject.SetActive(true);
        while (EmptyApplication.alpha < 1)
        {
            if (SkipTyping) break;
            EmptyApplication.alpha += Time.deltaTime * FadeSpeed;
            yield return null;
        }
        if(!SkipTyping) yield return new WaitForSeconds(FadeSpeed);

        if (!SkipTyping) yield return new WaitForSeconds(0.2f);

        EmptyApplicationText.gameObject.SetActive(true);
        while (EmptyApplicationText.alpha < 1)
        {
            if (SkipTyping) break;
            EmptyApplicationText.alpha += Time.deltaTime * FadeSpeed;
            yield return null;
        }
        if(!SkipTyping) yield return new WaitForSeconds(FadeSpeed);


        EmptyApplicationText.alpha = 1f;
        EmptyApplication.alpha = 1f;
        FlashingEmptyApplication.SetBool("IsFlashing", true);

        SkipTyping = false;
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
