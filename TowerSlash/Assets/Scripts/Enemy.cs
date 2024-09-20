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
    [SerializeField] private Transform _player;
    [SerializeField] private float _triggerDistance = 3.0f;

    private GameObject _spawnedBox;
    private Vector3 _originalScale;
    private SwipeDirection _swipeDirection;
    private bool _isEnemyAlive = true;

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
        SpawnRandomBox();
    }

    private void Update()
    {
        HandleBoxScaling();
    }

    private void SpawnRandomBox()
    {
        int randomIndex = GetRandomBoxIndex();
        Vector3 boxPosition = CalculateBoxPosition();
        _spawnedBox = Instantiate(_boxPairs[randomIndex].boxPrefab, boxPosition, Quaternion.identity);
        _spawnedBox.transform.parent = transform;

        _originalScale = _spawnedBox.transform.localScale;
        _swipeDirection = _boxPairs[randomIndex].swipeDirection; 
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

    private void HandleBoxScaling()
    {
        if (_spawnedBox != null)
        {
            float distanceToPlayer = Vector3.Distance(_spawnedBox.transform.position, _player.position);
            Vector3 targetScale = (distanceToPlayer < _triggerDistance) ?
                _originalScale * 1.5f :
                _originalScale;

            _spawnedBox.transform.localScale = Vector3.Lerp(_spawnedBox.transform.localScale, targetScale, Time.deltaTime * 5f);
        }
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
            float targetScale = (_originalScale * 1.5f).magnitude;
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
            Destroy(gameObject);
            _isEnemyAlive = false;
        }
    }
}
