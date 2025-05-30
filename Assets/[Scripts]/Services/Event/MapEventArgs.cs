using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum MapArgType
{
    UPSTAIR,
    DOWNSTAIR,
    ELEVATOR,
    ROOMEXIT,
    ROOM1,
    ROOM2,
    ROOM3,
    ROOM4,
    ROOM5,
    ROOM6,
    ROOM7,
    ROOM8,
    DESTROYENEMY,
    DESTROYTRESURE,
    UPDATE,    
    GOTOLOBBY,
    ITEM,
    ENEMY
};


public class MapEventArgs : EventArgs
{
    public MapArgType mapArgType;
    public Vector2 CollisionDirection;
    public int MapNum;
    

    public MapEventArgs(MapArgType arg, Vector2 collisionDirection, int mapNum = 0)
    {
        this.mapArgType = arg;
        this.CollisionDirection = collisionDirection;
        this.MapNum = mapNum;
    }
    
}

