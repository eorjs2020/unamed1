using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponService : IGameService
{
    ScriptableWeapon SelectedWeapon { get; }
    float GetDamageCalculated();
    float GetDefaultAttackChanceIncrementValue();
    float GetDefaultAttackChanceDecrementValue();
}
