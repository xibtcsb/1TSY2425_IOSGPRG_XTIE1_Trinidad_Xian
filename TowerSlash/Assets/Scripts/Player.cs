using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _dashSpeed = 20f;
    [SerializeField] private float _dashDuration = 3.5f;
    [SerializeField] private float _dashKillRange = 1.5f;

    [SerializeField] private Image _healthBar;
    [SerializeField] private GaugeBar _gaugeBar;

    private bool _isDashing = false;
    private int _health = 3;
    private int _maxHealth = 3;
    private bool _isSpeedCharacter = false;

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
        Debug.Log("ResetPlayer called. Health reset to 3.");
        _health = 3;
        _maxHealth = 3; 
        UpdateHealthBar();
    }

    public void SetHealth(int health)
    {
        _health = health;
        _maxHealth = health;
        UpdateHealthBar();
        Debug.Log($"SetHealth called with: {health}. Current Health: {_health}, Max Health: {_maxHealth}");
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

    private void LoseHealth()
    {
        _health--;
        UpdateHealthBar();
        if (_health <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    private void UpdateHealthBar()
    {
        if (_healthBar != null)
        {
            _healthBar.fillAmount = (float)_health / _maxHealth;
            Debug.Log($"HealthBar Updated: {_health} / {_maxHealth}");
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
                    _gaugeBar.EnemyKilled(_isSpeedCharacter);
                }
            }
        }
    }
}
