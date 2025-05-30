using System;

public class BuffEventArgs : EventArgs
{
    public RoomType roomType;

    public BuffEventArgs(RoomType RoomType)
    {
       roomType = RoomType;
    }
}
