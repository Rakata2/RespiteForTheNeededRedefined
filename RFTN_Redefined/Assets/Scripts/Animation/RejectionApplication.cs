using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RejectionApplication : MonoBehaviour
{
    public float AnimationDuration = 0.25f;
    public float AnimationDelay = 0.1f;
    public Transform StartPoint;
    public Transform MidPoint;
    public Transform EndPoint;
    private SpriteRenderer SpriteRenderer;
    private Coroutine CurrentAnimation;

    private bool IsInitialized = false;

    public AudioSource PaperSlideSound;
    public float TakePaperSound = 0.7f;
    private void InitializeIfNeeded()
    {
        if (IsInitialized) return;
        SpriteRenderer = GetComponent<SpriteRenderer>();
        if (StartPoint != null)
        {
            transform.position = StartPoint.position;
        }
        SetSpriteAlpha(0f);
        IsInitialized = true;
    }

    public void GiveApplication()
    {
        InitializeIfNeeded();
        gameObject.SetActive(true);
        if (CurrentAnimation != null) StopCoroutine(CurrentAnimation);
        CurrentAnimation = StartCoroutine(AnimateRoutine(StartPoint.position, MidPoint.position, 0f, 1f, false, AnimationDelay));
    }

    public void TakeApplication()
    {
        if (!gameObject.activeInHierarchy) return;
        if (CurrentAnimation != null) StopCoroutine(CurrentAnimation);
        PaperSlideSound.pitch = TakePaperSound;
        PaperSlideSound.PlayOneShot(PaperSlideSound.clip);
        CurrentAnimation = StartCoroutine(AnimateRoutine(MidPoint.position, EndPoint.position, SpriteRenderer.color.a, 0f, true, 0f));
    }

    private IEnumerator AnimateRoutine(Vector3 fromPos, Vector3 toPos, float fromAlpha, float toAlpha, bool deactivateAfter, float delay)
    {
        if(delay > 0f)
        {
            yield return new WaitForSecondsRealtime(delay);
        }

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
