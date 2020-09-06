using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public List<AudioClip> sounds = new List<AudioClip>();

    public float audioVolume;

    public void PlaySound(AudioSource source, string sound)
    {
        foreach (AudioClip clip in sounds)
        {
            if (clip.name == sound)
            {
                SetVolume(source);
                source.PlayOneShot(clip);
                print("PLAYED SOUND");
            }
        }
    }

    private void SetVolume(AudioSource source)
    {
        source.volume = audioVolume;
    }
}
