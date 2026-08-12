using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrayItemAnimator : MonoBehaviour
{
    public float AnimationDuration = 0.25f;
    public Transform StartPoint;
    public Transform EndPoint;
    private SpriteRenderer SpriteRenderer;
    private Coroutine CurrentAnimation;
    private bool IsInitialized = false;
    public static TrayItemAnimator instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void InitializeIfNeeded()
    {
        if (IsInitialized) return;
        SpriteRenderer = GetComponent<SpriteRenderer>();
        if(StartPoint!= null)
        {
            transform.position = StartPoint.position;
        }
        SetSpriteAlpha(0f);
        IsInitialized = true;
    }

    public void ShowItem()
    {
        InitializeIfNeeded();
        gameObject.SetActive(true);
        if(CurrentAnimation != null) StopCoroutine(CurrentAnimation);
        CurrentAnimation = StartCoroutine(AnimateRoutine(StartPoint.position, EndPoint.position, 0f, 1f, false));
    }

    public void HideItem()
    {
        gameObject.SetActive(false);
    }


    private IEnumerator AnimateRoutine(Vector3 fromPos, Vector3 toPos, float fromAlpha, float toAlpha, bool DeactivateAfter)
    {
        float time = 0f;
        while (time < AnimationDuration)
        {
            time += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(time / AnimationDuration);
            transform.position = Vector3.Lerp(fromPos, toPos, t);
            SetSpriteAlpha(Mathf.Lerp(fromAlpha, toAlpha, t));
            yield return null;
        }

        if(DeactivateAfter == true)
        {
            gameObject.SetActive(false);
        }
    }
    public void SetSpriteAlpha(float alpha)
    {
        if(SpriteRenderer != null)
        {
            Color color = SpriteRenderer.color;
            color.a = alpha;
            SpriteRenderer.color = color;
        }
    }
}
