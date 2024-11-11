using UnityEngine;

public class EnemyAI : Unit
{
    [SerializeField] private float _movementSpeed = 2f;
    [SerializeField] private float _detectionRadius = 5f;
    [SerializeField] private float _changeDirectionTime = 2f;
    [SerializeField] private float _randomMoveDistance = 3f;
    [SerializeField] private GameObject[] _guns;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private Transform _weaponInstantiatePoint;

    private Vector2 _targetPosition;
    private float _changeDirectionTimer;

    private enum State { Wander, Chase, Shoot }
    private State _currentState;

    private float _shootCooldown = .6f;
    private float _shootCooldownTimer = 0f;

    protected override void Start()
    {
        base.Start();

        _currentState = State.Wander;
        SetNewRandomPosition();
        _changeDirectionTimer = _changeDirectionTime;
        AssignRandomGun();

        if (_player == null)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null)
            {
                _player = playerObject;
                Debug.Log("Player found: " + _player.name);
            }
            else
            {
                Debug.LogError("Player object not found! Ensure there's a Player object with the tag 'Player' in the scene.");
            }
        }
    }

    void Update()
    {
        if (!IsAlive()) return;

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
        if (_player == null)
        {
            Debug.LogError("Player reference is missing!");
            return;
        }

        if (Vector2.Distance(transform.position, _player.transform.position) < _detectionRadius)
        {
            Debug.Log("Player detected! Transitioning to Chase.");
            _currentState = State.Chase;
        }
        else
        {
            Debug.Log("Player not in detection range.");
        }
    }

    private void ChasePlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, _movementSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, _player.transform.position) < 2f)
        {
            Debug.Log("Chasing player, close enough to shoot.");
            _currentState = State.Shoot;
        }
    }

    private void ShootPlayer()
    {
        if (_shootCooldownTimer <= 0f)
        {
            if (_currentGun != null && _bulletPrefab != null && _bulletSpawnPoint != null)
            {
                GameObject bullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, Quaternion.identity);
                Vector2 direction = (_player.transform.position - _bulletSpawnPoint.position).normalized;
                bullet.GetComponent<Rigidbody2D>().velocity = direction * 10f;
                Debug.Log("Shooting at player!");

                _shootCooldownTimer = _shootCooldown;
            }
            else
            {
                Debug.LogError("Gun or bullet prefab is not assigned!");
            }

            _currentState = State.Chase;
        }
        else
        {
            _shootCooldownTimer -= Time.deltaTime;
        }
    }

    private void AssignRandomGun()
    {
        if (_guns.Length > 0)
        {
            int randomIndex = Random.Range(0, _guns.Length);
            GameObject gunObject = Instantiate(_guns[randomIndex], _weaponInstantiatePoint.position, Quaternion.identity, transform);

            _currentGun = gunObject.GetComponent<Gun>();

            if (_currentGun != null)
            {
                Debug.Log("Gun instantiated: " + _currentGun.name);
            }
            else
            {
                Debug.LogError("Gun component missing on instantiated gun!");
            }
        }
        else
        {
            Debug.LogError("No guns available in the array!");
        }
    }


    protected override void Die()
    {
        Debug.Log("Enemy has been destroyed!");
        base.Die();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }
}
