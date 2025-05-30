using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattleTransferService : IGameService
{
    public Enemy CurrentEnemy { get; set; }
}
