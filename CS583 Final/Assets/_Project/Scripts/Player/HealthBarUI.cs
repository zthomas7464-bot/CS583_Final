using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [Header("References")]
    public PlayerHealth playerHealth;
    public Slider healthSlider;
    public TextMeshProUGUI healthText;
    public Image damageFlashImage;

    [Header("Damage Flash Settings")]
    public float flashDuration = 0.4f;
    [Range(0f,1f)]
    public float maxFlashAlpha = 0.35f;


    [Header("Low Health Settings")]
    [Range(0f, 1f)]
    public float lowHealthThreshold = 0.33f;

    private float flashTimer = 0f;
    private Color originalTextColor;
    private Color lowHealthColor;

    // Start is called before the first frame update
    void Start()
    {
        if (playerHealth == null)
        {
            playerHealth = FindObjectOfType<PlayerHealth>();
        }

        if (playerHealth == null)
        {
            Debug.LogError("HealthBarUI: Reference to PlayerHealth is missing");
            enabled = false;
            return;
        }

        //Inititalize with range and current value
        if (healthSlider != null){
            healthSlider.maxValue = playerHealth.maxHealth;
            healthSlider.value = playerHealth.CurrentHealth;
        }

        if (healthText != null)
        {
            originalTextColor = healthText.color;
        }

        Color parsedColor;
        if (ColorUtility.TryParseHtmlString("#8a0303", out parsedColor))
        {
            lowHealthColor = parsedColor;
        }
        else
        {
            lowHealthColor = Color.red;
        }

        if (damageFlashImage != null)
        {
            Color c = damageFlashImage.color;
            c.a = 0f;
            damageFlashImage.color = c;
        }
        
        //update the HP Text
        UpdateHealthText();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth == null) return;

        //update the slider so it matches current health
        if (healthSlider != null)
        {
            healthSlider.value = playerHealth.CurrentHealth;
        }

        //update the HP Text
        UpdateHealthText();

        //Low health warning
        if (healthText != null)
        {
            float healthPercent = (float)playerHealth.CurrentHealth / playerHealth.maxHealth;
            if (healthPercent <= lowHealthThreshold)
            {
                healthText.color = lowHealthColor;
            }
            else
            {
                healthText.color = originalTextColor;
            }
        }

        //Damage flash fade out
        if (damageFlashImage != null && flashTimer > 0f)
        {
            flashTimer -= Time.deltaTime;

            float t = 1f - (flashTimer / flashDuration);
            t = Mathf.Clamp01(t);

            // Smooth easing from maxFlashAlpha to 0
            float alpha = Mathf.SmoothStep(maxFlashAlpha, 0f, t);

            Color c = damageFlashImage.color;
            c.a = alpha;
            damageFlashImage.color = c;
        }
    }

    public void TriggerDamageFlash()
    {
        if (damageFlashImage == null)
            return;

        flashTimer = flashDuration;

        Color c = damageFlashImage.color;
        c.a = maxFlashAlpha;
        damageFlashImage.color = c;
    }

    void UpdateHealthText()
    {
        if (healthText == null) return;

        healthText.text = $"HP: {playerHealth.CurrentHealth} / {playerHealth.maxHealth}";
    }
}
