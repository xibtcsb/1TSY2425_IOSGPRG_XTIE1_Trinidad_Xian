using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] protected float _hp;
    [SerializeField] protected float _maxHp = 100f;

    protected virtual void Start()
    {
        _hp = _maxHp;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) 
        {
            TakeDamage(10f); 
        }
    }
    public virtual void TakeDamage(float damageAmount)
    {
        _hp -= damageAmount;
        Debug.Log("Damage taken: " + damageAmount + ". Current HP: " + _hp);

        if (_hp <= 0)
        {
            Die();
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
