using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleVFXService : MonoBehaviour, IBattleVFXService
{

    public GameObject attackEffectPrefab;  // 공격 효과 프리팹
    public Transform effectSpawnPoint;     // 효과가 생성될 위치
    public Transform PlayerHitPosition;




    protected IGameManager battleGameMgr;
    protected IGlobalEventService globalEvent;
    //protected IPlayerControllerService playerControllerService;

    public void Init(IGameManager gameManager)
    {
        battleGameMgr = (BattleGameManager)gameManager;
        globalEvent = battleGameMgr.GetMainGameManager().GetService<IGlobalEventService>();

    }


    public void PlayAttackEffect()
    {
        //todo
        // testing.. change attack effect as a weapon later
        // changing attack sound as a weapon
        AudioEXManager.Instance.PlayFX(Sound.FIRE_FX);
        attackEffectPrefab = Resources.Load<GameObject>(ResourcePath.AttackEffect);
        GameObject effect = Instantiate(attackEffectPrefab, effectSpawnPoint);
        effect.GetComponent<Animator>().runtimeAnimatorController = battleGameMgr.GetService<IWeaponService>().SelectedWeapon.WeaponAttackAnimation;
        //effect.transform.parent = effectSpawnPoint;

        // set destroy duration by using animation's duration, and change the game state
        float effectDuration = effect.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        Destroy(effect, effectDuration);
        battleGameMgr.SetBattleState(BattleStateType.playerAttackResult, effectDuration);
        //StartCoroutine(ChangeGameState(effectDuration,BattleStateType.playerAttackResult));
    }

    public void EnemyAttackEffect()
    {
        attackEffectPrefab = Resources.Load<GameObject>(ResourcePath.EnemyAttackEffect1);
        GameObject effect = Instantiate(attackEffectPrefab, PlayerHitPosition);
        float effectDuration = effect.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        Destroy(effect, effectDuration);
        battleGameMgr.SetBattleState(BattleStateType.enemyAttackResult, effectDuration);
        //StartCoroutine(ChangeGameState(effectDuration, BattleStateType.enemyAttackResult));
    }

    public float MissEffect()
    {
        var effect = Resources.Load<DamageNumber>(ResourcePath.MissDamageEffect);
        effect.SpawnGUI(effectSpawnPoint.GetComponent<RectTransform>(), Vector2.zero);
        return effect.lifetime;
    }

    public float EnemyDamageEffect(float damage)
    {
        var effect = Resources.Load<DamageNumber>(ResourcePath.DamageNumberEffect);
        effect.SpawnGUI(effectSpawnPoint.GetComponent<RectTransform>(), Vector2.zero, damage);
        return effect.lifetime;
    }

    public float PercentagePopEffect(GameObject parent, float percentage, bool isMinus = false)
    {
        DamageNumber effect;
        if (percentage >= 0)
        {
            effect = Resources.Load<DamageNumber>(ResourcePath.PercentPopEffect);
            effect.SpawnGUI(parent.GetComponent<RectTransform>(), Vector2.zero, percentage);
        }
        else
        {
            effect = Resources.Load<DamageNumber>(ResourcePath.MinusPercentPopEffect);
            effect.SpawnGUI(parent.GetComponent<RectTransform>(), Vector2.zero, percentage);
        }
        return effect.lifetime;
    }

}
