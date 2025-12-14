using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnManager : MonoBehaviour
{
    [Header("References")]
    public PlayerHealth playerHealth;
    public Transform[] spawnPoints;

    private CharacterController controller;

    void Awake()
    {
        if (playerHealth == null)
            playerHealth = FindObjectOfType<PlayerHealth>();

        if (playerHealth != null)
            controller = playerHealth.GetComponent<CharacterController>();
    }

    // Called by PlayerHealth when the player "dies"
    public void RespawnPlayer()
    {
        if (playerHealth == null || spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("RespawnManager: Missing player or spawn points!");
            // Fallback: reload scene so game still works
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.buildIndex);
            return;
        }

        Transform spawn = GetRandomSpawnPoint();

        // Reset health
        playerHealth.ResetHealth();

        // Teleport player safely
        if (controller != null)
            controller.enabled = false;

        playerHealth.transform.position = spawn.position;
        playerHealth.transform.rotation = spawn.rotation;

        if (controller != null)
            controller.enabled = true;
    }

    Transform GetRandomSpawnPoint()
    {
        int index = Random.Range(0, spawnPoints.Length);
        Debug.Log("RespawnManager: choosing spawn index " + index);
        return spawnPoints[index];
    }
}
