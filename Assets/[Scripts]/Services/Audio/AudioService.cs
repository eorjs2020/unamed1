using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using Unity.VisualScripting;

public enum AudioType
{
    BGM,
    EFFECT,
    MAX,
}

public class AudioService : MonoBehaviour, IAudioService
{
    protected IGameManager gameMgr;
    protected IGlobalEventService globalEvent;

    [SerializeField]
    private AudioSource[] _audioSources = new AudioSource[(int)AudioType.MAX];
    
    public void Init(IGameManager gameManager) 
    {
        gameMgr = gameManager;
        globalEvent = gameMgr.GetService<IGlobalEventService>();

        globalEvent.AudioUpdatedGlobal += PlayEventHandler;
    }

    public void OnDisable()
    {
        globalEvent.AudioUpdatedGlobal -= PlayEventHandler;
    }

    public void Play(AudioType type)
    {
        AudioSource audioSource = _audioSources[(int)type];
        audioSource.Play();
    }

    public void Play(AudioType type, AudioClip audioClip, float pitch = 1.0f)
    {
        AudioSource audioSource = _audioSources[(int)type];

        if (type == AudioType.BGM)
        {
            if (audioSource.isPlaying) audioSource.Stop();

            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else
        {
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
    }

    public void PlayBgm(Sound audioName, float fadeDuration = 2)
    {
        throw new NotImplementedException();
    }

    public void PlayEventHandler(IGameManager sender, AudioEventArgs e)
    {
        AudioSource audioSource = _audioSources[(int)e.audioType];
        AudioClip audioClip = Resources.Load<AudioClip>(e.clipPath);

        if (e.audioType == AudioType.BGM)
        {
            if (audioSource.isPlaying) audioSource.Stop();

            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else
        {
            audioSource.pitch = 1.0f;
            audioSource.PlayOneShot(audioClip);
        }
    }

    public void PlayFX(Sound audioName, bool loop = false)
    {
        throw new NotImplementedException();
    }

    public void Stop(AudioType type)
    {
        AudioSource audioSource = _audioSources[(int)type];
        audioSource.Stop();
    }
}


