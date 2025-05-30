using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarActionButtonEventArgs : EventArgs
{
    public BarActionButton ClickedBtn;
   
    public BarActionButtonEventArgs(BarActionButton clikedBtn)
    {
        ClickedBtn = clikedBtn;
    }
}
