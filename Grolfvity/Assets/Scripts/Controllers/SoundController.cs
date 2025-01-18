using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    //Audio Sources
    [Header("Audio Sources")]
    [SerializeField] AudioSource BGMSource;
    [SerializeField] AudioSource SFXSource;

    //BGM samples
    [Header("BGM samples")]
    public AudioClip BGM;

    //SFX samples
    [Header("SFX samples")]
    public AudioClip ballHit;
    public AudioClip ballBounce;
    public AudioClip ballOnHole;
    public AudioClip levelCompleted;

    void Start()
    {
        BGMSource.clip = BGM;
        BGMSource.ignoreListenerPause = true;
    }

    public void PlayBGM()
    {
        BGMSource.Play();
    }

    public void PauseBGM()
    {
        BGMSource.Pause();
    }

    public void StopBGM()
    {
        BGMSource.Stop();
    }

    public void PlaySFX(AudioClip sample, float vol = 1.0f)
    {
        SFXSource.PlayOneShot(sample, vol);
    }

}
