using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 2f;
    [SerializeField] private float _detectionRadius = 5f;
    [SerializeField] private float _changeDirectionTime = 2f;
    [SerializeField] private float _randomMoveDistance = 3f;
    [SerializeField] private GameObject[] _guns;
    [SerializeField] private Transform _player;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private Transform _weaponInstantiatePoint;

    private GameObject _currentGun;
    private Vector2 _targetPosition;
    private float _changeDirectionTimer;

    private enum State { Wander, Chase, Shoot }
    private State _currentState;

    void Start()
    {
        _currentState = State.Wander;
        SetNewRandomPosition();
        _changeDirectionTimer = _changeDirectionTime;
        AssignRandomGun();
    }

    void Update()
    {
        switch (_currentState)
        {
            case State.Wander:
                Wander();
                break;
            case State.Chase:
                ChasePlayer();
                break;
            case State.Shoot:
                ShootPlayer();
                break;
        }

        DetectPlayer();
    }

    private void Wander()
    {
        transform.position = Vector2.MoveTowards(transform.position, _targetPosition, _movementSpeed * Time.deltaTime);
        _changeDirectionTimer -= Time.deltaTime;
        if (_changeDirectionTimer <= 0f)
        {
            SetNewRandomPosition();
            _changeDirectionTimer = _changeDirectionTime;
        }
    }

    private void SetNewRandomPosition()
    {
        float randomX = Random.Range(-_randomMoveDistance, _randomMoveDistance);
        float randomY = Random.Range(-_randomMoveDistance, _randomMoveDistance);
        _targetPosition = (Vector2)transform.position + new Vector2(randomX, randomY);
    }

    private void DetectPlayer()
    {
        if (Vector2.Distance(transform.position, _player.position) < _detectionRadius)
        {
            _currentState = State.Chase;
        }
    }

    private void ChasePlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, _player.position, _movementSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, _player.position) < 1f)
        {
            _currentState = State.Shoot;
        }
    }

    private void ShootPlayer()
    {
        if (_currentGun != null && _bulletPrefab != null && _bulletSpawnPoint != null)
        {
            GameObject bullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, Quaternion.identity);
            Vector2 direction = (_player.position - _bulletSpawnPoint.position).normalized;
            bullet.GetComponent<Rigidbody2D>().velocity = direction * 10f;
            _currentState = State.Chase;
        }
        else
        {
            Debug.LogError("Gun or bullet prefab is not assigned!");
        }
    }

    private void AssignRandomGun()
    {
        if (_guns.Length > 0)
        {
            int randomIndex = Random.Range(0, _guns.Length);
            _currentGun = Instantiate(_guns[randomIndex], _weaponInstantiatePoint.position, Quaternion.identity, transform);
            _currentGun.transform.localPosition = Vector3.zero;
            Debug.Log("Gun instantiated: " + _currentGun.name);
        }
        else
        {
            Debug.LogError("No guns available in the array!");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }
}
