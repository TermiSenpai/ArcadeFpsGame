using System;
using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] float timerSeconds;
    TMP_Text timerTxt;

    bool canRestTimer = true;

    public delegate void TimerFinish();
    public static TimerFinish timerFinishReleased;

    private void Start()
    {
        timerTxt = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (!canRestTimer) return;

        timerToTxt();
        restTimer();
        checkTimer();
    }

    void restTimer()
    {
        timerSeconds -= Time.deltaTime;
    }

    void timerToTxt()
    {
        string minutes = TimeSpan.FromSeconds(timerSeconds).Minutes.ToString();
        string seconds = TimeSpan.FromSeconds(timerSeconds).Seconds.ToString();
        timerTxt.text = $"{minutes} : {seconds}";
    }

    void checkTimer()
    {
        if (timerSeconds <= 0)
        {
            canRestTimer = false;
            timerSeconds = 0;
            FinishGame();
        }
    }
    private void FinishGame()
    {
        if (timerFinishReleased != null)
            timerFinishReleased();
    }
}
