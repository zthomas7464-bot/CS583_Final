using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressAnyKeyToStart : MonoBehaviour
{
    [Header("UI References")]
    public CanvasGroup fadeGroup; // Black image CanvasGroup
    public CanvasGroup pressAnyKeyGroup;

    [Header("Settings")]
    public float fadeDuration = 1.5f;
    public string levelToLoad = "Level1";

    private bool hasPressed = false;

    void Start()
    {
        // Ensure starting state
        if (fadeGroup != null)
            fadeGroup.alpha = 0f;

        if (pressAnyKeyGroup != null)
            pressAnyKeyGroup.alpha = 1f;
    }

    void Update()
    {
        if (!hasPressed && Input.anyKeyDown)
        {
            hasPressed = true;
            StartCoroutine(FadeAndLoad());
        }
    }

    private IEnumerator FadeAndLoad()
    {
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float normalized = Mathf.Clamp01(t / fadeDuration);

            // Fade panel to black
            if (fadeGroup != null)
                fadeGroup.alpha = normalized;

            //Fade out text
            if (pressAnyKeyGroup != null)
                pressAnyKeyGroup.alpha = 1f - normalized;

            yield return null;
        }

        // Make sure fully black at the end
        if (fadeGroup != null)
            fadeGroup.alpha = 1f;

        // Load game scene
        SceneManager.LoadScene(levelToLoad);
    }
}
