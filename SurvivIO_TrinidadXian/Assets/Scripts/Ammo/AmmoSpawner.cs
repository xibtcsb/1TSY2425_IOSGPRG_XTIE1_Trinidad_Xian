using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private GameObject[] _spawnAreas;
    [SerializeField] private AmmoPickup[] _ammoPickupPrefabs; 
    [SerializeField] private int _maxAmmoPickups = 50; 
    [SerializeField] private float _respawnTime = 20f;

    private List<GameObject> _activeAmmoPickups = new List<GameObject>();
    private Queue<GameObject> _pickupQueue = new Queue<GameObject>();

    private void Start()
    {
        for (int i = 0; i < _maxAmmoPickups; i++)
        {
            SpawnSingleAmmoPickup();
        }
    }

    private void SpawnSingleAmmoPickup()
    {
        GameObject randomArea = _spawnAreas[Random.Range(0, _spawnAreas.Length)];
        AmmoPickup randomAmmoType = _ammoPickupPrefabs[Random.Range(0, _ammoPickupPrefabs.Length)];

        Collider2D areaCollider = randomArea.GetComponent<Collider2D>();
        Vector2 spawnPosition = RandomPointInBox(areaCollider.bounds);

        GameObject ammoPickup = Instantiate(randomAmmoType.gameObject, spawnPosition, Quaternion.identity);
        _activeAmmoPickups.Add(ammoPickup);

        AmmoPickup ammoScript = ammoPickup.GetComponent<AmmoPickup>();
        ammoScript.OnDestroyCallback = (pickup) =>
        {
            if (this != null) 
            {
                StartCoroutine(RespawnAmmo(pickup));
            }
        };
    }

    private IEnumerator RespawnAmmo(GameObject pickup)
    {
        _activeAmmoPickups.Remove(pickup);
        yield return new WaitForSeconds(_respawnTime);

        if (this != null)
        {
            SpawnSingleAmmoPickup();
        }
    }

    private Vector2 RandomPointInBox(Bounds bounds)
    {
        return new Vector2(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y)
        );
    }
}
