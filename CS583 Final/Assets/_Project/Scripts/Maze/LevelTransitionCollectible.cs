using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitionCollectible : MonoBehaviour
{
    [Header("Scene Settings")]
    public string nextSceneName = "Level2";

    [Header("Pickup Settings")]
    public AudioClip pickupSound;
    public float loadDelay = 0.5f;   // small delay before loading

    [Header("Visual")]
    public GameObject visualObject;  // assign the model here (or leave null to use self)

    private bool collected = false;
    private Collider col;
    private Renderer[] renderers;

    void Awake()
    {
        col = GetComponent<Collider>();

        if (visualObject == null)
            visualObject = this.gameObject;

        renderers = visualObject.GetComponentsInChildren<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected) return;

        // Only react to the player
        if (other.CompareTag("Player"))
        {
            collected = true;

            // Play pickup sound at this position
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }

            // Disable collider so it can't be triggered again
            if (col != null)
                col.enabled = false;

            // Hide visuals without disabling the whole GameObject
            HideVisuals();

            // Now we can safely run a coroutine
            StartCoroutine(LoadNextScene());
        }
    }

    void HideVisuals()
    {
        if (renderers == null) return;

        foreach (var r in renderers)
            r.enabled = false;
    }

    IEnumerator LoadNextScene()
    {
        // optional delay (use unscaled if you ever do this while paused)
        yield return new WaitForSeconds(loadDelay);

        // Try to use your fade component if it exists
        FadeOutOnCommand fade = FindObjectOfType<FadeOutOnCommand>();
        if (fade != null)
        {
            // this works for ANY scene name, not just the main menu
            fade.StartFadeToMenu(nextSceneName);
        }
        else
        {
            // fallback: load directly
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
