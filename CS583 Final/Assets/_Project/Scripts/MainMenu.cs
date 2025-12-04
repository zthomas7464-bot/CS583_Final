using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Called by Start button
    public void PlayGame()
    {
        SceneManager.LoadScene("Level1");
    }

    // Called by Quit button
    public void QuitGame()
    {
        Debug.Log("Quit game");   // will show in editor

        Application.Quit();

        // So it also stops play mode in the editor:
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
