using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUIManager : MonoBehaviour
{
    public CanvasGroup FrontPage;
    public CanvasGroup LevelSelect;

    public float FadeDuration = 0.5f;

    public CanvasGroup Fader;

    private void Start()
    {
        if(Fader != null)
        {
            Fader.alpha = 0f;
            Fader.gameObject.SetActive(false);
        }
    }
    public void PlayButton()
    {
        StartCoroutine(SwitchPanels(FrontPage, LevelSelect));
    }
    public void BackButton()
    {
        StartCoroutine(SwitchPanels(LevelSelect, FrontPage));
    }

    private IEnumerator SwitchPanels(CanvasGroup current, CanvasGroup next)
    {
        float elapsed = 0f;
        while (elapsed < FadeDuration)
        {
            elapsed += Time.deltaTime;
            current.alpha = Mathf.Lerp(1f, 0f, elapsed / FadeDuration);
            yield return null;
        }
        current.gameObject.SetActive(false);
        next.alpha = 0f;
        next.gameObject.SetActive(true);
        elapsed = 0f;
        while (elapsed < FadeDuration)
        {
            elapsed += Time.deltaTime;
            next.alpha = Mathf.Lerp(0f, 1f, elapsed / FadeDuration);
            yield return null;
        }
        next.alpha = 1f;
    }

    public void LevelOneButton()
    {
        StartCoroutine(FadeToScene("Level1"));
    }

    public void LevelTwoButton()
    {
        StartCoroutine(FadeToScene("Level2"));
    }

    private IEnumerator FadeToScene(string sceneName)
    {
        if (Fader != null)
        {
            Fader.gameObject.SetActive(true);
            float elapsed = 0f;
            while (elapsed < FadeDuration)
            {
                elapsed += Time.deltaTime;
                Fader.alpha = Mathf.Lerp(0f, 1f, elapsed / FadeDuration);
                yield return null;
            }
        }

        SceneManager.LoadScene(sceneName);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
