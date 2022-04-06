using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public Sound[] sounds;
    public static SoundManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
                s.source.playOnAwake = s.playOnAwake;
            }

            AllSound(DataManager.instance.sound);
        }
        else
        {
            DestroyImmediate(this);
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, c => c.name.Equals(name));
        s.source.Play();
    }

    public void AllSound(bool mute)
    {
        foreach (Sound s in sounds)
        {
            s.source.volume = mute ? 0 : s.volume;
        }
    }
}
[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;
    public bool loop;
    public bool playOnAwake;
    [HideInInspector]
    public AudioSource source;
}
