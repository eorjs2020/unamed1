using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputEventArgs : EventArgs
{
    public Vector2 TouchPoint;

    public InputEventArgs(Vector2 touchPoint)
    {
        this.TouchPoint = touchPoint;
    }

}