using TMPro;
using UnityEngine;

public class AmmoTest : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _ammoText;

    void Start()
    {
        if (_ammoText != null)
        {
            _ammoText.text = "Ammo: 30/120";  // Simple test to set text
        }
        else
        {
            Debug.LogWarning("Ammo Text not assigned!");
        }
    }
}
