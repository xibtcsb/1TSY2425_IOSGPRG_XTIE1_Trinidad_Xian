using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : Singleton<SceneSwitch>
{
    public void RetryScene()
    {
        LoadScene("TowerSlashLevel");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
