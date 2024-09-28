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

    private void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        float speed = _isDashing ? _dashSpeed : _moveSpeed;
        Vector3 movement = Vector3.right * speed * Time.deltaTime;
        transform.Translate(movement);
    }

    public void StartDash()
    {
        if (!_isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        _isDashing = true;

        yield return new WaitForSeconds(_dashDuration);
        _isDashing = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            LoseHealth();
            Destroy(other.gameObject);
        }
    }

    private void LoseHealth()
    {
        _health--;
        if (_health <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    public void ResetPlayer()
    {
        _health = 3;
    }

    private void FixedUpdate()
    {
        if (_isDashing)
        {
            KillEnemiesInRange();
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
                }
            }
        }
    }
}
