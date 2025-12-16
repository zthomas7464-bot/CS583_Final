using System.Collections;
using UnityEngine;

public class PauseHintFade : MonoBehaviour
{
    public CanvasGroup hintGroup;
    public float showTime = 30f;
    public float fadeDuration = 2f;

    void Start()
    {
        if (hintGroup == null)
            hintGroup = GetComponent<CanvasGroup>();

        // make sure it's visible at start
        if (hintGroup != null)
            hintGroup.alpha = 1f;

        StartCoroutine(ShowAndFade());
    }

    IEnumerator ShowAndFade()
    {
        // wait while fully visible
        yield return new WaitForSeconds(showTime);

        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float normalized = Mathf.Clamp01(t / fadeDuration);

            if (hintGroup != null)
                hintGroup.alpha = 1f - normalized;

            yield return null;
        }

        // ensure fully invisible
        if (hintGroup != null)
            hintGroup.alpha = 0f;

        gameObject.SetActive(false);
    }

    public void HideInstant()
    {
        if (hintGroup != null)
            hintGroup.alpha = 0f;

        gameObject.SetActive(false);
    }

}
