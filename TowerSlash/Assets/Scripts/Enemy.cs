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
    [SerializeField] private float _triggerDistance = 5.0f;
    [SerializeField] private float _scaleTolerance = 0.01f; 

    private Transform _player;
    private GameObject _spawnedBox;
    private Vector3 _originalScale;
    private SwipeDirection _swipeDirection;
    private bool _isEnemyAlive = true;
    private bool _isLockedInPlace = false; 
    private Quaternion _initialRotation;
    private Vector3 _targetScale;

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
        HandleBoxScalingAndSpinning();
    }

    private void SpawnRandomBox()
    {
        int randomIndex = GetRandomBoxIndex();
        Vector3 boxPosition = CalculateBoxPosition();
        _spawnedBox = Instantiate(_boxPairs[randomIndex].boxPrefab, boxPosition, Quaternion.identity);
        _spawnedBox.transform.parent = transform;

        _originalScale = _spawnedBox.transform.localScale;
        _targetScale = _originalScale * 1.5f;  
        _swipeDirection = _boxPairs[randomIndex].swipeDirection;

        _initialRotation = _spawnedBox.transform.rotation;

        Debug.Log($"Spawned box with swipe direction: {_swipeDirection}");
    }

    private int GetRandomBoxIndex()
    {
        return Random.Range(0, _boxPairs.Length);
    }

    private Vector3 CalculateBoxPosition()
    {
        return new Vector3(transform.position.x - _boxOffset, transform.position.y, transform.position.z);
    }

    private void HandleBoxScalingAndSpinning()
    {
        if (_spawnedBox != null && _player != null)
        {
            float distanceToPlayer = Vector3.Distance(_spawnedBox.transform.position, _player.position);
            Vector3 targetScale = (distanceToPlayer < _triggerDistance) ? _targetScale : _originalScale;
            _spawnedBox.transform.localScale = Vector3.Lerp(_spawnedBox.transform.localScale, targetScale, Time.deltaTime * 5f);

            if (!_isLockedInPlace && Mathf.Abs(_spawnedBox.transform.localScale.magnitude - _targetScale.magnitude) < _scaleTolerance)
            {
                LockBoxRotation();
            }

            if (!_isLockedInPlace)
            {
                _spawnedBox.transform.Rotate(0f, 0f, 300f * Time.deltaTime);
            }
        }
    }

    private void LockBoxRotation()
    {
        _isLockedInPlace = true; 
        _spawnedBox.transform.rotation = _initialRotation;
        Debug.Log("Box locked in initial rotation!");
    }

    private void OnSwipeDetected(SwipeDirection swipeDirection)
    {
        Debug.Log($"Swipe detected: {swipeDirection}. Expected: {_swipeDirection}");

        if (_isEnemyAlive && CanSlash() && swipeDirection == _swipeDirection)
        {
            KillEnemy();
        }
        else
        {
            Debug.Log($"Wrong Swipe: {_swipeDirection} was the correct direction, you swiped... {swipeDirection}");
        }
    }

    public bool CanSlash()
    {
        if (_spawnedBox != null)
        {
            float currentScale = _spawnedBox.transform.localScale.magnitude;
            float targetScale = _targetScale.magnitude;
            float tolerance = 0.05f;
            bool isCloseEnough = Mathf.Abs(currentScale - targetScale) < tolerance;

            return isCloseEnough;
        }

        return false;
    }

    public void KillEnemy()
    {
        if (_isEnemyAlive)
        {
            Debug.Log("Enemy killed!");

            GaugeBar gaugeBar = FindObjectOfType<GaugeBar>();
            if (gaugeBar != null)
            {
                gaugeBar.EnemyKilled(true);
            }

            GameManager.Instance.AddScore(250);

            Destroy(gameObject);
            _isEnemyAlive = false;
        }
    }

}
