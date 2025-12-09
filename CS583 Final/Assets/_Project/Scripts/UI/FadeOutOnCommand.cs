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
        // Make sure fadeGroup starts at 0
        fadeGroup.alpha = 0f;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime; // bypass timeScale = 0
            fadeGroup.alpha = Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }

        // Ensure fully black
        fadeGroup.alpha = 1f;

        // Reset timeScale before changing scenes
        Time.timeScale = 1f;

        SceneManager.LoadScene(sceneName);
    }
}
