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
        Invoke(nameof(PlayRandomClip), 1f);
    }

    private void Update()
    {
        if (!source.isPlaying && firstPlay != 0)
        {
            PlayRandomClip();
        }
    }

    void PlayRandomClip()
    {
        firstPlay = 1;
        currentClip = musicClips[Random.Range(0, musicClips.Length)];
        source.PlayOneShot(currentClip);
    }
}
