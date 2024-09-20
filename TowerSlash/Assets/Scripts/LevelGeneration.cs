using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    [SerializeField] private GameObject _floorPrefab;
    [SerializeField] private Transform _player;
    [SerializeField] private float _floorHeight = 5f;
    [SerializeField] private int _initialFloorCount = 5;

    private Queue<GameObject> _floors;
    private float _spawnPositionY = 0f;

    private void Start()
    {
        _floors = new Queue<GameObject>();
        SpawnInitialFloors();
    }

    private void Update()
    {
        if (ShouldSpawnNewFloor())
        {
            SpawnFloor();
            RemoveOldFloor();
        }
    }

    private void SpawnInitialFloors()
    {
        for (int i = 0; i < _initialFloorCount; i++)
        {
            SpawnFloor();
        }
    }

    private bool ShouldSpawnNewFloor()
    {
        return _player.position.y > _spawnPositionY - (_initialFloorCount * _floorHeight / 2);
    }

    private void SpawnFloor()
    {
        GameObject newFloor = Instantiate(_floorPrefab, new Vector3(1.85f, _spawnPositionY, 0), Quaternion.identity);
        _floors.Enqueue(newFloor);
        _spawnPositionY += _floorHeight;
    }

    private void RemoveOldFloor()
    {
        if (_floors.Count > 0)
        {
            GameObject oldFloor = _floors.Dequeue();
            Destroy(oldFloor);
        }
    }
}
