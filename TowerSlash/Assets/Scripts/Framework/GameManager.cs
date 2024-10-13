using UnityEngine;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    public Transform Player { get; private set; }
    public GaugeBar GaugeBar { get; private set; }

    private int _score = 0;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject characterSelectCanvas;

    private void Start()
    {
        InitializePlayer();
        InitializeGaugeBar();
        UpdateScoreUI();
        gameOverCanvas.SetActive(false);
        characterSelectCanvas.SetActive(true);  // Make sure character select canvas is active at start
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

    public void OnCharacterSelected() // Call this when the player selects a character
    {
        characterSelectCanvas.SetActive(false);  // Disable character select canvas after character selection
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        gameOverCanvas.SetActive(false);
        characterSelectCanvas.SetActive(false);  // Keep it inactive during restart

        if (Player != null)
        {
            Player.GetComponent<Player>().ResetPlayer();
        }

        _score = 0;
        UpdateScoreUI();
        SpawnerController.Instance.Reset();
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        gameOverCanvas.SetActive(true);
        characterSelectCanvas.SetActive(true);  // Allow character selection on game over
    }
}
