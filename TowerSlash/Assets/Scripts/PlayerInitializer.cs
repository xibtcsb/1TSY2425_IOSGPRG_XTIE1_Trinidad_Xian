using UnityEngine;

public class PlayerInitializer : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.InitializePlayer();
        InitializePlayerAttributes();
    }

    private void InitializePlayerAttributes()
    {
        CharacterType selectedCharacter = GameManager.Instance.SelectedCharacter;
        Player player = GetComponent<Player>();

        if (player == null)
        {
            Debug.LogError("Player component not found!");
            return;
        }

        switch (selectedCharacter)
        {
            case CharacterType.Tank:
                player.SetTankAttributes();
                break;
            case CharacterType.Speed:
                player.SetSpeedAttributes();
                break;
            default:
                player.SetDefaultAttributes();
                break;
        }
    }
}
