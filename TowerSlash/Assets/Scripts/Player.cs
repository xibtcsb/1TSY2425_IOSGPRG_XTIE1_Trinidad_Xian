using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _dashSpeed = 20f;
    [SerializeField] private float _microDashSpeed = 10f; 
    [SerializeField] private float _dashDuration = 3.5f;
    [SerializeField] private float _microDashDuration = 0.5f; 
    [SerializeField] private float _dashKillRange = 1.5f;
    [SerializeField] private Image _healthBar;
    [SerializeField] private GaugeBar _gaugeBar;
    [SerializeField] private GameObject _gameOverCanvas;

    private bool _isDashing = false;
    private bool _canMicroDash = true; 
    private int _health = 3;
    private int _maxHealth = 3;
    private Vector3 _initialPosition;
    private bool _isSpeedCharacter = false;

    [SerializeField] private float _microDashSwipeThreshold = 50f;
    private Vector2 _swipeStartPosition; 

    private void Start()
    {
        _initialPosition = transform.position;
        UpdateHealthBar();
        _gameOverCanvas.SetActive(false);
    }

    private void Update()
    {
        MovePlayer();
        CheckForTap(); 
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
        _health = _maxHealth = 3;
        _isDashing = false;
        transform.position = _initialPosition;
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

    private void CheckForTap()
    {
        if (Input.GetMouseButtonDown(0) && _canMicroDash) 
        {
            _swipeStartPosition = Input.mousePosition; 
        }

        if (Input.GetMouseButtonUp(0) && _canMicroDash) 
        {
            Vector2 swipeEndPosition = Input.mousePosition;
            float swipeDistance = Vector2.Distance(swipeEndPosition, _swipeStartPosition);

            if (swipeDistance >= _microDashSwipeThreshold)
            {
                StartCoroutine(MicroDash());
            }
        }
    }

    private IEnumerator MicroDash()
    {
        _canMicroDash = false;
        float originalSpeed = _moveSpeed;
        _moveSpeed = _microDashSpeed; 

        yield return new WaitForSeconds(_microDashDuration); 

        _moveSpeed = originalSpeed; 
        GameManager.Instance.AddScore(50); 

        yield return new WaitForSeconds(5f); 
        _canMicroDash = true; 
    }

    private void LoseHealth()
    {
        _health--;
        UpdateHealthBar();
        if (_health <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        _gameOverCanvas.SetActive(true);
        Time.timeScale = 0f; 
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
