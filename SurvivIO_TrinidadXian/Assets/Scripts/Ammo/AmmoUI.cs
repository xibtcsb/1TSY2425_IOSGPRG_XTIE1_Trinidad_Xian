using UnityEngine;
using TMPro;

public class AmmoUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI _assaultRifleAmmoText;
    [SerializeField] private TextMeshProUGUI _shotgunAmmoText;
    [SerializeField] private TextMeshProUGUI _pistolAmmoText;

    [SerializeField] private AssaultRifle _assaultRifle;
    [SerializeField] private Shotgun _shotgun;
    [SerializeField] private Pistol _pistol;

    private void Start()
    {
        if (_assaultRifle != null) UpdateAmmoUI(_assaultRifle, _assaultRifleAmmoText);
        if (_shotgun != null) UpdateAmmoUI(_shotgun, _shotgunAmmoText);
        if (_pistol != null) UpdateAmmoUI(_pistol, _pistolAmmoText);
    }

    private void Update()
    {
        if (_assaultRifle != null) UpdateAmmoUI(_assaultRifle, _assaultRifleAmmoText);
        if (_shotgun != null) UpdateAmmoUI(_shotgun, _shotgunAmmoText);
        if (_pistol != null) UpdateAmmoUI(_pistol, _pistolAmmoText);
    }

    private void UpdateAmmoUI(Gun gun, TextMeshProUGUI ammoText)
    {
        if (ammoText != null && gun != null)
        {
            ammoText.text = $"{gun.GetCurrentCarryAmmo()}/{gun.GetCurrentClipAmmo()}";
        }
    }
}
