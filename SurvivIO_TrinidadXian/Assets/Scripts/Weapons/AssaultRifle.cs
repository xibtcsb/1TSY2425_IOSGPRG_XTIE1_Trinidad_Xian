using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : Gun
{
    private void Awake()
    {
        _clipCapacity = 30;
        _maxCarryAmmo = 120;
        InitializeAmmo();
    }

    public new void Shoot()
    {
        base.Shoot();
    }
}