using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapUpadateArg : EventArgs
{
    public int RoomNumber;
    public int MapNumber;

    public MapUpadateArg(int argMapNum, int RoomNum)
    {
        this.RoomNumber = RoomNum;
        this.MapNumber = argMapNum;
    }
}
