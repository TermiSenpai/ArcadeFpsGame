using UnityEngine;

public class PlayerWalkSound : MonoBehaviour
{
    [Header("References")]
    [SerializeField] PlayerMovConfig config;
    [SerializeField] AudioSource source;
    [SerializeField] PlayerJump PlayerJump;
    [SerializeField] PlayerMovement playerMov;

    [Header("Interval")]
    [SerializeField] float stepInterval;
    float steptimer;

    void Update()
    {
        steptimer -= Time.deltaTime;
        if (playerMov.GetMovementDir().magnitude >= 0.1f && steptimer <= 0 && PlayerJump.IsGrounded())
        {
            PlayStepSound();
            steptimer = stepInterval;

        }
    }

    private void PlayStepSound()
    {
        source.PlayOneShot(config.walkClip);
    }
}
