using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioType[] audioTypes;

    private void Awake()
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
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        foreach (AudioType type in audioTypes)
        {
            type.Source = gameObject.AddComponent<AudioSource>();
            type.Source.clip = type.Clip;
            type.Source.volume = type.Volume;
            type.Source.pitch = type.Pitch;
            type.Source.loop = type.Loop;
            type.Source.playOnAwake = type.PlayOnAwake;

            if (type.Group != null)
            {
                type.Source.outputAudioMixerGroup = type.Group;
            }
        }
    }

    public void Play(string name)//播放音效
    {
        AudioType type = Array.Find(audioTypes, audioType => audioType.Name == name);
        if (type == null)
        {
            Debug.LogWarning("Audio: " + name + " not found!");
            return;
        }
        type.Source.Play();
    }

    public void Play(string name, float pitch = 1f) // 支持动态设置 pitch
    {
        AudioType type = Array.Find(audioTypes, audioType => audioType.Name == name);
        if (type == null)
        {
            Debug.LogWarning("Audio: " + name + " not found!");
            return;
        }
        type.Source.pitch = pitch;
        type.Source.Play();
    }

    public void Stop(string name)//停止音效
    {
        AudioType type = Array.Find(audioTypes, audioType => audioType.Name == name);
        if (type == null)
        {
            Debug.LogWarning("Audio: " + name + " not found!");
            return;
        }
        type.Source.Stop();
    }


    public void Pause(string name)//暂停音效
    {
        AudioType type = Array.Find(audioTypes, audioType => audioType.Name == name);
        if (type == null)
        {
            Debug.LogWarning("Audio: " + name + " not found!");
            return;
        }
        type.Source.Pause();
    }

    public void SetPitch(string name, float pitch) // 动态调整 pitch
    {
        AudioType type = Array.Find(audioTypes, audioType => audioType.Name == name);
        if (type == null)
        {
            Debug.LogWarning("Audio: " + name + " not found!");
            return;
        }
        type.Source.pitch = pitch;
    }

    public bool IsPlaying(string name)//判断音效是否正在播放
    {
        AudioType type = Array.Find(audioTypes, audioType => audioType.Name == name);
        if (type == null)
        {
            Debug.LogWarning("Audio: " + name + " not found!");
            return false;
        }
        return type.Source.isPlaying;
    }

    //调整音量大小
    public void SetVolume(string name, float volume)
    {
        AudioType type = Array.Find(audioTypes, audioType => audioType.Name == name);
        if (type == null)
        {
            Debug.LogWarning("Audio: " + name + " not found!");
            return;
        }
        type.Source.volume = volume;
    }
}
