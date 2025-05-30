using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStatsInfoEventArgs : EventArgs
{
    public WeaponAttackStats weaponAttackStats;

    public WeaponStatsInfoEventArgs(WeaponAttackStats weaponAttackStats)
    {
        this.weaponAttackStats = weaponAttackStats;
    }
}
