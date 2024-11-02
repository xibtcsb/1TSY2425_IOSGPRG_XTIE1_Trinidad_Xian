using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public enum AmmoType
    {
        Pistol,
        Shotgun,
        AssaultRifle
    }

    [SerializeField] private AmmoType _ammoType;

    private int _minAmmo;
    private int _maxAmmo;

    private void Start()
    {
        switch (_ammoType)
        {
            case AmmoType.Pistol:
                _minAmmo = 1;
                _maxAmmo = 8;
                break;
            case AmmoType.Shotgun:
                _minAmmo = 1;
                _maxAmmo = 2;
                break;
            case AmmoType.AssaultRifle:
                _minAmmo = 5;
                _maxAmmo = 15;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                int ammoAmount = Random.Range(_minAmmo, _maxAmmo + 1);
                player.AddAmmo(_ammoType, ammoAmount);
                Destroy(gameObject);
            }
        }
    }
}
