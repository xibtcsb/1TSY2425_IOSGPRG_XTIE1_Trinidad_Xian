using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public Transform Player { get; private set; }
    public GaugeBar GaugeBar { get; private set; } 

    private void Start()
    {
        InitializePlayer();
        InitializeGaugeBar();
    }

    private void InitializePlayer()
    {
        Player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void InitializeGaugeBar()
    {
        GaugeBar = FindObjectOfType<GaugeBar>();
    }

    public void ResetGame()
    {
        Player?.GetComponent<Player>()?.ResetPlayer();
        SceneManager.LoadScene("TowerSlashLevel");
    }
}
