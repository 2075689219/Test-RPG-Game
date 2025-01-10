using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class AudioType
{
    public AudioSource Source;
    public AudioClip Clip;
    public AudioMixerGroup Group;


    public string Name;
    public bool Loop;
    public bool PlayOnAwake;
    [Range(0f, 10f)]
    public float Volume = 1f;
    [Range(0.5f, 2f)]
    public float Pitch = 1f;

    // 当在编辑器中调整属性时触发
    public void OnValidate()
    {
        if (Source != null)
        {
            Source.volume = Volume;
            Source.pitch = Pitch;
        }
    }
}
