using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField]
    private AudioSource[] audioSources = new AudioSource[0];

    public void PlayAll()
    {
        foreach (AudioSource src in audioSources)
        {
            src.Play();
        }
    }

    public void StopAll()
    {
        foreach (AudioSource src in audioSources)
        {
            src.Stop();
        }
    }

    public void Retrigger(int id)
    {
        audioSources[id].time = 0.0f;
    }

    public void SetVolume(int id, float vol)
    {
        audioSources[id].volume = vol;
    }
}
