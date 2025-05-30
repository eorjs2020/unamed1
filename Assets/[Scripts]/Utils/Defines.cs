using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defines
{
    static public int ScriptableItemId = 0;
    static public int ScriptableCraftingRecipeId = 0;
    static public int ScriptableEnemyId = 0;
    static public int ScriptableWeaponId = 0;

    public enum StatusType
    {
        HP,
        Hunger
    }

    public enum InvenItemType
    {
        None,
        Misc,
        Box,
        Consumable,
        Weapon,
        Material,
    }

    public enum RecipeType
    {
        None,
        Consumable,
        Weapon,
        Misc,
    }
}
