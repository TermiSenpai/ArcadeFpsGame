using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    PhotonTransformView[] photonTransforms;
    [SerializeField] CanvasGroup scoreboard;

    private void Start()
    {
        photonTransforms = FindObjectsOfType<PhotonTransformView>();
    }

    private void OnEnable()
    {
        GameTimer.timerFinishReleased += GameOver;
    }

    private void OnDisable()
    {
        GameTimer.timerFinishReleased -= GameOver;
    }

    void GameOver()
    {
        disablePhotonTransform();
        StartCoroutine(GradualScoreboard());
    }

    void disablePhotonTransform()
    {
        foreach (var t in photonTransforms)
        {
            t.enabled = false;
        }

    }
    public IEnumerator GradualScoreboard()
    {
        while (scoreboard.alpha < 1.0f)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            scoreboard.alpha += Time.deltaTime;
        }
    }
}
