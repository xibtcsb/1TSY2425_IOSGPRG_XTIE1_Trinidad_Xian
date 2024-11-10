using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Gun Settings")]
    [SerializeField] protected int _damage;
    [SerializeField] protected int _maxCarryAmmo;
    [SerializeField] protected int _clipCapacity;
    [SerializeField] protected float _reloadSpeed;

    protected int _currentCarryAmmo;
    protected int _currentClipAmmo;
    protected bool _isReloading = false;

    [Header("References")]
    [SerializeField] protected GameObject _bulletPrefab;
    [SerializeField] protected Transform _shootPoint;

    private void Start()
    {
        InitializeAmmo();
    }

    public void ShootButton()
    {
        if (gameObject.activeSelf && !_isReloading)
        {
            Shoot();
        }
    }

    protected void Shoot()
    {
        if (_currentClipAmmo > 0)
        {
            FireBullet();
            _currentClipAmmo--;

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

    protected void FireBullet()
    {
        GameObject bullet = Instantiate(_bulletPrefab, _shootPoint.position, _shootPoint.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDamage(_damage);
        }
    }

    protected void InitializeAmmo()
    {
        _currentCarryAmmo = _maxCarryAmmo;
        _currentClipAmmo = _clipCapacity;
    }

    public int GetCurrentCarryAmmo() 
    {
        return _currentCarryAmmo;
    }

    public int GetCurrentClipAmmo()
    { 
        return _currentClipAmmo;
    }

    public IEnumerator Reload()
    {
        _isReloading = true;
        yield return new WaitForSeconds(_reloadSpeed);

        int ammoNeeded = _clipCapacity - _currentClipAmmo;
        int ammoToReload = Mathf.Min(ammoNeeded, _currentCarryAmmo);

        _currentClipAmmo += ammoToReload;
        _currentCarryAmmo -= ammoToReload;
        _isReloading = false;
    }
}
