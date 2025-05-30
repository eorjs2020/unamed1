using System;
using static Defines;

public class UIStatusEventArgs : EventArgs
{
    public StatusType StatusType { get; private set; }
    public float CurValue { get; private set; }
    public float MaxValue { get; private set; }

    public UIStatusEventArgs(StatusType statusType, float curValue, float maxValue)
    {
        StatusType = statusType;
        CurValue = curValue;
        MaxValue = maxValue;
    }
}
