using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void CustomEventHandler<T, E>(T sender, E args);


public class GlobalEventService : MonoBehaviour, IGlobalEventService
{
    #region Attributes
    // Game services
    protected IGameManager gameMgr { private set; get; }
    protected IAudioService audioService { private set; get; }
    protected IMapService mapService { private set; get; }
    #endregion


    public void Init(IGameManager gameManager)
    {
        gameMgr = gameManager;
        audioService = gameMgr.GetService<IAudioService>();
        mapService = gameMgr.GetService<IMapService>();
    }


    #region GameUdpate
    public event CustomEventHandler<IGameManager, GameStateEventArgs> GameStateUpdatedGlobal;
    public void RaiseGameStateUpdatedGlobal(IGameManager sender, GameStateEventArgs e)
    {
        var handler = GameStateUpdatedGlobal;
        handler?.Invoke(sender, e);
    }
    #endregion


    #region InputUpdate
    public event CustomEventHandler<IGameManager, InputEventArgs> InputUpdatedGlobal;
    public void RaiseInputUpdatedGlobal(IGameManager sender, InputEventArgs e)
    {
        var handler = InputUpdatedGlobal;
        handler?.Invoke(sender, e);
    }
    #endregion


    #region AudioUpdate
    public event CustomEventHandler<IGameManager, AudioEventArgs> AudioUpdatedGlobal;
    public void RaiseAudioUpdatedGlobal(IGameManager sender, AudioEventArgs e)
    {
        var handler = AudioUpdatedGlobal;
        handler?.Invoke(sender, e);
    }
    #endregion


    #region MapSave
    public event CustomEventHandler<IGameManager, MapEventArgs> MapCollisionGlobal;
    public void RaiseMapCollisionGlobal(IGameManager sender, MapEventArgs e)
    {
        var handler = MapCollisionGlobal;
        handler?.Invoke(sender, e);
    }
    #endregion
    
    #region MapUpdate
    public event CustomEventHandler<IGameManager, MapUpadateArg> MapUpdatedGlobal;
    public void RaiseMapUpdatedGlobal(IGameManager sender, MapUpadateArg e)
    {
        var handler = MapUpdatedGlobal;
        handler?.Invoke(sender, e);
    }
    #endregion

    #region Buff
    public event CustomEventHandler<IGameManager, BuffEventArgs> BuffGlobal;

    public void RaiseBuffGlobal(IGameManager sender, BuffEventArgs e)
    {
        var handler = BuffGlobal;
        handler?.Invoke(sender, e);
    }
    #endregion

    public event CustomEventHandler<IGameManager, UIStatusEventArgs> UIStatusUpdateGlobal;
    public void RaiseUIStatusTextUpdateGlobal(IGameManager sender, UIStatusEventArgs e)
    {
        var handler = UIStatusUpdateGlobal;
        handler?.Invoke(sender, e);
    }

    #region Player Teleport

    public event CustomEventHandler<IGameManager, Vector3> PlayerTeleportGlobal;
    public void RaisePlayerTeleportGlobal(IGameManager sender, Vector3 e)
    {
        var handler = PlayerTeleportGlobal;
        handler?.Invoke(sender, e);
    }
    #endregion

    #region Battle Event

    public event CustomEventHandler<IGameManager, BattleEventArgs> BattleEventInfoGlobal;

    public void RaiseBattleEventInfoGlobal(IGameManager sender, BattleEventArgs battleEventArgs)
    {
        Debug.Log("Battle Event Generated.");
        var handler = BattleEventInfoGlobal;
        handler?.Invoke(sender, battleEventArgs);
    }
    #endregion

    #region DisplayMessage
    public event CustomEventHandler<IGameManager, MessageEventArgs> DisplayMessageGlobal;
    public void RaiseDisplayMessageGlobal(IGameManager sender, MessageEventArgs e)
    {
        var handler = DisplayMessageGlobal;
        handler?.Invoke(sender, e);
    }
    #endregion

    #region Pick up Items
    public event CustomEventHandler<IGameManager, ItemEventArgs> PickupItemGlobal;
    public void RaisePickupItemGlobal(IGameManager sender, ItemEventArgs e)
    {
        var handler = PickupItemGlobal;
        handler?.Invoke(sender, e);
    }
    #endregion

    #region Remove Items
    public event CustomEventHandler<IGameManager, ItemEventArgs> RemoveItemGlobal;
    public void RaiseRemoveItemGlobal(IGameManager sender, ItemEventArgs e)
    {
        var handler = RemoveItemGlobal;
        handler?.Invoke(sender, e);
    }
    #endregion

    #region Box Collision
    public event CustomEventHandler<IGameManager, BoxCollisionEventArgs> BoxCollisionGlobal;

    public void RaiseBoxCollisionGlobal(IGameManager sender, BoxCollisionEventArgs e)
    {
        var handler = BoxCollisionGlobal;
        handler?.Invoke(sender, e);
    }

    #endregion


    #region BattleUdpate
    public event CustomEventHandler<IGameManager, BattleStateEventArgs> BattleStateUpdatedGlobal;
    public void RaiseBattleStateUpdatedGlobal(IGameManager sender, BattleStateEventArgs e)
    {
        var handler = BattleStateUpdatedGlobal;
        handler?.Invoke(sender, e);
    }

    public event CustomEventHandler<IGameManager, WeaponInfoEventArgs> SelectedWeaponInfoUpdatedGlobal;
    public void RaiseSelectedWeaponInfoUpdatedGlobal(IGameManager sender, WeaponInfoEventArgs e)
    {
        var handler = SelectedWeaponInfoUpdatedGlobal;
        handler?.Invoke(sender, e);
    }

    public event CustomEventHandler<IGameManager, SelectedCardUpdateEventArgs> SelectedCardUpdatedGlobal;
    public void RaiseSelectedCardUpdatedGlobal(IGameManager sender, SelectedCardUpdateEventArgs e)
    {
        var handler = SelectedCardUpdatedGlobal;
        handler?.Invoke(sender, e);
    }

    public event CustomEventHandler<IGameManager, WeaponStatsInfoEventArgs> WeaponStatsInfoUpdatedGlobal;
    public void RaiseWeaponStatsInfoUpdatedGlobal(IGameManager sender, WeaponStatsInfoEventArgs e)
    {
        var handler = WeaponStatsInfoUpdatedGlobal;
        handler?.Invoke(sender, e);
    }

    public event CustomEventHandler<IGameManager, BattleResultEventArgs> BattleResultGlobal;
    public void RaiseBattleResultGlobal(IGameManager sender, BattleResultEventArgs e)
    {
        var handler = BattleResultGlobal;
        handler?.Invoke(sender, e);
    }

    public event CustomEventHandler<IGameManager, PlayerStatBattleUpdateEventArgs> PlayerStatBattleUpdateGlobal;
    public void RaisePlayerStatBattleUpdateGlobal(IGameManager sender, PlayerStatBattleUpdateEventArgs e)
    {
        var handler = PlayerStatBattleUpdateGlobal;
        handler?.Invoke(sender, e);
    }
    public event CustomEventHandler<IGameManager, EnemyStatBattleUpdateEventArgs> EnemyStatBattleUpdateGlobal;
    public void RaiseEnemyStatBattleUpdateGlobal(IGameManager sender, EnemyStatBattleUpdateEventArgs e)
    {
        var handler = EnemyStatBattleUpdateGlobal;
        handler?.Invoke(sender, e);
    }

    public event CustomEventHandler<IGameManager, BarPercentageUpdateEventArgs> BarPercentageUpdateGlobal;
    public void RaiseBarPercentageUpdateGlobal(IGameManager sender, BarPercentageUpdateEventArgs e)
    {
        var handler = BarPercentageUpdateGlobal;
        handler?.Invoke(sender, e);
    }

    public void RaiseBuffGlobal(IGameManager sender, MapEventArgs mapEvent)
    {
        throw new NotImplementedException();
    }

    public event CustomEventHandler<IGameManager, BarActionButtonEventArgs> BarActionButtonClickedGlobal;
    public void RaiseBarActionButtonClickedGlobal(IGameManager sender, BarActionButtonEventArgs e)
    {
        var handler = BarActionButtonClickedGlobal;
        handler?.Invoke(sender, e);
    }

    public event CustomEventHandler<IGameManager, AttackChanceIncrementEventArgs> AttackChanceIncrementUpdateGlobal;
    public void RaiseAttackChanceIncrementUpdateGlobal(IGameManager sender, AttackChanceIncrementEventArgs e)
    {
        var handler = AttackChanceIncrementUpdateGlobal;
        handler?.Invoke(sender, e);
    }


    #endregion
}
