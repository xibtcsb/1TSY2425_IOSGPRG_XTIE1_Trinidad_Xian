using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    private void Awake()
    {
        _clipCapacity = 2;
        _maxCarryAmmo = 60;
        InitializeAmmo();
    }

    public new void Shoot()
    {
        base.Shoot();
    }

    public new IEnumerator Reload()
    {
        yield return base.Reload();
    }
}
