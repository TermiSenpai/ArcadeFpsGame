using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkSound : MonoBehaviour
{

    [SerializeField] PlayerMovConfig config;
    [SerializeField] float stepInterval;
    float steptimer;

    [SerializeField] AudioSource source;
    [SerializeField] PlayerMovement playerMov;
    [SerializeField] PlayerJump PlayerJump;

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
