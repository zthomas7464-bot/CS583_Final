using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3; //Max
    private int currentHealth; // What its at rn

    void Start()
    {
        //Set health at start
        currentHealth = maxHealth;
    }

    //Change health when damage is taken
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        //Die if health is 0 or less
        if (currentHealth <= 0)
            Die();
    }

    //As of now just resets level --> might change to a death screen
    void Die()
    {
        //Get active scene and reset it
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }
}
