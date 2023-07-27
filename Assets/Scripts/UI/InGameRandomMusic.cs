using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameRandomMusic : MonoBehaviour
{
    AudioSource source;
    [SerializeField] AudioClip[] musicClips;
    AudioClip currentClip;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    void Start()
    {
        PlayRandomTrack();
        source.Play();
    }

    void PlayRandomTrack()
    {
        currentClip = musicClips[Random.Range(0, musicClips.Length)];
        source.clip = currentClip;
    }
}
