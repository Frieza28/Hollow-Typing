using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public void LoadLevel1()  { SceneManager.LoadScene("Level1"); }
    public void LoadLevel2()  { SceneManager.LoadScene("Level2"); }
    public void LoadLevel3()  { SceneManager.LoadScene("Level3"); }
    public void LoadBoss()    { SceneManager.LoadScene("BossLevel"); }
    public void GoToMainMenu() { SceneManager.LoadScene("MainMenu"); }
}
