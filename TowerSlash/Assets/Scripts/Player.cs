using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _dashSpeed = 20f;
    [SerializeField] private float _dashDuration = 3.5f;
    [SerializeField] private float _dashKillRange = 1.5f;

    private bool _isDashing = false;
    private int _health = 3;

    private void Start()
    {
        UpdateHealthBar();
    }

    private void Update()
    {
        MovePlayer();
    }

    private void FixedUpdate()
    {
        if (_isDashing)
        {
            KillEnemiesInRange();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            LoseHealth();
            Destroy(other.gameObject);
        }
    }

    public void StartDash()
    {
        if (!_isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    public void ResetPlayer()
    {
        _health = 3;
        UpdateHealthBar();
    }

    private void MovePlayer()
    {
        float speed = _isDashing ? _dashSpeed : _moveSpeed;
        Vector3 movement = Vector3.right * speed * Time.deltaTime;
        transform.Translate(movement);
    }

    private IEnumerator Dash()
    {
        _isDashing = true;
        yield return new WaitForSeconds(_dashDuration);
        _isDashing = false;
    }

    public void SetHealth(int health)
    {
        _health = health;
        UpdateHealthBar();
    }

    public void SetSpeed(float speed)
    {
        _moveSpeed = speed;
    }

    public void SetDefaultAttributes()
    {
        SetPlayerAttributes(3, false);
    }

    public void SetTankAttributes()
    {
        SetPlayerAttributes(5, false);
    }

    public void SetSpeedAttributes()
    {
        SetPlayerAttributes(3, true);
    }

    private void SetPlayerAttributes(int health, bool isSpeedCharacter)
    {
        _health = health;
        UpdateHealthBar();
    }

    private void LoseHealth()
    {
        _health--;
        UpdateHealthBar();
        if (_health <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    private void KillEnemiesInRange()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, _dashKillRange);
        foreach (Collider2D enemy in enemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript != null)
                { 
                    enemyScript.KillEnemy();
                    GameManager.Instance.GaugeBar.EnemyKilled();
                }
            }
        }
    }

    private void UpdateHealthBar()
    {
        GameManager.Instance.GaugeBar.UpdateHealth(_health);
    }
}
