using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CardType
{
    HEAD,
    BODY,
    ARM
}

public class SelectedCardUpdateEventArgs : EventArgs
{
    public CardType CardType;

    public SelectedCardUpdateEventArgs(CardType cardType)
    {
        CardType = cardType;
    }
}
