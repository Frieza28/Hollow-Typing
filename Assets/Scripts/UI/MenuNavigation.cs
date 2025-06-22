using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNavigation : MonoBehaviour
{
    public void GoToLevelSelection() { SceneManager.LoadScene("Level Selection"); }
    public void GoToControls() { SceneManager.LoadScene("Controls"); }
    public void GoToCredits() { SceneManager.LoadScene("Credits"); }
    public void GoToOptions() { SceneManager.LoadScene("Options"); }
    public void GoToMainMenu() { SceneManager.LoadScene("MainMenu"); }
    public void QuitToDesktop() {  
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif 
    }
}
