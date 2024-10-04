using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelectionUI : MonoBehaviour
{
    public Button defaultButton;
    public Button tankButton;
    public Button speedButton;

    private void Start()
    {
        defaultButton.onClick.AddListener(() => SelectCharacterAndLoadScene(CharacterType.Default));
        tankButton.onClick.AddListener(() => SelectCharacterAndLoadScene(CharacterType.Tank));
        speedButton.onClick.AddListener(() => SelectCharacterAndLoadScene(CharacterType.Speed));
    }

    private void SelectCharacterAndLoadScene(CharacterType characterType)
    {
        GameManager.Instance.SelectCharacter(characterType);
        LoadTowerSlashLevel();
    }

    private void LoadTowerSlashLevel()
    {
        SceneManager.LoadScene("TowerSlashLevel");
    }
}
