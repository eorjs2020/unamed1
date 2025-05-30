using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Sound
{
    /////////////////////////////////////////
    // create like below by using sound file name.
    // string should be same with the file name. Care about upper case.
    public static Sound INGAME_BGM = new Sound("Game");
    public static Sound BATTLE_BGM = new Sound("Battle");
    public static Sound FIRE_FX = new Sound("Fire");




    /////////////////////////////////////////
    #region Ristrited Constructor
    public string Name
    {
        get;
        private set;
    }
    private Sound(string name) => Name = name;
    #endregion
}

[System.Serializable]
public class CustomAudio
{
    public AudioClip audioClip = null;
    public string name = "";
}

public class AudioEXManager : MonoBehaviour
{
    // for Singltone pattern
    private static AudioEXManager instance;

    public static AudioEXManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject singletonObject = new GameObject();
                instance = singletonObject.AddComponent<AudioEXManager>();
                singletonObject.name = typeof(AudioEXManager).ToString() + " (Singleton)";

                DontDestroyOnLoad(singletonObject);
            }
            return instance;
        }
    }

    [Header("[Background Sounds]")]
    public List<CustomAudio> backgroundSoundClips = new List<CustomAudio>();
    [Header("[FX Sounds]")]
    public List<CustomAudio> effectSoundClips = new List<CustomAudio>();


    [Header("[For Checking Sound Playing]")]
    public AudioSource bgm1;
    public AudioSource bgm2;
    public AudioSource[] audioSourceArray;

    [SerializeField, Range(0f, 1f)]
    private float _BgmVolume = 0.5f;
    [SerializeField]
    private float _SfxVolume = 1f;

    public float BgmVolume
    {
        get { return _BgmVolume; }
        set
        {
            if (value > 1.0f)
            {
                _BgmVolume = 1.0f;
            }
            if (value < 0.0f)
            {
                _BgmVolume = 0.0f;
            }
            _BgmVolume = value;
            UpdateAllSoundVolume();
        }

    }

    public float SfxVolume
    {
        get { return _SfxVolume; }
        set
        {
            if (value > 1.0f)
            {
                _SfxVolume = 1.0f;
            }
            if (value < 0.0f)
            {
                _BgmVolume = 0.0f;
            }
            _SfxVolume = value;
            UpdateAllSoundVolume();
        }

    }

    // for playing SFX, each 20 AudioSource can sound at the same time.
    private const int AUDIOMAXNUM = 30;
    private int sourceIdx = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        bgm1 = gameObject.AddComponent<AudioSource>();
        bgm2 = gameObject.AddComponent<AudioSource>();
        // for first Fade In of BGM
        bgm1.volume = 0;
        bgm2.volume = 0;

        // loading all sound assets from Resources folder.
        AudioClip[] sounds = Resources.LoadAll<AudioClip>("Audio/BGM");
        if (sounds.Length == 0)
        {
            Debug.LogWarning("There are no BGM Sounds");
        }

        foreach (AudioClip sound in sounds)
        {
            CustomAudio tmp = new CustomAudio();
            tmp.audioClip = sound;
            tmp.name = sound.name;
            backgroundSoundClips.Add(tmp);
        }

        sounds = Resources.LoadAll<AudioClip>("Audio/EFFECT");
        if (sounds.Length == 0)
        {
            Debug.LogWarning("There are no FX Sounds");
        }

        foreach (AudioClip sound in sounds)
        {
            CustomAudio tmp = new CustomAudio();
            tmp.audioClip = sound;
            tmp.name = sound.name;
            effectSoundClips.Add(tmp);
        }



        audioSourceArray = new AudioSource[AUDIOMAXNUM];

        for (int i = 0; i < audioSourceArray.Length; i++)
        {
            audioSourceArray[i] = gameObject.AddComponent<AudioSource>();
        }

    }


    public void OnDisable()
    {

    }

    /// <summary>
    /// Play Sound Effects
    /// 
    /// </summary>
    /// <param name="audioName"> Sound class for playing </param>
    /// <param name="loop"> Loop do you want? </param>
    public void PlayFX(Sound audioName, bool loop = false)
    {
        // if this AudioSource is playing, just use other one.
        int numOfPlayingSource = 0;
        while (audioSourceArray[sourceIdx].isPlaying)
        {
            numOfPlayingSource++;
            sourceIdx++;
            if (sourceIdx >= AUDIOMAXNUM)
            {
                sourceIdx = 0;
            }

            if (numOfPlayingSource >= AUDIOMAXNUM)
            {
                Debug.LogError("Over "+ AUDIOMAXNUM + "FX AuudioSources are being used!!");
                break;
            }
        }

        // not so expensive..
        CustomAudio sound = effectSoundClips.Find(source => source.name == audioName.Name);

        if (sound == null)
        {
            Debug.LogError("Sound " + audioName + " is missing!!");
            return;
        }
        audioSourceArray[sourceIdx].clip = sound.audioClip;
        audioSourceArray[sourceIdx].loop = loop;
        audioSourceArray[sourceIdx].volume = _SfxVolume;
        audioSourceArray[sourceIdx].Play();
    }

    /// <summary>
    /// Play background music. It will fade out and in
    /// </summary>
    /// <param name="audioName"> Sound class for playing </param> 
    /// <param name="fadeDuration">Loop do you want?</param>
    public void PlayBgm(Sound audioName, float fadeDuration = 2.0f)
    {
        if (fadeDuration == 0.0f)
        {
            return;
        }
        StopAllCoroutines();

        CustomAudio sound = backgroundSoundClips.Find(source => source.name == audioName.Name);
        if (sound == null)
        {
            Debug.LogError("Sound " + audioName + " is missing!!");
            return;
        }
        if (bgm1.isPlaying)
        {
            bgm2.clip = sound.audioClip;
            StartCoroutine(PlaySoundFadeIn(bgm2, fadeDuration, _BgmVolume));
            StartCoroutine(StopSoundFadeIn(bgm1, fadeDuration));

        }
        else if (bgm2.isPlaying)
        {
            bgm1.clip = sound.audioClip;
            StartCoroutine(PlaySoundFadeIn(bgm1, fadeDuration, _BgmVolume));
            StartCoroutine(StopSoundFadeIn(bgm2, fadeDuration));
        }
        else
        {
            bgm1.clip = sound.audioClip;
            StartCoroutine(PlaySoundFadeIn(bgm1, fadeDuration, _BgmVolume));
        }
    }

    private IEnumerator PlaySoundFadeIn(AudioSource source, float duration, float volume)
    {

        source.loop = true;
        source.Play();

        float tmpVolume = source.volume;

        float factor = (volume - tmpVolume) / (duration * 50);


        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            tmpVolume += factor;
            source.volume = tmpVolume;
            if (source.volume >= volume)
            {
                source.volume = volume;
                break;
            }
        }
    }

    private IEnumerator StopSoundFadeIn(AudioSource source, float duration)
    {

        float tmpVolume = source.volume;
        float factor = tmpVolume / (duration * 50);


        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            tmpVolume -= factor;
            source.volume = tmpVolume;
            if (source.volume <= 0.0f)
            {
                source.Stop();
                break;
            }
        }
    }

    private void UpdateAllSoundVolume()
    {
        bgm1.volume = _BgmVolume;
        bgm2.volume = _BgmVolume;
        foreach (var sound in audioSourceArray)
        {
            sound.volume = _SfxVolume;
        }
    }

    private void OnValidate()
    {
        UpdateAllSoundVolume();
    }

}