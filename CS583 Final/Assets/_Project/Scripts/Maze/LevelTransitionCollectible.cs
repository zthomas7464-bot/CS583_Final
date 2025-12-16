using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitionCollectible : MonoBehaviour
{
    [Header("Scene Settings")]
    public string nextSceneName = "Level2";

    [Header("Pickup Settings")]
    public AudioClip pickupSound;
    public float loadDelay = 0.5f;

    [Header("Visual")]
    public GameObject visualObject;

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

            // Play pickup sound
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }

            if (col != null)
                col.enabled = false;

            // Hide visuals without disabling the whole GameObject
            HideVisuals();

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
        // delay
        yield return new WaitForSeconds(loadDelay);

        // Mark Level 2 as unlocked
        PlayerPrefs.SetInt("Level2Unlocked", 1);
        PlayerPrefs.Save();

        //use fade
        FadeOutOnCommand fade = FindObjectOfType<FadeOutOnCommand>();
        if (fade != null)
        {
            fade.StartFadeToMenu(nextSceneName); 
        }
        else
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
