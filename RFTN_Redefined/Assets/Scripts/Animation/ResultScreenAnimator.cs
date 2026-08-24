using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultScreenAnimator : MonoBehaviour
{
    [Header("Panel settings")]
    public RectTransform PanelRect;
    public CanvasGroup PanelCanvasGroup;
    public Vector2 StartPosition = new Vector2(0, -100f);
    public Vector2 CenterPosition = Vector2.zero;
    public float PanelAnimationTime = 0.5f;

    [Header("Button settings")]
    public GameObject ButtonContainer;
    public CanvasGroup ButtonCanvasGroup;
    public float ButtonDelay = 0.3f;
    public float ButtonFadeTime = 0.4f;


    public void PlayResultAnimation()
    {
        PanelRect.anchoredPosition = StartPosition;
        PanelCanvasGroup.alpha = 0f;

        ButtonContainer.SetActive(false);
        ButtonCanvasGroup.alpha = 0f;
        ButtonCanvasGroup.interactable = false;
        ButtonCanvasGroup.blocksRaycasts = false;

        StartCoroutine(AnimatePanelRoutine());
    }

    private IEnumerator AnimatePanelRoutine()
    {
        float ElapsedTime = 0f;
        while(ElapsedTime < PanelAnimationTime)
        {
            ElapsedTime += Time.deltaTime;
            float T = ElapsedTime / PanelAnimationTime;

            float SmoothT = T * T * (3f - 2f * T);

            PanelRect.anchoredPosition = Vector2.Lerp(StartPosition, CenterPosition, SmoothT);
            PanelCanvasGroup.alpha = Mathf.Lerp(0f, 1f, SmoothT);

            yield return null;
        }

        PanelRect.anchoredPosition = CenterPosition;
        PanelCanvasGroup.alpha = 1f;
        yield return new WaitForSeconds(ButtonDelay);
        StartCoroutine(FadeButtonsRoutine());
    }

    private IEnumerator FadeButtonsRoutine()
    {
        ButtonContainer.SetActive(true);
        float ElapsedTime = 0f;

        while(ElapsedTime < ButtonFadeTime)
        {
            ElapsedTime += Time.deltaTime;
            float T = ElapsedTime / ButtonFadeTime;
            ButtonCanvasGroup.alpha = Mathf .Lerp(0f, 1f, T);
            yield return null;
        }
        ButtonCanvasGroup.alpha = 1f;
        ButtonCanvasGroup.interactable = true;
        ButtonCanvasGroup.blocksRaycasts = true;
    }
    
}
