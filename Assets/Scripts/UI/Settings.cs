using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [Header("Sliders")]
    public Slider musicSlider;
    public Slider soundSlider;
    public Slider volumeSlider;
    public Slider voiceSlider;

    [Header("Screen")]
    public Toggle fullscreenCheckbox;
    // Scrapped
    //public TMP_Dropdown resolutionDropdown;

    public AudioMixer audioMixer;

    /* Methods to save, load, and apply settings for:
    - 3 sliders for MusicVolume, SoundVolume, and MainVolume.
    - A dropdown for choosing resolution.
    - A checkbox to toggle fullscreen.
    */
    public void SetVolume(string property, float volume)
    {
        audioMixer.SetFloat(property, Mathf.Log10(volume) * 20);
    }

    public void SetResolution(string resolution, bool isFullscreen)
    {
        string[] dimensions = resolution.Split('x');
        // Valid resolution format.
        if (dimensions.Length == 2 && int.TryParse(dimensions[0], out int width) && int.TryParse(dimensions[1], out int height))
        {
            Debug.LogError("Resolution changed to: " + dimensions);
            Screen.SetResolution(width, height, isFullscreen);
        }
        else
        {
            Debug.LogError("Invalid resolution format.");
        }

    } 

    public void SaveSettings() 
    {
        // PlayerPrefs.SetFloat("Voice", voiceSlider.value);
        PlayerPrefs.SetFloat("Music", musicSlider.value);
        PlayerPrefs.SetFloat("Sound", soundSlider.value);
        PlayerPrefs.SetFloat("Master", volumeSlider.value);
        //PlayerPrefs.SetInt("Resolution", resolutionDropdown.value);
        PlayerPrefs.SetInt("Fullscreen", fullscreenCheckbox.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        // voiceSlider.value = PlayerPrefs.GetFloat("Voice", 0.75f);
        musicSlider.value = PlayerPrefs.GetFloat("Music", 0.75f);
        soundSlider.value = PlayerPrefs.GetFloat("Sound", 0.75f);
        volumeSlider.value = PlayerPrefs.GetFloat("Master", 0.75f);
        fullscreenCheckbox.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        //resolutionDropdown.value = PlayerPrefs.GetInt("Resolution", resolutionDropdown.value);    
    }

    public void ApplySettings()
    {
        // SetVolume("Voice", voiceSlider.value);
        SetVolume("Master", volumeSlider.value);
        SetVolume("Sound", soundSlider.value);
        SetVolume("Music", musicSlider.value);

        //string selectResolution = resolutionDropdown.options[resolutionDropdown.value].text;
        bool isFullscreen = fullscreenCheckbox.isOn;

        Screen.fullScreen = isFullscreen;
        SaveSettings();
    }
}