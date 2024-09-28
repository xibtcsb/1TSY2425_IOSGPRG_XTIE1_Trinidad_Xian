using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    public void RetryScene()
    {
        SceneManager.LoadScene("TowerSlashLevel");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
