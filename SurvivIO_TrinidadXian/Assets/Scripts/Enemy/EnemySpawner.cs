using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _spawnAreas;
    [SerializeField] private float _timeBetweenSpawns;
    [SerializeField] private EnemyAI _enemyPrefab;

    private float _lastSpawnTime;
    private GameObject _areaToUse;
    private Collider2D _areaCollider;
    private Bounds _bounds;

    void Update()
    {
        if (Time.time > _lastSpawnTime + _timeBetweenSpawns)
        {
            _lastSpawnTime = Time.time;
            GetRandomCollider();
            Instantiate(_enemyPrefab, RandomPointInBox(_bounds), Quaternion.identity);
        }
    }

    private void GetRandomCollider()
    {
        _areaToUse = _spawnAreas[Random.Range(0, _spawnAreas.Length)];
        _areaCollider = _areaToUse.GetComponent<Collider2D>();
        _bounds = _areaCollider.bounds;
    }

    private Vector2 RandomPointInBox(Bounds bounds)
    {
        return new Vector2(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y));
    }
}
