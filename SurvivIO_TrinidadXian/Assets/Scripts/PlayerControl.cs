using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb2d;
    [SerializeField] private FixedJoystick _joyStickMovement;
    [SerializeField] private FixedJoystick _joyStickLooking;
    [SerializeField] private float _moveSpeed = 5f;

    private void FixedUpdate()
    {
        Movement();
        LookAround();
    }

    public void Movement()
    {
        Vector2 movement = new Vector2(_joyStickMovement.Horizontal * _moveSpeed, _joyStickMovement.Vertical * _moveSpeed);
        _rb2d.velocity = movement;
    }

    public void LookAround()
    {
        Vector2 direction = new Vector2(_joyStickLooking.Horizontal, _joyStickLooking.Vertical);

        if (direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            _rb2d.rotation = angle;
        }
    }
}
