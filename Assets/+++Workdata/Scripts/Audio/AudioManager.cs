using UnityEngine;
using System;
using NUnit.Framework;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public List<Sounds> sounds;

    public static AudioManager instance;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sounds s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.playOnAwake = s.playOnStart;

            s.source.clip = s.audio[UnityEngine.Random.Range(0, s.audio.Length)];

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name)
    {
        Sounds s = sounds.Find (s => s.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }
        if (s.source.isPlaying && s.dontInterrupt == true)
            return;

        s.source.clip = s.audio[UnityEngine.Random.Range(0, s.audio.Length)];
        s.source.Play();
    }
}
