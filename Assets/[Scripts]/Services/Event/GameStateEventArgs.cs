using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateEventArgs : EventArgs
{
    public GameStateType gameStateType;

    public GameStateEventArgs(GameStateType gameStateType)
    {
        this.gameStateType = gameStateType;
    }
    
}
