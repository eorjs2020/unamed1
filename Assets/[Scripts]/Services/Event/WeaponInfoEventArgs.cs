using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInfoEventArgs : EventArgs
{
    public ScriptableWeapon scriptableWeapon;

    public WeaponInfoEventArgs(ScriptableWeapon scriptableWeapon)
    {
        this.scriptableWeapon = scriptableWeapon;
    }
}
