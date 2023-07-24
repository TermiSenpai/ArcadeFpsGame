using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMenuMusic : MonoBehaviour
{
    AudioSource source;
    [SerializeField] private AudioClip[] musicClips;
    AudioClip currentClip;
    int firstPlay = 0;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        Invoke(nameof(playRandomClip), 1f);
    }

    private void Update()
    {
        if (!source.isPlaying && firstPlay != 0)
        {
            playRandomClip();
        }
    }

    void playRandomClip()
    {
        firstPlay = 1;
        currentClip = musicClips[Random.Range(0, musicClips.Length)];
        source.PlayOneShot(currentClip);
    }
}
