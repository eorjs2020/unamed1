using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonsterService : IGameService
{
    public Enemy GetCurrentBattleEnemy();
    public float CurrentDamage { get; }
}
