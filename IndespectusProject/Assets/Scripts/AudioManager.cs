using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public List<AudioClip> sounds = new List<AudioClip>();

    public float audioVolume;

    public void PlaySound(AudioSource source, string sound, float volume)
    {
        foreach (AudioClip clip in sounds)
        {
            if (clip.name == sound)
            {
                SetVolume(source);
                source.PlayOneShot(clip, volume);
                //print("PLAYED SOUND");
            }
        }
    }

    public void PlayRepeatingSound(AudioSource source, string sound)
    {
        foreach (AudioClip clip in sounds)
        {
            if (clip.name == sound)
            {
                SetVolume(source);
                source.loop = true;
                source.clip = clip;
                source.Play();
                //print("PLAYED SOUND");
            }
        }
    }

    public void StopRepeatingSound(AudioSource source)
    {
        source.Stop();
    }

    private void SetVolume(AudioSource source)
    {
        source.volume = audioVolume;
    }
}
