using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _bulletSpan = 5.5f;
    private int _damage;

    private void Start()
    {
        Destroy(gameObject, _bulletSpan);
    }

    private void Update()
    {
        BulletShoot();
    }

    private void BulletShoot()
    {
        transform.position += transform.up * Time.deltaTime * _speed;
    }

    public void SetDamage(int damage)
    {
        _damage = damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyAI enemy = collision.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.TakeDamage(_damage);
            }
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Player"))
        {
            Unit player = collision.GetComponent<Unit>();
            if (player != null)
            {
                OnPlayerHit(player);
            }
            Destroy(gameObject);
        }
    }

    private void OnPlayerHit(Unit player)
    {
        int playerDamage = 12;
        player.TakeDamage(playerDamage);
        Debug.Log("Player hit! Damage: " + playerDamage);
    }
}
