using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGlobalEventService : IGameService
{
    event CustomEventHandler<IGameManager, GameStateEventArgs> GameStateUpdatedGlobal;
    void RaiseGameStateUpdatedGlobal(IGameManager sender, GameStateEventArgs GameStateEvent);

    // input event
    event CustomEventHandler<IGameManager, InputEventArgs> InputUpdatedGlobal;
    void RaiseInputUpdatedGlobal(IGameManager sender, InputEventArgs intputEvent);
    
    // Audio event
    event CustomEventHandler<IGameManager, AudioEventArgs> AudioUpdatedGlobal;
    void RaiseAudioUpdatedGlobal(IGameManager sender, AudioEventArgs audioEvent);

    // Map Update
    event CustomEventHandler<IGameManager, MapUpadateArg> MapUpdatedGlobal;
    void RaiseMapUpdatedGlobal(IGameManager sender, MapUpadateArg mapUpadateArg);

    // Map Save
    public event CustomEventHandler<IGameManager, MapEventArgs> MapCollisionGlobal;
    public void RaiseMapCollisionGlobal(IGameManager sender, MapEventArgs mapEvent);

    //Buff Event
    public event CustomEventHandler<IGameManager, BuffEventArgs> BuffGlobal;
    public void RaiseBuffGlobal(IGameManager sender, BuffEventArgs mapEvent);

    // Player Status UI
    public event CustomEventHandler<IGameManager, UIStatusEventArgs> UIStatusUpdateGlobal;
    public void RaiseUIStatusTextUpdateGlobal(IGameManager sender, UIStatusEventArgs uiStatusEventArgs);

    // Player Teleport
    public event CustomEventHandler<IGameManager, Vector3> PlayerTeleportGlobal;
    public void RaisePlayerTeleportGlobal(IGameManager sender, Vector3 playerTeleportPosition);

    // Battle
    public event CustomEventHandler<IGameManager, BattleEventArgs> BattleEventInfoGlobal;
    public void RaiseBattleEventInfoGlobal(IGameManager sender, BattleEventArgs battleEventArgs);

    // Message event
    public event CustomEventHandler<IGameManager, MessageEventArgs> DisplayMessageGlobal;
    public void RaiseDisplayMessageGlobal(IGameManager sender, MessageEventArgs messageEvent);

    // Item event
    public event CustomEventHandler<IGameManager, ItemEventArgs> PickupItemGlobal;
    public void RaisePickupItemGlobal(IGameManager sender, ItemEventArgs pickupItemEvent);

    public event CustomEventHandler<IGameManager, ItemEventArgs> RemoveItemGlobal;
    public void RaiseRemoveItemGlobal(IGameManager sender, ItemEventArgs RemoveItemEvent);

    // Collision event
    public event CustomEventHandler<IGameManager, BoxCollisionEventArgs> BoxCollisionGlobal;
    public void RaiseBoxCollisionGlobal(IGameManager sender, BoxCollisionEventArgs boxCollisionEvent);

    #region Battle
    // BattleState
    event CustomEventHandler<IGameManager, BattleStateEventArgs> BattleStateUpdatedGlobal;
    void RaiseBattleStateUpdatedGlobal(IGameManager sender, BattleStateEventArgs battleState);

    // When Weapon is selected
    event CustomEventHandler<IGameManager, WeaponInfoEventArgs> SelectedWeaponInfoUpdatedGlobal;
    void RaiseSelectedWeaponInfoUpdatedGlobal(IGameManager sender, WeaponInfoEventArgs WeaponStatsInfo);

    // When card is selected
    event CustomEventHandler<IGameManager, SelectedCardUpdateEventArgs> SelectedCardUpdatedGlobal;
    void RaiseSelectedCardUpdatedGlobal(IGameManager sender, SelectedCardUpdateEventArgs selectedCardUpdateEvent);

    // Weapon Stat updated for attack value
    event CustomEventHandler<IGameManager, WeaponStatsInfoEventArgs> WeaponStatsInfoUpdatedGlobal;
    void RaiseWeaponStatsInfoUpdatedGlobal(IGameManager sender, WeaponStatsInfoEventArgs WeaponStatsInfo);

    // result of battle
    event CustomEventHandler<IGameManager, BattleResultEventArgs> BattleResultGlobal;
    void RaiseBattleResultGlobal(IGameManager sender, BattleResultEventArgs battleResult);

    // for player state
    event CustomEventHandler<IGameManager, PlayerStatBattleUpdateEventArgs> PlayerStatBattleUpdateGlobal;
    void RaisePlayerStatBattleUpdateGlobal(IGameManager sender, PlayerStatBattleUpdateEventArgs playerStatBattleUpdateEventArgs);
  
    // for Enemy state
    event CustomEventHandler<IGameManager, EnemyStatBattleUpdateEventArgs> EnemyStatBattleUpdateGlobal;
    void RaiseEnemyStatBattleUpdateGlobal(IGameManager sender, EnemyStatBattleUpdateEventArgs enemyStatBattleUpdateEventArgs);

    event CustomEventHandler<IGameManager, BarPercentageUpdateEventArgs> BarPercentageUpdateGlobal;
    void RaiseBarPercentageUpdateGlobal(IGameManager sender, BarPercentageUpdateEventArgs barPercentageUpdateEventArgs);

    event CustomEventHandler<IGameManager, BarActionButtonEventArgs> BarActionButtonClickedGlobal;
    void RaiseBarActionButtonClickedGlobal(IGameManager sender, BarActionButtonEventArgs barActionButtonEventArgs);

    event CustomEventHandler<IGameManager, AttackChanceIncrementEventArgs> AttackChanceIncrementUpdateGlobal;
    void RaiseAttackChanceIncrementUpdateGlobal(IGameManager sender, AttackChanceIncrementEventArgs attackChanceIncrementEventArgs);


    #endregion
}
