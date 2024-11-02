using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{
    private void Awake()
    {
        _clipCapacity = 15;
        _maxCarryAmmo = 90;
        InitializeAmmo();
    }

    public new void Shoot()
    {
        base.Shoot();
    }
}