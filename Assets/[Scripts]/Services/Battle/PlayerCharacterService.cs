using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterService : MonoBehaviour, IPlayerCharacterService
{
    protected IGameManager battleGameMgr;
    protected IGlobalEventService globalEvent;

    private PlayerCharacter playerCharacter;

    public void Init(IGameManager gameManager)
    {
        battleGameMgr = (BattleGameManager)gameManager;
        globalEvent = battleGameMgr.GetMainGameManager().GetService<IGlobalEventService>();
        playerCharacter = battleGameMgr.GetMainGameManager().GetService<IPlayerControllerService>().PlayerCharacter;

        globalEvent.BattleStateUpdatedGlobal += GlobalEvent_BattleStateUpdatedGlobal;
    }

    private void OnDisable()
    {
        globalEvent.BattleStateUpdatedGlobal -= GlobalEvent_BattleStateUpdatedGlobal;
    }

    private void GlobalEvent_BattleStateUpdatedGlobal(IGameManager sender, BattleStateEventArgs args)
    {
        switch (args.battleStateType)
        {
            case BattleStateType.enemyAttackResult:
                playerCharacter.Damaged(10);
                globalEvent.RaisePlayerStatBattleUpdateGlobal(battleGameMgr, new PlayerStatBattleUpdateEventArgs(playerCharacter.CurrentHp));
                

                if (playerCharacter.CurrentHp <= 0)
                    battleGameMgr.SetBattleState(BattleStateType.end, result: BattleResult.Lose);
                else
                    battleGameMgr.SetBattleState(BattleStateType.ready);
                break;

        }
    }
}
