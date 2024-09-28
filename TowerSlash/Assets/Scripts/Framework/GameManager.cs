using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Transform Player { get; private set; }

    private void Awake()
    {
        if (Instance == this)
        {
            DontDestroyOnLoad(gameObject);
            InitializePlayer();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePlayer()
    {
        Player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    public void ResetGame()
    {
        Player?.GetComponent<Player>()?.ResetPlayer();
        SceneManager.LoadScene("TowerSlashLevel");
    }
}
