using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleTransferService : MonoBehaviour, IBattleTransferService
{
    protected IGameManager gameMgr;
    protected IGlobalEventService globalEvent;

    public Enemy CurrentEnemy { get; set; }

    public void Init(IGameManager gameManager)
    {
        gameMgr = gameManager;
        globalEvent = gameMgr.GetService<IGlobalEventService>();


        globalEvent.BattleEventInfoGlobal += GlobalEvent_BattleEventInfo;
        globalEvent.BattleResultGlobal += GlobalEvent_BattleResultGlobal;
        AudioEXManager.Instance.PlayBgm(Sound.INGAME_BGM);
    }

    
    private void OnDisable()
    {
        globalEvent.BattleEventInfoGlobal -= GlobalEvent_BattleEventInfo;
        globalEvent.BattleResultGlobal -= GlobalEvent_BattleResultGlobal;
    }

    private void GlobalEvent_BattleEventInfo(IGameManager sender, BattleEventArgs args)
    {
        Debug.Log("Start Battle with " + args.Enemy.EnemyInfo.ScriptableEnemy.name + "!!!");
        CurrentEnemy = args.Enemy;
        gameMgr.GetService<IVFXService>().StartBattleEffect(StartBattleScene);
    }

    private void StartBattleScene()
    {
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Additive);
    }
    private void GlobalEvent_BattleResultGlobal(IGameManager sender, BattleResultEventArgs args)
    {
        //switch(args.result)
        //{
        //    case BattleResult.Win:
        //        globalEvent.RaiseMapCollisionGlobal(gameMgr, new MapEventArgs(MapArgType.ENEMY,Vector2.zero));
        //        //Destroy(CurrentEnemy.gameObject);
        //        break;
        //    case BattleResult.Lose:
        //        break;
        //    case BattleResult.Flee:
        //        break;
        //}
    }

}
