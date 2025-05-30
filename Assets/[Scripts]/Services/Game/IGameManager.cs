using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameManager : IServicePublisher<IGameService>
{

    GameStateType State { get; }
    BattleStateType BattleState { get; }

    IGameManager GetMainGameManager();

    void SetState(GameStateType newState);

    void SetBattleState(BattleStateType newState, float duration = 0, BattleResult result = BattleResult.Win);

}
