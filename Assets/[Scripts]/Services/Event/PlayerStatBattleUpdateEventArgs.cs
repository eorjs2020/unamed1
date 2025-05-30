using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatBattleUpdateEventArgs : EventArgs
{
    public float HP;

    public PlayerStatBattleUpdateEventArgs(float hP)
    {
        HP = hP;
    }
}
