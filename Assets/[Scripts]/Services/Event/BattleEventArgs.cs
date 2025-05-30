using System;

public class BattleEventArgs : EventArgs
{
    // Battle Type?, Player, Enemy
    public PlayerCharacter PlayerCharacter;
    public Enemy Enemy;

    public BattleEventArgs(PlayerCharacter player, Enemy enemy)
    {
        PlayerCharacter = player;
        Enemy = enemy;
    }
}
