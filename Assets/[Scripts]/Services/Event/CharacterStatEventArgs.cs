using System;

public class CharacterStatEventArgs : EventArgs
{
    public string statName;
    public int value;

    public CharacterStatEventArgs(string statName, int value)
    {
        this.statName = statName;
        this.value = value;
    }
}
