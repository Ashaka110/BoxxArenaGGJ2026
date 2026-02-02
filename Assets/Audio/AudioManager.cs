
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip Hit;
    [SerializeField] private AudioClip Shoot;
    [SerializeField] private AudioClip BigDestroy;

    public static AudioManager Instance;
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioSource _Musicsource;

    private void Awake()
    {
        Instance = this;
    }

    public void PlaySound(int id)
    {
        switch (id)
        {
            case 0:
               PlayClip(Hit);
                break;
            case 1:
                PlayClip(Shoot);
                break;
            case 2:
                PlayClip(BigDestroy);
                break;
        }
    }

    void PlayClip(AudioClip clip)
    {
        _source.PlayOneShot(clip);
    }

    public void StopMusic()
    {
        _Musicsource.Stop();
    }
}
