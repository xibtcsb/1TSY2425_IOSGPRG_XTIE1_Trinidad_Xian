using UnityEngine;
using UnityEngine.SceneManagement;
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10f); //debug for damage
        }
    }

    public override void TakeDamage(float damageAmount)
    {
        base.TakeDamage(damageAmount);
        UpdateHealthUI();

        if (_hp <= 0)
        {
            GoToGameOverScene();
        }
    }

    private void UpdateHealthUI()
    {
        if (_healthImage != null)
        {
            _healthImage.fillAmount = _hp / _maxHp;
        }
    }

    private void GoToGameOverScene()
    {
        SceneManager.LoadScene("Game Over");
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
