using System.Collections;
using UnityEngine;

public enum SwipeDirection
{
    Up,
    Right,
    Down,
    Left
}

[System.Serializable]
public class BoxPair
{
    public GameObject boxPrefab;
    public SwipeDirection swipeDirection;
}

public class Enemy : MonoBehaviour
{
    [SerializeField] private BoxPair[] _boxPairs;
    [SerializeField] private float _boxOffset = 1.0f;
    [SerializeField] private float _triggerDistance = 3.0f;
    [SerializeField] private float _rotationSpeed = 90.0f;
    [SerializeField] private float _rotationConfusionDuration = 2.0f;

    private Transform _player;
    private GameObject _spawnedBox;
    private Vector3 _originalScale;
    private SwipeDirection _swipeDirection;
    private bool _isEnemyAlive = true;
    private bool _isLocked = false;

    private void OnEnable()
    {
        TouchControl.SwipeEvent += OnSwipeDetected;
    }

    private void OnDisable()
    {
        TouchControl.SwipeEvent -= OnSwipeDetected;
    }

    private void Start()
    {
        _player = GameManager.Instance.Player;
        SpawnRandomBox();
    }

    private void Update()
    {
        HandleBoxRotation();
        HandleBoxScaling();
    }

    private void SpawnRandomBox()
    {
        int randomIndex = GetRandomBoxIndex();
        Debug.Log($"Spawning box of type: {_boxPairs[randomIndex].swipeDirection} at index: {randomIndex}");

        Vector3 boxPosition = CalculateBoxPosition();
        _spawnedBox = Instantiate(_boxPairs[randomIndex].boxPrefab, boxPosition, Quaternion.identity);
        _spawnedBox.transform.parent = transform;

        _originalScale = _spawnedBox.transform.localScale;
        _swipeDirection = _boxPairs[randomIndex].swipeDirection;

        StartCoroutine(RotateBoxConfusion());
    }


    private int GetRandomBoxIndex()
    {
        return Random.Range(0, _boxPairs.Length);
    }

    private Vector3 CalculateBoxPosition()
    {
        return new Vector3(transform.position.x - _boxOffset, transform.position.y, transform.position.z);
    }

    private void HandleBoxRotation()
    {
        if (!_isLocked && _spawnedBox != null)
        {
            _spawnedBox.transform.Rotate(0, 0, _rotationSpeed * Time.deltaTime);
        }
    }

    private void HandleBoxScaling()
    {
        if (_spawnedBox != null && _player != null)
        {
            float distanceToPlayer = Vector3.Distance(_spawnedBox.transform.position, _player.position);
            Vector3 targetScale = (distanceToPlayer < _triggerDistance) ? _originalScale * 1.5f : _originalScale;
            _spawnedBox.transform.localScale = Vector3.Lerp(_spawnedBox.transform.localScale, targetScale, Time.deltaTime * 5f);

            if (distanceToPlayer < _triggerDistance && !_isLocked)
            {
                LockInDirection();
            }
        }
    }

    private IEnumerator RotateBoxConfusion()
    {
        float elapsedTime = 0f;

        while (elapsedTime < _rotationConfusionDuration)
        {
            if (!_isLocked)
            {
                _spawnedBox.transform.Rotate(0, 0, _rotationSpeed * Time.deltaTime);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (!_isLocked)
        {
            LockInDirection();
        }
    }

    private void LockInDirection()
    {
        _spawnedBox.transform.rotation = GetTargetRotation(_swipeDirection);
        _isLocked = true;
    }

    private Quaternion GetTargetRotation(SwipeDirection direction)
    {
        switch (direction)
        {
            case SwipeDirection.Up:
                return Quaternion.Euler(0, 0, 0);
            case SwipeDirection.Right:
                return Quaternion.Euler(0, 0, 90);
            case SwipeDirection.Down:
                return Quaternion.Euler(0, 0, 180);
            case SwipeDirection.Left:
                return Quaternion.Euler(0, 0, 270);
            default:
                return Quaternion.identity;
        }
    }

    private void OnSwipeDetected(SwipeDirection swipeDirection)
    {
        if (_isEnemyAlive && CanSlash() && swipeDirection == _swipeDirection)
        {
            KillEnemy();
        }
    }

    public bool CanSlash()
    {
        if (_spawnedBox != null)
        {
            float currentScale = _spawnedBox.transform.localScale.magnitude;
            float targetScale = (_originalScale * 1.5f).magnitude;
            float tolerance = 0.05f;
            return Mathf.Abs(currentScale - targetScale) < tolerance;
        }

        return false;
    }

    public void KillEnemy()
    {
        if (_isEnemyAlive)
        {
            GaugeBar gaugeBar = GameManager.Instance.GaugeBar;
            gaugeBar?.EnemyKilled();

            Destroy(gameObject);
            _isEnemyAlive = false;
        }
    }
}
