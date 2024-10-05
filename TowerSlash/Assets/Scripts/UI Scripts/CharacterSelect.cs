using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
    [SerializeField] private GameObject characterSelectPanel;
    [SerializeField] private Button defaultButton;
    [SerializeField] private Button tankButton;
    [SerializeField] private Button speedButton;

    [SerializeField] private Player player;

    private void Start()
    {
        ShowCharacterSelectPanel();

        defaultButton.onClick.AddListener(() => SelectCharacter(3));
        tankButton.onClick.AddListener(() => SelectCharacter(5));
        speedButton.onClick.AddListener(() => SelectCharacter(3));
    }

    private void ShowCharacterSelectPanel()
    {
        characterSelectPanel.SetActive(true);
        Time.timeScale = 0f; 
    }

    private void SelectCharacter(int health)
    {
        Debug.Log($"Selecting character with health: {health}");
        player.SetHealth(health);

        characterSelectPanel.SetActive(false);
        Time.timeScale = 1f; 
    }
}
