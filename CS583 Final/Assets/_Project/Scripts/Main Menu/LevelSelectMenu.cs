using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelectMenu : MonoBehaviour
{
    [Header("Buttons")]
    public Button level1Button;
    public Button level2Button;

    [Header("Labels")]
    public TMP_Text level2Label;

    [Header("Scene Names")]
    public string level1SceneName = "Level1";
    public string level2SceneName = "Level2";

    void Start()
    {
        // Always allow Level 1
        if (level1Button != null)
            level1Button.onClick.AddListener(LoadLevel1);

        // Check if Level 2 is unlocked
        bool level2Unlocked = PlayerPrefs.GetInt("Level2Unlocked", 0) == 1;

        // Configure Level 2 button
        if (level2Button != null)
        {
            level2Button.onClick.AddListener(LoadLevel2);

            // Update label text
            if (level2Label != null)
            {
                if (level2Unlocked)
                    level2Label.text = "level 2";
                else
                    level2Label.text = "level 2 (locked)";
            }

            // Enable/disable button
            level2Button.interactable = level2Unlocked;
        }
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene(level1SceneName);
    }

    public void LoadLevel2()
    {
        bool level2Unlocked = PlayerPrefs.GetInt("Level2Unlocked", 0) == 1;
        if (!level2Unlocked)
            return;

        SceneManager.LoadScene(level2SceneName);
    }
}
