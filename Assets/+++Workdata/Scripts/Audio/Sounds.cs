using System;
using UnityEngine;

[Serializable]
public class Sounds
{
    public string name;

    public AudioClip[] audio;

    public bool playOnStart = false;

    [Range(0f, 1f)] public float volume = 1;
    [Range(0f, 2f)] public float pitch = 1;
    public bool loop = false;
    public bool dontInterrupt = false;

    [HideInInspector] public AudioSource source;

    //public Sounds(string soundName, AudioClip audioClip, float audioVolume, float audioPitch, bool shouldLoop, bool shouldPlayOnStart)
    //{
    //    name = soundName;
    //    audio = audioClip;
    //    volume = audioVolume;
    //    pitch = audioPitch;
    //    loop = shouldLoop;
    //    playOnStart = shouldPlayOnStart;
    //}
}
