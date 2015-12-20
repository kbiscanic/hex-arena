using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class UiAudioManager : MonoBehaviour {

    public AudioClip buttonClickSound;
    public AudioClip buttonHoverSound;

    private AudioSource audioSource;
    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayButtonHoverAudio() {
        audioSource.clip = buttonHoverSound;
        audioSource.Play();
    }

    public void PlayButtonClickAudio() {
        audioSource.clip = buttonClickSound;
        audioSource.Play();
    }

}
