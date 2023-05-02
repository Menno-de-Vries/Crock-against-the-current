using UnityEngine;
using System;

[System.Serializable]
public class Sound
{
    public string Name;
    public AudioClip Clip;
    [Range(0f, 1f)]
    public float Volume;
    [Range(1f, 5f)]
    public float Pitch;
    public bool Loop;

    [HideInInspector]
    public AudioSource Source;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public Sound[] Sounds;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in Sounds)
        {
            s.Source = gameObject.AddComponent<AudioSource>();
            s.Source.playOnAwake = false;
            s.Source.clip = s.Clip;
            s.Source.volume = s.Volume;
            s.Source.pitch = s.Pitch;
            s.Source.loop = s.Loop;
        }

        Play("MainMenu");
    }

    private void Update()
    {
        foreach (Sound s in Sounds)
        {
            if (s.Source.time >= s.Source.clip.length && !s.Source.loop)
            {
                Stop(s.Name);
            }
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(Sounds, sound => sound.Name == name);

        if (s == null)
        {
            Debug.LogError("Sound: " + name + " not found!");
            return;
        }

        s.Source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(Sounds, sound => sound.Name == name);

        if (s == null)
        {
            Debug.LogError("Sound: " + name + " not found!");
            return;
        }

        s.Source.Stop();
    }
}
