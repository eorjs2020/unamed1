
using UnityEngine;

public interface  IPlayerControllerService : IGameService
{
    public void TargetPosition(Vector3 position);
    public PlayerCharacter PlayerCharacter { get; }
}
