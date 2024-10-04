using UnityEngine;

public class TouchControl : MonoBehaviour
{
    [SerializeField] private float swipeThreshold = 50f;
    private Vector2 _startTouchPosition;
    private Vector2 _swipeDelta;
    private bool _canSwipe = true;
    private const float swipeCooldown = 0.2f;

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
            Debug.Log($"Mouse Down at: {_startTouchPosition}");
        }
        else if (Input.GetMouseButton(0))
        {
            _swipeDelta = (Vector2)Input.mousePosition - _startTouchPosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _swipeDelta = (Vector2)Input.mousePosition - _startTouchPosition;
            Debug.Log($"Mouse Up at: {Input.mousePosition}, Swipe Delta: {_swipeDelta}");
            DetectSwipe();
        }
    }

    private void ProcessTouch(Touch touch)
    {
        switch (touch.phase)
        {
            case TouchPhase.Began:
                _startTouchPosition = touch.position;
                Debug.Log($"Touch Began at: {_startTouchPosition}");
                break;

            case TouchPhase.Moved:
                _swipeDelta = touch.position - _startTouchPosition;
                break;

            case TouchPhase.Ended:
                _swipeDelta = touch.position - _startTouchPosition;
                Debug.Log($"Touch Ended at: {touch.position}, Swipe Delta: {_swipeDelta}");
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
            _canSwipe = false;
            Invoke(nameof(ResetSwipe), swipeCooldown);
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
        _canSwipe = true;
    }
}
