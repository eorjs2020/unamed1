using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTimingbarService :MonoBehaviour, IAttackTimingbarService
{
    protected IGameManager battleGameMgr;
    protected IGlobalEventService globalEvent;
    //protected IPlayerControllerService playerControllerService;

    public void Init(IGameManager gameManager)
    {
        battleGameMgr = (BattleGameManager)gameManager;
        globalEvent = battleGameMgr.GetMainGameManager().GetService<IGlobalEventService>();
    }
}
