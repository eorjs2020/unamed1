using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCollisionEventArgs : EventArgs
{
    public CollectableBox box;

    public BoxCollisionEventArgs(CollectableBox box)
    {
        this.box = box; 
    }

}
