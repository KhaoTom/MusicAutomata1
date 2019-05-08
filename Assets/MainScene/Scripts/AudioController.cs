using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField]
    private AudioSource[] audioSources = new AudioSource[0];
    private float[] targetVolumes;
    private int numSources;

    private void Start()
    {
        numSources = audioSources.Length;
        targetVolumes = new float[numSources];
        for (int i = 0; i < numSources; i++)
        {
            targetVolumes[i] = 0.0f;
        }
    }

    private void Update()
    {
        for (int i = 0; i < numSources; i++)
        {
            AudioSource src = audioSources[i];
            float sv = src.volume;
            float tv = targetVolumes[i];
            
            if (sv < tv)
            {
                src.volume += (tv - sv) * 0.4f;
            }
            else
            {
                src.volume -= (sv - tv) * 0.4f;
            }
        }
    }

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
        targetVolumes[id] = vol;
    }
}
