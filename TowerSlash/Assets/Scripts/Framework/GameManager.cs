using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public Transform Player { get; private set; }

    private void Awake()
    {
        if (Instance == this)
        {
            DontDestroyOnLoad(gameObject);
            Player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetGame()
    {
        Player.GetComponent<Player>()?.ResetPlayer();
        SceneManager.LoadScene("TowerSlashLevel");
    }
}
