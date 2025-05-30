using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SegmentOverlaped
{
    RED, // first
    GREEN, // Second
    BLUE, // Third
}

public class BarPercentageUpdateEventArgs : EventArgs
{
    public float BarPercentage;
    public SegmentOverlaped Overlaped;

    public BarPercentageUpdateEventArgs(float barPercentage, SegmentOverlaped overlaped)
    {
        BarPercentage = barPercentage;
        Overlaped = overlaped;
    }
}
