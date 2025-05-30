using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;


public enum ItemLevel
{
    [Description("Lvel_10")]
    _10,
    [Description("Lvel_20")]
    _20,
    [Description("Lvel_30")]
    _30,
    [Description("Lvel_40")]
    _40,
    [Description("Lvel_50")]
    _50,
    [Description("Lvel_60")]
    _60,
    [Description("Lvel_70")]
    _70,
    [Description("Lvel_80")]
    _80,
}

[CreateAssetMenu(fileName = "ScriptableItem", menuName = "Bandibul/ScriptableItem")]

public class ScriptableItem : ScriptableObject
{
    public Defines.InvenItemType InvenItemType = Defines.InvenItemType.None;
    public int ItemId = Defines.ScriptableItemId++;
    public string ItemName;
    public string Description;
    public Sprite Sprite;
    public ItemLevel ItemLevel;
    
}
