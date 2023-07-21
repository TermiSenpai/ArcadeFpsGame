using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    [SerializeField] GameObject[] effects;

    private void OnEnable()
    {
        Invoke(nameof(disableEffect), 2.5f);

        foreach (var effect in effects)
        {
            effect.SetActive(true);
        }
    }

    void disableEffect()
    {
        this.enabled = false;
    }
}
