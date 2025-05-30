using System;

public class AudioEventArgs : EventArgs
{
    public AudioType audioType;
    public string clipPath;
    
    public AudioEventArgs(AudioType audioType, string clipPath)
    {
        this.audioType = audioType;
        this.clipPath = clipPath;
    }
}
