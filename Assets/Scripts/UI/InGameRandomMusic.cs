using UnityEngine;

public class InGameRandomMusic : MonoBehaviour
{
    [SerializeField] AudioClip[] musicClips;
    AudioSource source;
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
