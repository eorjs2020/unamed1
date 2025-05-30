using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;

public class BattleGameManager : GameManager
{
    public IGameManager MainGameManger;
    protected IGlobalEventService globalEvent;
    public override BattleStateType BattleState { get; protected set; }

    private void Awake()
    {
        // todo changing find function to tag?
        MainGameManger = GameObject.Find("GameManager").GetComponent<IGameManager>();
        globalEvent = MainGameManger.GetService<IGlobalEventService>();
        base.Awake();

        SetBattleState(BattleStateType.init_done);
        //globalEvent.RaiseBattleStateUpdatedGlobal(this, new BattleStateEventArgs(BattleStateType.init_done));
        AudioEXManager.Instance.PlayBgm(Sound.BATTLE_BGM);
        MainGameManger.SetState(GameStateType.battle);


    }

    public override IGameManager GetMainGameManager()
    {
        return MainGameManger;
    }

    public override void SetBattleState(BattleStateType newState, float duration = 0, BattleResult result = BattleResult.Win)
    {
        
        StartCoroutine(ChangeGameState(newState, duration, result));

        
    }

    IEnumerator ChangeGameState(BattleStateType newState, float duration, BattleResult result)
    {
        yield return new WaitForSeconds(duration);
        BattleState = newState;
        //globalEvent.RaiseBattleStateUpdatedGlobal(this, new BattleStateEventArgs(BattleState));
        if (globalEvent != null)
            globalEvent.RaiseBattleStateUpdatedGlobal(this, new BattleStateEventArgs(BattleState));

        if (newState == BattleStateType.end)
        {
            globalEvent.RaiseBattleResultGlobal(this, new BattleResultEventArgs(result));
            // call after 0.2 sec
            DOTween.Sequence().AppendInterval(0.2f).AppendCallback(()=>
            {
                GetMainGameManager().SetState(GameStateType.playing);
                
                SceneManager.UnloadSceneAsync(0);
                AudioEXManager.Instance.PlayBgm(Sound.INGAME_BGM);
            });
            
        }
        //globalEvent.RaiseBattleStateUpdatedGlobal(battleGameMgr, new BattleStateEventArgs(type));
    }
}
