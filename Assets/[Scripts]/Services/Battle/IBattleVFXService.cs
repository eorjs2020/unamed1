using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattleVFXService : IGameService
{
    public void PlayAttackEffect();
    public void EnemyAttackEffect();
    public float MissEffect();
    public float EnemyDamageEffect(float damage);
    public float PercentagePopEffect(GameObject parent, float percentage, bool isMinus = false);
}
