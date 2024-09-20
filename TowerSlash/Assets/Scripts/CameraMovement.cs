using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Vector3 _offset;

    private void Update()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        Vector3 newPosition = new Vector3(transform.position.x, _player.position.y + _offset.y, transform.position.z);
        transform.position = newPosition;
    }
}
