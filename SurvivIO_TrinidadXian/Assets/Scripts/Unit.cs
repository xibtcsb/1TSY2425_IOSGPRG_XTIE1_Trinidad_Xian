using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] protected float _hp;
    [SerializeField] protected float _maxHp = 100f;
    [HideInInspector]
    [SerializeField] public Gun _currentGun;

    [SerializeField] protected FloatingHealthbar _healthBar;

    protected virtual void Start()
    {
        _hp = _maxHp;

        if (_healthBar == null)
        {
            _healthBar = GetComponentInChildren<FloatingHealthbar>();
        }
    }

    public virtual void TakeDamage(float damageAmount)
    {
        _hp -= damageAmount;
        Debug.Log("Damage taken: " + damageAmount + ". Current HP: " + _hp);

        if (_healthBar != null)
        {
            _healthBar.UpdateHealthBar(_hp, _maxHp); 
        }

        if (_hp <= 0)
        {
            Die();
        }
    }

    public virtual void EnemyShoot()
    {
        if (_currentGun != null && !_currentGun._isReloading)
        {
            if (_currentGun.GetCurrentClipAmmo() > 0)
            {
                _currentGun.ShootButton();
            }
            else
            {
                StartCoroutine(_currentGun.Reload());
            }
        }
    }

    public bool IsAlive()
    {
        return _hp > 0;
    }

    protected virtual void Die()
    {
        Debug.Log(this.gameObject.name + " has died.");
        Destroy(this.gameObject);
    }
}
