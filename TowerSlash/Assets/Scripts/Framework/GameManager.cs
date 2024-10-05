using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    public Transform Player { get; private set; }
    public GaugeBar GaugeBar { get; private set; }

    private int _score = 0; 

    [SerializeField] private TextMeshProUGUI scoreText;  

    private void Start()
    {
        InitializePlayer();
        InitializeGaugeBar();
        UpdateScoreUI();  
    }

    private void InitializePlayer()
    {
        Player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void InitializeGaugeBar()
    {
        GaugeBar = FindObjectOfType<GaugeBar>();
    }

    public void AddScore(int points)
    {
        _score += points;
        UpdateScoreUI(); 
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + _score.ToString();
        }
    }

    public void ResetGame()
    {
        Player?.GetComponent<Player>()?.ResetPlayer();
        SceneManager.LoadScene("TowerSlashLevel");
    }
}
