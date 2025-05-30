using UnityEngine;

public interface IAudioService : IGameService
{
    void PlayBgm(Sound audioName, float fadeDuration = 2.0f);
    void PlayFX(Sound audioName, bool loop = false);
}
