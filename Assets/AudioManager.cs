using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager Instance;
    public AudioMixerGroup mixer;
    public int SceneMusic = 0;
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = mixer;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    void Start()
    {
        switch (SceneMusic)
        {
            case 0:
                Play("TemaMenus");
                break;
            case 1:
                Play("TemaPeleaBosque");
                break;
            case 2:
                Play("TemaPeleaAgua");
                break;
            case 3:
                Play("TemaPeleaMontaña");
                break;
            case 4:
                Play("TemaPeleaDungeon");
                break;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    public void PlayAttackSound()
    {
        AudioManager.Instance.Play("AtaqueAliado");
    }
}
