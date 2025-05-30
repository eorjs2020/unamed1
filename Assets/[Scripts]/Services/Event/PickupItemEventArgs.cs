using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEventArgs : EventArgs
{
    public ItemInfo ItemInfo;
    public int Count;

    public ItemEventArgs(ItemInfo itemInfo , int count)
    {
        ItemInfo = itemInfo;
        Count = count;
    }
   
}
