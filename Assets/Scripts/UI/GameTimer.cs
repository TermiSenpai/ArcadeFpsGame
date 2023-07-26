using System;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameTimer : MonoBehaviourPunCallbacks
{
    [SerializeField] float timerMinutes = 5;
    TMP_Text timerTxt;

    float timerSeconds;

    bool canRestTimer = true;

    // Clave para la propiedad personalizada del temporizador
    const string TimerKey = "GameTimer";

    // delegado
    public delegate void TimerFinish();
    public static TimerFinish timerFinishReleased;

    private void Awake()
    {
        // Solo el maestro de la sala inicializará el temporizador y sincronizará su valor en la sala
        timerSeconds = (float)TimeSpan.FromMinutes(timerMinutes).TotalSeconds;
        if (PhotonNetwork.IsMasterClient)
        {
            Hashtable initialProps = new Hashtable { { TimerKey, timerSeconds } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(initialProps);
        }
    }

    private void Start()
    {
        timerTxt = GetComponent<TMP_Text>();

        // Si el temporizador es mayor a cero, sincronizamos el valor en todos los clientes.
        if (timerSeconds > 0)
        {
            UpdateTimerText(timerSeconds);
            Hashtable initialProps = new Hashtable { { TimerKey, timerSeconds } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(initialProps);
        }
    }

    private void Update()
    {
        // Solo el maestro de la sala actualiza el temporizador y sincroniza el valor.
        if (PhotonNetwork.IsMasterClient && canRestTimer)
        {
            UpdateTimerText(timerSeconds);
            RestTimer();

            // Actualizar el temporizador en las propiedades personalizadas de la sala.
            Hashtable timerProps = new Hashtable { { TimerKey, timerSeconds } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(timerProps);
        }

        CheckTimer();
    }

    void RestTimer()
    {
        timerSeconds -= Time.deltaTime;
    }

    void UpdateTimerText(float timeInSeconds)
    {
        string minutes = TimeSpan.FromSeconds(timeInSeconds).Minutes.ToString();
        string seconds = TimeSpan.FromSeconds(timeInSeconds).Seconds.ToString("D2");
        timerTxt.text = $"{minutes} : {seconds}";
    }

    void CheckTimer()
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

    // Método que se llama automáticamente cuando las propiedades personalizadas de la sala son actualizadas.
    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        if (!PhotonNetwork.IsMasterClient)
            if (propertiesThatChanged.TryGetValue(TimerKey, out object timerValue))
            {
                // Si se recibió una actualización del temporizador, actualizamos el valor local del temporizador y su visualización.
                timerSeconds = (float)timerValue;
                UpdateTimerText(timerSeconds);
            }
    }
}
