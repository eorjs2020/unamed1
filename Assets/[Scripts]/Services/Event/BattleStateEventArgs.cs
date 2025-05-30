using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStateEventArgs : EventArgs
{
    public BattleStateType battleStateType;

    public BattleStateEventArgs(BattleStateType battleStateType)
    {
        this.battleStateType = battleStateType;
    }
}
