using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] CanvasGroup scoreboard;


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
        StartCoroutine(GradualScoreboard());
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
