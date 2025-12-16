using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectMenu : MonoBehaviour
{
    [Header("Buttons")]
    public Button level1Button;
    public Button level2Button;

    [Header("Scene Names")]
    public string level1SceneName = "Level1";
    public string level2SceneName = "Level2";

    void Start()
    {
        // Always allow Level 1
        if (level1Button != null)
            level1Button.onClick.AddListener(LoadLevel1);

        // Unlock state for Level 2
        bool level2Unlocked = PlayerPrefs.GetInt("Level2Unlocked", 0) == 1;

        if (level2Button != null)
        {
            Text label = level2Button.GetComponentInChildren<Text>();
            if (label != null)
                label.text = "level 2";

            level2Button.onClick.AddListener(LoadLevel2);

            //Lock or unlock button
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
