using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameOptionsManager : MonoBehaviour {

    public GameObject gameOptionsContainer;
    public Slider masterVolumeSlider;
    public Toggle muteToggle;

    private const string MASTER_VOLUME_LEVEL_KEY = "MasterVolume";
    private const string AUDIO_MUTED_KEY = "AudioMuted";

    private float masterVolume;
    private bool audioMuted = false;

    private void Start() {
        InitAudioSettings();
    }

    public void ShowOptions() {
        masterVolumeSlider.value = masterVolume;
        muteToggle.isOn = audioMuted;
        gameOptionsContainer.SetActive(true);
    }

    public void MuteAudioToggle(bool state) {
        audioMuted = state;
    }

    public void AudioSliderChanged(float value) {
        masterVolume = Mathf.Clamp(masterVolumeSlider.value, 0f, 1f);
    }

    public void CancelChanges() {
        InitAudioSettings();
        HideOptions();
    }

    public void SaveChanges() {
        if (audioMuted) {
            PlayerPrefs.SetInt(AUDIO_MUTED_KEY, 1);
        }
        else {
            PlayerPrefs.DeleteKey(AUDIO_MUTED_KEY);
        }
        PlayerPrefs.SetFloat(MASTER_VOLUME_LEVEL_KEY, masterVolume);
        HideOptions();
    }

    public void HideOptions() {
        gameOptionsContainer.SetActive(false);
    }

    private void InitAudioSettings() {
        if (PlayerPrefs.HasKey(MASTER_VOLUME_LEVEL_KEY) == false) {
            masterVolume = 1.0f;
        }
        else {
            masterVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_LEVEL_KEY);
        }
        AudioListener.volume = masterVolume;

        if (PlayerPrefs.HasKey(AUDIO_MUTED_KEY)) {
            audioMuted = true;
            AudioListener.volume = 0;
        }
        else {
            audioMuted = false;
        }
    }
}
