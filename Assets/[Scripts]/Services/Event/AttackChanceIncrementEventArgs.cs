using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackChanceIncrementEventArgs : EventArgs
{
    public float AttackChanceIncrementValue;

    public AttackChanceIncrementEventArgs(float attackChanceIncrementValue)
    {
        AttackChanceIncrementValue = attackChanceIncrementValue;
    }

}
