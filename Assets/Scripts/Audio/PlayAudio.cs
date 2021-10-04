using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayAudio : MonoBehaviour
{
    AudioSource audioData;

    public void PlayAudioSource()
    {
        audioData = GetComponent<AudioSource>();
        audioData.Play(0);
    }
}
