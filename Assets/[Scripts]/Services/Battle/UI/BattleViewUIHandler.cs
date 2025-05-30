using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.Networking.PlayerConnection;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleViewUIHandler : MonoBehaviour, IBattleViewUIHandler
{
    protected IGameManager battleGameMgr;
    protected IGlobalEventService globalEvent;
    //protected IPlayerControllerService playerControllerService;

    public GameObject MonsterUI;
    [SerializeField]
    private GameObject AttackBarBackPannel;
    [SerializeField]
    private GameObject AttackButton;
    [SerializeField]
    private TextMeshProUGUI CharacterHPText;
    [SerializeField]
    private TextMeshProUGUI EnemyHPText;

    public void Init(IGameManager gameManager)
    {
        battleGameMgr = (BattleGameManager)gameManager;
        globalEvent = battleGameMgr.GetMainGameManager().GetService<IGlobalEventService>();

        globalEvent.BattleStateUpdatedGlobal += GlobalEvent_BattleStateUpdatedGlobal;
        globalEvent.EnemyStatBattleUpdateGlobal += GlobalEvent_EnemyStatBattleUpdateGlobal;
        globalEvent.PlayerStatBattleUpdateGlobal += GlobalEvent_PlayerStatBattleUpdateGlobal;
    }

    private void GlobalEvent_PlayerStatBattleUpdateGlobal(IGameManager sender, PlayerStatBattleUpdateEventArgs args)
    {
        CharacterHPText.text = args.HP.ToString("F2");
    }

    private void GlobalEvent_EnemyStatBattleUpdateGlobal(IGameManager sender, EnemyStatBattleUpdateEventArgs args)
    {
        EnemyHPText.text = args.HP.ToString("F2");
        //battleGameMgr.GetService<IBattleVFXService>().MissEffect();
    }

    private void OnDisable()
    {
        globalEvent.BattleStateUpdatedGlobal -= GlobalEvent_BattleStateUpdatedGlobal;
        globalEvent.EnemyStatBattleUpdateGlobal -= GlobalEvent_EnemyStatBattleUpdateGlobal;
        globalEvent.PlayerStatBattleUpdateGlobal -= GlobalEvent_PlayerStatBattleUpdateGlobal;
    }

    private void GlobalEvent_BattleStateUpdatedGlobal(IGameManager sender, BattleStateEventArgs args)
    {
        float duration = 0;
        switch (args.battleStateType)
        {
            case BattleStateType.init_done:
                InitBattleView();
                break;
            case BattleStateType.ready:
                AttackBarBackPannel.SetActive(false);
                AttackButton.SetActive(true);
                break;
            case BattleStateType.bar:
                AttackBarBackPannel.SetActive(true);
                break;
            case BattleStateType.loading:
                break;
            case BattleStateType.select_weapon:
                AttackButton.SetActive(false);
                break;
            case BattleStateType.playerAttackEffectAnimation:
                // get damage 
                battleGameMgr.GetService<IBattleVFXService>().PlayAttackEffect();
                AttackBarBackPannel.SetActive(false);
                break;
            case BattleStateType.enemyHitAnimation:
                var currentDamage = battleGameMgr.GetService<IMonsterService>().CurrentDamage;
                if (currentDamage > 0)
                {
                    var animator = MonsterUI.GetComponent<Animator>();
                    animator.SetTrigger("Hit");
                    var numberEffectDuration = battleGameMgr.GetService<IBattleVFXService>().EnemyDamageEffect(currentDamage);
                    var currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);
                    var animationDuration = currentStateInfo.length;
                    duration = Mathf.Max(numberEffectDuration, animationDuration);

                    //MonsterUI.GetComponent<Animator>().SetTrigger("Hit");
                    //var numberEffectDuration = battleGameMgr.GetService<IBattleVFXService>().EnemyDamageEffect(currentDamage);
                    //var aniamtionDuration = MonsterUI.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
                    //duration = numberEffectDuration > aniamtionDuration ? numberEffectDuration : aniamtionDuration;
                }
                else
                {
                    duration = battleGameMgr.GetService<IBattleVFXService>().MissEffect();
                }

                if (battleGameMgr.GetService<IMonsterService>().GetCurrentBattleEnemy().CurrentHp <= 0)
                    battleGameMgr.SetBattleState(BattleStateType.enemyDeadAnimation, duration);
                else
                    battleGameMgr.SetBattleState(BattleStateType.enemyAttackAnimation, duration);
                
                //battleGameMgr.SetBattleState(BattleStateType.playerAttackResult, duration);
                break;
            case BattleStateType.enemyDeadAnimation:
                MonsterUI.GetComponent<Animator>().SetTrigger("Dead");
                duration = MonsterUI.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
                battleGameMgr.SetBattleState(BattleStateType.end, duration, BattleResult.Win);
                //battleGameMgr.SetBattleState(BattleStateType.playerAttackResult, duration);
                break;

            case BattleStateType.enemyAttackAnimation:
                // needs getting damage
                MonsterUI.GetComponent<Animator>().SetTrigger("Attack");
                duration = MonsterUI.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
                battleGameMgr.SetBattleState(BattleStateType.enemyAttackEffectAnimation, duration);
                break;
            case BattleStateType.enemyAttackEffectAnimation:
                battleGameMgr.GetService<IBattleVFXService>().EnemyAttackEffect();
                break;
        }

    }

    private void InitBattleView()
    {
        var ScriptableEnemy = battleGameMgr.GetService<IMonsterService>().GetCurrentBattleEnemy().EnemyInfo.ScriptableEnemy;
        MonsterUI.GetComponent<Image>().sprite = ScriptableEnemy.EnemySprite;
        MonsterUI.GetComponent<Animator>().runtimeAnimatorController = ScriptableEnemy.Animator;
        EnemyHPText.text = battleGameMgr.GetService<IMonsterService>().GetCurrentBattleEnemy().CurrentHp.ToString();
        CharacterHPText.text =  battleGameMgr.GetMainGameManager().GetService<IPlayerControllerService>().PlayerCharacter.CurrentHp.ToString();
        Debug.Log("Init done");
        battleGameMgr.SetBattleState(BattleStateType.ready);
        //globalEvent.RaiseBattleStateUpdatedGlobal(battleGameMgr, new BattleStateEventArgs(BattleStateType.ready));
    }

    public void OnAttackButton()
    {
        battleGameMgr.SetBattleState(BattleStateType.select_weapon);
        //globalEvent.RaiseBattleStateUpdatedGlobal(battleGameMgr, new BattleStateEventArgs(BattleStateType.select_weapon));
    }

    public void OnExitButton()
    {
        battleGameMgr.SetBattleState(BattleStateType.end);
        //battleGameMgr.GetMainGameManager().SetState(GameStateType.playing);
        //globalEvent.RaiseBattleResultGlobal(battleGameMgr, new BattleResultEventArgs(BattleResult.Win));
        //SceneManager.UnloadSceneAsync(0);
        //AudioEXManager.Instance.PlayBgm(Sound.INGAME_BGM);
    }
}
