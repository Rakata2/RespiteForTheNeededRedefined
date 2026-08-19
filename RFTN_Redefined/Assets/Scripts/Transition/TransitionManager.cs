using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    public CanvasGroup BlackScreenTransition;
    public float FadeSpeed = 0.5f;
    public static TransitionManager Instance;

    private void Awake()
    {
        if(Instance == null) Instance = this;
    }

    private void Start()
    {
        BlackScreenTransition.gameObject.SetActive(true);
        BlackScreenTransition.alpha = 1f;
        StartCoroutine(StartTransition());
    }
    private IEnumerator StartTransition()
    {
        while (BlackScreenTransition.alpha > 0)
        {
            BlackScreenTransition.alpha -= Time.deltaTime * FadeSpeed;
            yield return null;
        }
        BlackScreenTransition.gameObject.SetActive(false);
    }

    public IEnumerator RetryTransition()
    {
        BlackScreenTransition.gameObject.SetActive(true);
        while (BlackScreenTransition.alpha < 1)
        {
            BlackScreenTransition.alpha += Time.deltaTime * FadeSpeed;
            yield return null;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public IEnumerator MainMenuTransition()
    {
        BlackScreenTransition.gameObject.SetActive(true);
        while (BlackScreenTransition.alpha < 1)
        {
            BlackScreenTransition.alpha += Time.deltaTime * FadeSpeed;
            yield return null;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScene");
    }
}
