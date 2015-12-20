using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameOptionsManager : MonoBehaviour {

    public GameObject gameOptionsContainer;
    public Slider masterVolumeSlider;
    public Toggle muteMasterToggle;
    public Slider backgroundVolumeSlider;
    public Toggle muteBackgroundToggle;
    public AudioSource backgroundMusicAudioSource;

    private const string MASTER_VOLUME_LEVEL_KEY = "MasterVolume";
    private const string AUDIO_MUTED_KEY = "AudioMuted";
    private const string BACKGROUND_VOLUME_LEVEL_KEY = "BackgroundVolume";
    private const string BACKGROUND_MUTED_KEY = "BackgroundMuted";

    private float masterVolume;
    private bool masterAudioMuted = false;

    private float backgroundVolume;
    private bool backgroundMuted = false;

    private void Start() {
        InitAudioSettings();
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
            masterAudioMuted = true;
            AudioListener.volume = 0;
        }
        else {
            masterAudioMuted = false;
        }

        if (PlayerPrefs.HasKey(BACKGROUND_VOLUME_LEVEL_KEY) == false) {
            backgroundVolume = 1.0f;
        }
        else {
            backgroundVolume = PlayerPrefs.GetFloat(BACKGROUND_VOLUME_LEVEL_KEY);
        }
        backgroundMusicAudioSource.volume = backgroundVolume;

        if (PlayerPrefs.HasKey(BACKGROUND_MUTED_KEY)) {
            backgroundMuted = true;
            backgroundMusicAudioSource.volume = 0;
        }
        else {
            backgroundMuted = false;
        }
    }

    public void ShowOptions() {
		Debug.Log (gameOptionsContainer);

        masterVolumeSlider.value = masterVolume;
        muteMasterToggle.isOn = masterAudioMuted;
        backgroundVolumeSlider.value = backgroundVolume;
        muteBackgroundToggle.isOn = backgroundMuted;
        gameOptionsContainer.SetActive(true);
    }

    public void MuteMasterVolumeToggle(bool state) {
        masterAudioMuted = state;
        AudioListener.volume = GetVolumeLevel(masterVolume, masterAudioMuted);
    }
    public void MuteBackgroundVolumeToggle(bool state) {
        backgroundMuted = state;
        backgroundMusicAudioSource.volume = GetVolumeLevel(backgroundVolume, backgroundMuted);
    }

    public void MasterVolumeSliderChanged(float value) {
        masterVolume = value;
        AudioListener.volume = GetVolumeLevel(masterVolume, masterAudioMuted);
    }

    public void BackgroundVolumeSliderChanged(float value) {
        backgroundVolume = value;
        backgroundMusicAudioSource.volume = GetVolumeLevel(backgroundVolume, backgroundMuted);
    }

    public void CancelChanges() {
        InitAudioSettings();
        HideOptions();
    }

    public void SaveChanges() {
        if (masterAudioMuted) {
            PlayerPrefs.SetInt(AUDIO_MUTED_KEY, 1);
        }
        else {
            PlayerPrefs.DeleteKey(AUDIO_MUTED_KEY);
        }
        PlayerPrefs.SetFloat(MASTER_VOLUME_LEVEL_KEY, masterVolume);
        if (backgroundMuted) {
            PlayerPrefs.SetInt(BACKGROUND_MUTED_KEY, 1);
        }
        else {
            PlayerPrefs.DeleteKey(BACKGROUND_MUTED_KEY);
        }
        PlayerPrefs.SetFloat(BACKGROUND_VOLUME_LEVEL_KEY, backgroundVolume);
        HideOptions();
    }

    public void HideOptions() {
        gameOptionsContainer.SetActive(false);
    }
    private float GetVolumeLevel(float value, bool audioMuted) {
        if (audioMuted) {
            return 0;
        }
        else {
            return Mathf.Clamp(value, 0f, 1f);
        }
    }
}
