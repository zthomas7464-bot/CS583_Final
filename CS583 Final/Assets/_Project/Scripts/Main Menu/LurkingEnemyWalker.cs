using System.Collections;
using UnityEngine;

public class LurkingEnemyWalker : MonoBehaviour
{
    [Header("Path")]
    public Transform startPoint;
    public Transform endPoint;

    [Header("Timing")]
    public float moveDuration = 4f;      // time to walk from start to end
    public float minDelay = 5f;          // min time before it appears again
    public float maxDelay = 15f;         // max time before it appears again

    [Header("Appearance")]
    public bool hideBetweenWalks = true; // disable renderer between walks

    private Renderer[] renderers;

    void Awake()
    {
        // cache all renderers so we can show/hide
        renderers = GetComponentsInChildren<Renderer>();
    }

    void Start()
    {
        // start at the startPoint
        if (startPoint != null)
        {
            transform.position = startPoint.position;
            transform.rotation = startPoint.rotation;
        }

        StartCoroutine(WalkLoop());
    }

    IEnumerator WalkLoop()
    {
        while (true)
        {
            // wait some time before next walk
            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);

            // make sure at start
            if (startPoint != null)
            {
                transform.position = startPoint.position;
                transform.rotation = startPoint.rotation;
            }

            // show enemy if hidden
            if (hideBetweenWalks)
                SetVisible(true);

            // do the walk
            yield return StartCoroutine(WalkOnce());

            // hide after done
            if (hideBetweenWalks)
                SetVisible(false);
        }
    }

    IEnumerator WalkOnce()
    {
        if (startPoint == null || endPoint == null)
            yield break;

        float t = 0f;

        while (t < moveDuration)
        {
            t += Time.deltaTime;
            float normalized = Mathf.Clamp01(t / moveDuration);

            // move along the path
            transform.position = Vector3.Lerp(startPoint.position, endPoint.position, normalized);

            yield return null;
        }

        // ensure at end
        transform.position = endPoint.position;
    }

    void SetVisible(bool visible)
    {
        if (renderers == null) return;

        foreach (Renderer r in renderers)
        {
            r.enabled = visible;
        }
    }
}
