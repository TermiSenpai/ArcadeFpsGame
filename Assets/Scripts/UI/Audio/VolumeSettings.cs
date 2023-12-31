using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;

public class VolumeSettings : MonoBehaviour
{
    #region Variables
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer mixer;
    [Header("Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider fxSlider;

    // mixer exposed names
    private const string masterName = "MasterVolume";
    private const string musicName = "MusicVolume";
    private const string fxName = "FxVolume";

    #endregion

    #region Unity
    private void Start()
    {
        LoadSaveValues();
        AddSliderListeners();
        SetAllVolumes();
    }

    #endregion

    #region Load
    private void SetAllVolumes()
    {
        // Establecer los valores iniciales de volumen llamando a SetVolume() directamente
        SetVolume(masterName, masterSlider.value);
        SetVolume(musicName, musicSlider.value);
        SetVolume(fxName, fxSlider.value);
    }

    private void LoadSaveValues()
    {
        float masterVolume = PlayerPrefs.GetFloat(masterName, 0.5f);
        float musicVolume = PlayerPrefs.GetFloat(musicName, 0.5f);
        float sfxVolume = PlayerPrefs.GetFloat(fxName, 0.5f);

        masterSlider.value = masterVolume;
        musicSlider.value = musicVolume;
        fxSlider.value = sfxVolume;
    }
    #endregion

    #region Action
    private void AddSliderListeners()
    {
        // Agregar listeners para mantener el volumen actualizado cuando se interact�a con los sliders
        masterSlider.onValueChanged.AddListener(delegate { SetVolume(masterName, masterSlider.value); });
        musicSlider.onValueChanged.AddListener(delegate { SetVolume(musicName, musicSlider.value); });
        fxSlider.onValueChanged.AddListener(delegate { SetVolume(fxName, fxSlider.value); });
    }

    private void SetVolume(string volumeType, float volume)
    {
        mixer.SetFloat(volumeType, Mathf.Log10(volume) * 20f);
        PlayerPrefs.SetFloat(volumeType, volume);
    }
    #endregion
}
