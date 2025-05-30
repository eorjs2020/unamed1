using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleUIService : MonoBehaviour, IUIService
{

    protected BattleGameManager battleGameMgr;
    protected IGlobalEventService globalEvent;
    //protected IPlayerControllerService playerControllerService;

    public void Init(IGameManager gameManager)
    {
        battleGameMgr = (BattleGameManager)gameManager;
        globalEvent = battleGameMgr.MainGameManger.GetService<IGlobalEventService>();
        InitComponent();
    }

    public void InitComponent()
    {
        GetComponentsInChildren<IUIComponent>().ToList().ForEach(component => { component.Init(battleGameMgr); });
    }

    public void UpdateStatusUIText(Defines.StatusType type, float curValue, float maxValue)
    {
        throw new System.NotImplementedException();
    }
}
