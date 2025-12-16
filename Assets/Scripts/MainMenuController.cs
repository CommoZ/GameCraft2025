using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio; // Required for AudioMixer

public class MainMenuController : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject optionsPanel;

    [Header("Audio")]
    public AudioMixer mainMixer;

    // --- Scene Management ---
    public void PlayGame()
    {
        SceneManager.LoadScene("RyanTest");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // --- Options Menu UI ---
    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
    }

    // --- Audio Control ---
    // Link this to the Music Slider's "On Value Changed"
    public void SetMusicVolume(float sliderValue)
    {
        // Convert linear slider value (0-1) to logarithmic decibels (-80 to 0)
        mainMixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
    }

    // Link this to the SFX Slider's "On Value Changed"
    public void SetSFXVolume(float sliderValue)
    {
        mainMixer.SetFloat("SFXVol", Mathf.Log10(sliderValue) * 20);
    }
}