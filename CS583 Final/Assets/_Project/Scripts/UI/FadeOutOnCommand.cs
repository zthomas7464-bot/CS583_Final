using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeOutOnCommand : MonoBehaviour
{
    public CanvasGroup fadeGroup;
    public float fadeDuration = 1.5f;

    public void StartFadeToMenu(string menuSceneName)
    {
        StartCoroutine(FadeAndLoad(menuSceneName));
    }

    IEnumerator FadeAndLoad(string sceneName)
    {
        fadeGroup.alpha = 0f;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            fadeGroup.alpha = Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }

        // Ensure fully faded
        fadeGroup.alpha = 1f;

        // reset timeScale before changing scenes
        Time.timeScale = 1f;

        SceneManager.LoadScene(sceneName);
    }
}
