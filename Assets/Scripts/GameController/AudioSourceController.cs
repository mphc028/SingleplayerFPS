using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceController : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource= GetComponent<AudioSource>();
    }

    public void Play(AudioClip audio)
    {
        audioSource.clip = audio;
        audioSource.Play();
    }
}
