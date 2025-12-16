using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // start button
    public void PlayGame()
    {
        SceneManager.LoadScene("Level1");
    }

    // quit button
    public void QuitGame()
    {
        Debug.Log("Quit game");

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
