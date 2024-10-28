using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Gun Settings")]
    [SerializeField] private int _damage = 10;
    [SerializeField] private int _maxCarryAmmo = 30;
    [SerializeField] private int _clipCapacity = 10;
    [SerializeField] private float _reloadSpeed = 1.5f;

    private int _currentCarryAmmo;
    private int _currentClipAmmo;
    private bool _isReloading = false;

    [Header("References")]
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _shootPoint;

    private void Start()
    {
        InitializeAmmo();
    }

    public void ShootButton()
    {
        if (!_isReloading)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (_currentClipAmmo > 0)
        {
            FireBullet();
            _currentClipAmmo--;

            Debug.Log("Shot fired. Bullets left in clip: " + _currentClipAmmo);

            if (_currentClipAmmo == 0 && _currentCarryAmmo > 0)
            {
                StartCoroutine(Reload());
            }
        }
        else
        {
            Debug.Log("Clip empty. Need to reload!");
        }
    }

    private void FireBullet()
    {
        Instantiate(_bulletPrefab, _shootPoint.position, _shootPoint.rotation);
    }

    private void InitializeAmmo()
    {
        _currentCarryAmmo = _maxCarryAmmo;
        _currentClipAmmo = _clipCapacity;

        Debug.Log("Gun initialized. Carry Ammo: " + _currentCarryAmmo + ", Clip Ammo: " + _currentClipAmmo);
    }

    private IEnumerator Reload()
    {
        _isReloading = true;
        Debug.Log("Reloading...");

        yield return new WaitForSeconds(_reloadSpeed);

        int ammoNeeded = _clipCapacity - _currentClipAmmo;
        int ammoToReload = Mathf.Min(ammoNeeded, _currentCarryAmmo);

        _currentClipAmmo += ammoToReload;
        _currentCarryAmmo -= ammoToReload;
        _isReloading = false;

        Debug.Log("Reload complete. Current clip: " + _currentClipAmmo + ", Carry Ammo: " + _currentCarryAmmo);
    }
}
