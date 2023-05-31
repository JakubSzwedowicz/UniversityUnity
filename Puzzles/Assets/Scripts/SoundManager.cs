using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource _audioSource;
    public AudioClip PutRight;
    public AudioClip PutWrong;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }

    public void PlaySoundPutRight()
    {
        PlaySound(PutRight);
    }

    public void PlaySoundPutWrong()
    {
        PlaySound(PutWrong);
    }
}