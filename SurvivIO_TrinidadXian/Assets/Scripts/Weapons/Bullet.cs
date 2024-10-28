using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _bulletSpan = 5.5f;

    private void Start()
    {
        Destroy(gameObject, _bulletSpan);
    }

    private void Update()
    {
        BulletShoot();
    }

    private void BulletShoot()
    {
        transform.position += transform.up * Time.deltaTime * _speed;
    }
}
