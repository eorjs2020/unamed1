using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatBattleUpdateEventArgs : EventArgs
{
    public float Damage;
    public float HP;

    public EnemyStatBattleUpdateEventArgs(float damage, float hP)
    {
        Damage = damage;
        HP = hP;
    }
}
