using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ScriptableItem", menuName = "Bandibul/ScriptableWeapon")]
public class ScriptableWeapon : ScriptableItem
{
    public int MinPower;
    public int MaxPower;
    public AnimatorOverrideController WeaponAttackAnimation;

    public ScriptableWeapon()
    {
        ItemId = Defines.ScriptableWeaponId++;
    }
}
