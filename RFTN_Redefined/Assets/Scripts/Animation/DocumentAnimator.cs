using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocumentAnimator : MonoBehaviour
{
    public float AnimationDuration = 0.25f;
    public Transform StartPoint;
    public Transform EndPoint;
    private SpriteRenderer SpriteRenderer;
    private Coroutine CurrentAnimation;

    private bool IsInitialized = false;

    public AudioSource SlidingPaperSound;
    public float TakeBackDocumentPitch = 0.7f;

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

    public void ShowDocument()
    {
        InitializeIfNeeded();
        gameObject.SetActive(true);
        SlidingPaperSound.PlayOneShot(SlidingPaperSound.clip);
        if(CurrentAnimation != null) StopCoroutine(CurrentAnimation);
        CurrentAnimation = StartCoroutine(AnimateRoutine(StartPoint.position, EndPoint.position, 0f, 1f, false));
    }

    public void HideDocument()
    {
        if (!gameObject.activeInHierarchy) return;
        if(CurrentAnimation != null) StopCoroutine(CurrentAnimation);

        SlidingPaperSound.pitch = TakeBackDocumentPitch;
        SlidingPaperSound.PlayOneShot(SlidingPaperSound.clip);
        
        CurrentAnimation = StartCoroutine(AnimateRoutine(EndPoint.position, StartPoint.position, SpriteRenderer.color.a, 0f, true));
    }

    private IEnumerator AnimateRoutine(Vector3 fromPos, Vector3 toPos, float fromAlpha, float toAlpha, bool deactivateAfter)
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
        transform.position = toPos;
        SetSpriteAlpha(toAlpha);
        if (deactivateAfter)
        {
            gameObject.SetActive(false);
        }
    }

    private void SetSpriteAlpha(float alpha)
    {
        if (SpriteRenderer != null)
        {
            Color color = SpriteRenderer.color;
            color.a = alpha;
            SpriteRenderer.color = color;
        }
    }

}
