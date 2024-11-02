using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponHolder : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Gun _pistol;
    [SerializeField] private Gun _shotgun;
    [SerializeField] private Gun _assaultRifle;

    public Button _primarySwap;
    public Button _secondarySwap;

    private int _weaponIndex = 0;

    private void Start()
    {
        _primarySwap.onClick.AddListener(PrimarySwap);
        _secondarySwap.onClick.AddListener(SecondarySwap);

        EquipGun(_pistol);
    }

    private void EquipGun(Gun newGun)
    {
        _player.SetCurrentGun(newGun);
        UpdateWeaponVisibility();
    }

    private void UpdateWeaponVisibility()
    {
        _pistol.gameObject.SetActive(_weaponIndex == 0);
        _shotgun.gameObject.SetActive(_weaponIndex == 1);
        _assaultRifle.gameObject.SetActive(_weaponIndex == 2);
    }

    public void PrimarySwap()
    {
        if (_weaponIndex == 1)
        {
            _weaponIndex = 2;
            EquipGun(_assaultRifle);
        }
        else
        {
            _weaponIndex = 1;
            EquipGun(_shotgun);
        }
    }

    public void SecondarySwap()
    {
        _weaponIndex = 0;
        EquipGun(_pistol);
    }
}
