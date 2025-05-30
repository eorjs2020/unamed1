using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.Animations;
using UnityEngine;


public enum EnemyLevel
{
    [Description("Lvel_1")]
    _1,
    [Description("Lvel_2")]
    _2,
    [Description("Lvel_3")]
    _3,
    [Description("Lvel_4")]
    _4,
    [Description("Lvel_5")]
    _5,
    
}


[CreateAssetMenu(fileName = "ScriptableItem", menuName = "Bandibul/ScriptableEnemy")]
public class ScriptableEnemy : ScriptableObject
{
    public int EnemyId = Defines.ScriptableEnemyId++;
    public string EnemyName;
    public string Description;
    public EnemyLevel Level;
    public Sprite EnemySprite;
    public AnimatorOverrideController Animator;
}
