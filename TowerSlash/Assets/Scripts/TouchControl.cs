using UnityEngine;


public class TouchControl : MonoBehaviour
{
    [SerializeField] private float swipeThreshold = 50f; // Configurable in the inspector
    private Vector2 _startTouchPosition;
    private Vector2 _swipeDelta;
    private bool _canSwipe = true; // Prevent multiple detections from a single swipe
    private const float swipeCooldown = 0.2f; // Cooldown period in seconds

    public delegate void OnSwipeDetected(SwipeDirection direction);
    public static event OnSwipeDetected SwipeEvent;

    private void Update()
    {
#if UNITY_EDITOR 
        HandleMouseInput();
#else
        HandleTouchInput();
#endif
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            ProcessTouch(touch);
        }
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _startTouchPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            _swipeDelta = (Vector2)Input.mousePosition - _startTouchPosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _swipeDelta = (Vector2)Input.mousePosition - _startTouchPosition;
            DetectSwipe();
        }
    }

    private void ProcessTouch(Touch touch)
    {
        switch (touch.phase)
        {
            case TouchPhase.Began:
                _startTouchPosition = touch.position;
                break;

            case TouchPhase.Moved:
                _swipeDelta = touch.position - _startTouchPosition;
                break;

            case TouchPhase.Ended:
                _swipeDelta = touch.position - _startTouchPosition;
                DetectSwipe();
                break;
        }
    }

    private void DetectSwipe()
    {
        if (_canSwipe && _swipeDelta.magnitude > swipeThreshold)
        {
            SwipeDirection direction = GetSwipeDirection();
            TriggerSwipe(direction);
            _canSwipe = false; // Start cooldown
            Invoke(nameof(ResetSwipe), swipeCooldown); // Reset cooldown
        }
    }

    private SwipeDirection GetSwipeDirection()
    {
        if (Mathf.Abs(_swipeDelta.x) > Mathf.Abs(_swipeDelta.y))
        {
            return _swipeDelta.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
        }
        else
        {
            return _swipeDelta.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
        }
    }

    private void TriggerSwipe(SwipeDirection direction)
    {
        SwipeEvent?.Invoke(direction);
        Debug.Log($"Swipe {direction} detected!");
    }

    private void ResetSwipe()
    {
        _canSwipe = true; // Allow swipes again
    }
}
