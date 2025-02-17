using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{

    public static SoundController Instance;


    //Audio Sources
    [Header("Audio Sources")]
    [SerializeField] AudioSource BGMSource;
    [SerializeField] AudioSource SFXSource;

    //BGM samples
    [Header("BGM samples")]
    public AudioClip BGM;

    //SFX samples
    [Header("SFX samples")]
    [Header("Ball related")]
    public AudioClip ballHit;
    public AudioClip ballBounce;
    public AudioClip ballInHole;
    public AudioClip outOfBounds;
    public AudioClip ballReady;

    [Header("SFX Extra")]
    public AudioClip levelCompleted;

    //Instantiate singleton
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
    }

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

    public void PlayMenuSFX(AudioClip sample)
    {
        SFXSource.PlayOneShot(sample, 0.5f);
    }

    public void PlaySFX(AudioClip sample, float vol = 1.0f)
    {
        SFXSource.PlayOneShot(sample, vol);
    }

}
