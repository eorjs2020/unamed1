using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVFXService : IGameService
{
    public void ScreenFadeInOut(System.Action<int> callback, int val);
    public void StartBattleEffect(System.Action callback);
}
