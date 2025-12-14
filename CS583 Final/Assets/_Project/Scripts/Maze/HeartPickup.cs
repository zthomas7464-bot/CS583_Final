using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    [Header("Healing")]
    public int healAmount = 1;

    [Header("Effects")]
    public AudioClip pickupSound;
    public GameObject visualObject;
    public float destroyDelay = 0.1f;

    private Collider col;
    private bool collected = false;

    void Awake()
    {
        col = GetComponent<Collider>();

        if (visualObject == null)
            visualObject = this.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected) return;

        // Check player
        if (!other.CompareTag("Player")) return;


        PlayerHealth ph = other.GetComponent<PlayerHealth>();
        if (ph == null)
        {
            Debug.LogWarning("HeartPickup: PlayerHealth not found on Player.");
            return;
        }

        bool healed = ph.Heal(healAmount);
        Debug.Log("HeartPickup: Heal result = " + healed);

        // If player already at full health, do nothing
        if (!healed) return;

        collected = true;

        // Play pickup sound -- add later
        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }

        // Turn off collider so it can't be picked up again
        if (col != null)
            col.enabled = false;

        // Hide the visual heart
        if (visualObject != null)
            visualObject.SetActive(false);

        // Destroy root after delay
        Destroy(gameObject, destroyDelay);
    }
}
