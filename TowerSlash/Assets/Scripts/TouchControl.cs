using UnityEngine;

public class TouchControl : MonoBehaviour
{
    private Vector2 _startTouchPosition;
    private Vector2 _endTouchPosition;
    private Vector2 _swipeDelta;
    private const float _swipeThreshold = 50f;

    public delegate void OnSwipeDetected(SwipeDirection direction);
    public static event OnSwipeDetected SwipeEvent;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _startTouchPosition = touch.position;
                    break;

                case TouchPhase.Moved:
                    _swipeDelta = touch.position - _startTouchPosition;
                    break;

                case TouchPhase.Ended:
                    _endTouchPosition = touch.position;
                    DetectSwipe();
                    _swipeDelta = Vector2.zero;
                    break;
            }
        }
    }

    private void DetectSwipe()
    {
        if (_swipeDelta.magnitude > _swipeThreshold)
        {
            if (Mathf.Abs(_swipeDelta.x) > Mathf.Abs(_swipeDelta.y))
            {
                if (_swipeDelta.x > 0)
                    TriggerSwipe(SwipeDirection.Right);
                else
                    TriggerSwipe(SwipeDirection.Left);
            }
            else
            {
                if (_swipeDelta.y > 0)
                    TriggerSwipe(SwipeDirection.Up);
                else
                    TriggerSwipe(SwipeDirection.Down);
            }
        }
    }

    private void TriggerSwipe(SwipeDirection direction)
    {
        SwipeEvent?.Invoke(direction);
        Debug.Log($"Swipe {direction} detected!");
    }
}
