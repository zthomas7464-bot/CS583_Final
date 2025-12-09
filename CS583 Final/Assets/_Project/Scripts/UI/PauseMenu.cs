using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;   // Panel with the pause menu
    public string mainMenuSceneName = "MainMenu";

    public static bool GameIsPaused = false;

    void Start()
    {
        // Make sure time is normal when Level1 loads
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Press ESC to toggle pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;

        // Lock & hide cursor again for FPS control
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

        // Unlock & show cursor so we can click UI
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        PauseHintFade hint = FindObjectOfType<PauseHintFade>();
        if (hint != null)
            hint.HideInstant();

    }

    public void LoadMainMenu()
    {
        // Find the fade controller
        FadeOutOnCommand fade = FindObjectOfType<FadeOutOnCommand>();

        if (fade != null)
        {
            fade.StartFadeToMenu(mainMenuSceneName);
        }
        else
        {
            // fallback: load instantly
            Time.timeScale = 1f;
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }


    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
