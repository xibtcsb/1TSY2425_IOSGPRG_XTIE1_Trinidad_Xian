using UnityEngine;
using UnityEngine.UI;

public class Player : Unit
{
    private Gun _currentGun;
    private int _pistolAmmo;
    private int _shotgunAmmo;
    private int _assaultRifleAmmo;

    [SerializeField] private Image _healthImage;  // Reference to the health image UI element

    protected override void Start()
    {
        base.Start();
        UpdateHealthUI();  // Initialize health UI at the start
    }

    public override void TakeDamage(float damageAmount)
    {
        base.TakeDamage(damageAmount);
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (_healthImage != null)
        {
            _healthImage.fillAmount = _hp / _maxHp;  // Update the fill amount based on current health
        }
    }

    public void SetCurrentGun(Gun newGun)
    {
        if (_currentGun != null)
        {
            _currentGun.gameObject.SetActive(false);
        }

        _currentGun = newGun;
        _currentGun.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) // For testing, press T to take damage
        {
            TakeDamage(10f); // Example call to TakeDamage
        }
    }

    public void AddAmmo(AmmoPickup.AmmoType ammoType, int amount)
    {
        switch (ammoType)
        {
            case AmmoPickup.AmmoType.Pistol:
                _pistolAmmo += amount;
                Debug.Log("Pistol ammo added: " + amount + ". Total: " + _pistolAmmo);
                break;
            case AmmoPickup.AmmoType.Shotgun:
                _shotgunAmmo += amount;
                Debug.Log("Shotgun ammo added: " + amount + ". Total: " + _shotgunAmmo);
                break;
            case AmmoPickup.AmmoType.AssaultRifle:
                _assaultRifleAmmo += amount;
                Debug.Log("Assault Rifle ammo added: " + amount + ". Total: " + _assaultRifleAmmo);
                break;
        }
    }
}
