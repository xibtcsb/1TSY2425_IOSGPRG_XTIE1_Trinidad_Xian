using UnityEngine;
using UnityEngine.SceneManagement;

public enum CharacterType
{
    Default,
    Tank,
    Speed
}

public class GameManager : Singleton<GameManager>
{
    public Transform Player { get; private set; }
    public GaugeBar GaugeBar { get; private set; }
    public CharacterType SelectedCharacter { get; private set; }

    private void Start()
    {
        InitializeGaugeBar();
    }

    private void InitializeGaugeBar()
    {
        GaugeBar = FindObjectOfType<GaugeBar>();
        if (GaugeBar == null)
        {
            Debug.LogError("GaugeBar not found in the scene!");
        }
    }

    public void SelectCharacter(CharacterType characterType)
    {
        SelectedCharacter = characterType;
    }

    public void ResetGame()
    {
        SceneManager.LoadScene("TowerSlashLevel");
    }

    public void InitializePlayer()
    {
        if (Player != null)
            return;

        Player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (Player == null)
        {
            Debug.LogError("Player object not found! Ensure there is a GameObject tagged 'Player' in the TowerSlashLevel scene.");
        }
        else
        {
            DontDestroyOnLoad(Player.gameObject);
        }

        // Reapply the character selection if already set
        if (SelectedCharacter != CharacterType.Default) // Ensure we only apply if a character was selected
        {
            var playerComponent = Player.GetComponent<Player>();
            if (playerComponent != null)
            {
                switch (SelectedCharacter)
                {
                    case CharacterType.Tank:
                        playerComponent.SetTankAttributes();
                        break;
                    case CharacterType.Speed:
                        playerComponent.SetSpeedAttributes();
                        break;
                    default:
                        playerComponent.SetDefaultAttributes();
                        break;
                }
            }
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializePlayer();
    }
}
