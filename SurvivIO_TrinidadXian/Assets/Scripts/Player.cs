using UnityEngine;
using UnityEngine.UI;

public class Player : Unit
{
    private int _pistolAmmo;
    private int _shotgunAmmo;
    private int _assaultRifleAmmo;

    [SerializeField] private Image _healthImage;

    protected override void Start()
    {
        base.Start();
        UpdateHealthUI();
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
            _healthImage.fillAmount = _hp / _maxHp;
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
