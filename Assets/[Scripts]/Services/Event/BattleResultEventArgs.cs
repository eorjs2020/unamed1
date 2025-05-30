using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleResult
{
    Win,
    Lose,
    Flee,
}

public class BattleResultEventArgs : EventArgs
{
    public BattleResult result;

    public BattleResultEventArgs(BattleResult result)
    {
        this.result = result;
    }
}
