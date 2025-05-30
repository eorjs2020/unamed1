using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MonsterService : MonoBehaviour, IMonsterService
{
    protected IGameManager battleGameMgr;
    protected IGlobalEventService globalEvent;
    //protected IPlayerControllerService playerControllerService;

    protected Enemy CurrentBattleEnemy;

    public float CurrentDamage { get; private set; }

    

    public void Init(IGameManager gameManager)
    {
        battleGameMgr = (BattleGameManager)gameManager;
        globalEvent = battleGameMgr.GetMainGameManager().GetService<IGlobalEventService>();
        CurrentBattleEnemy = battleGameMgr.GetMainGameManager().GetService<IBattleTransferService>().CurrentEnemy;

        globalEvent.BattleStateUpdatedGlobal += GlobalEvent_BattleStateUpdatedGlobal;
    }

    private void OnDisable()
    {
        globalEvent.BattleStateUpdatedGlobal -= GlobalEvent_BattleStateUpdatedGlobal;
    }

    public Enemy GetCurrentBattleEnemy() { return CurrentBattleEnemy;}

    private void GlobalEvent_BattleStateUpdatedGlobal(IGameManager sender, BattleStateEventArgs args)
    {
        switch (args.battleStateType)
        {
            case BattleStateType.playerAttackResult:
                // todo dameges as a weapon
                CurrentDamage = battleGameMgr.GetService<IWeaponService>().GetDamageCalculated();
                //if (selectedWeapon == null) { Debug.LogError("selected Weapon is null!"); }
                CurrentBattleEnemy.Damaged(CurrentDamage); // damage animation or time we need
                battleGameMgr.SetBattleState(BattleStateType.enemyHitAnimation);
                // for updating UI info
                globalEvent.RaiseEnemyStatBattleUpdateGlobal(battleGameMgr, new EnemyStatBattleUpdateEventArgs(CurrentDamage, CurrentBattleEnemy.CurrentHp));


                //if(CurrentBattleEnemy.CurrentHp <= 0)
                //    battleGameMgr.SetBattleState(BattleStateType.enemyDeadAnimation);
                //else
                //    battleGameMgr.SetBattleState(BattleStateType.enemyAttackAnimation);
                break;

        }
    }
}
