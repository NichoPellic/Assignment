using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Dropdown resolutionDropdown;
    public AudioMixer mixer;

    private int _index = 0;

    Resolution[] resolutions;

    public void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> resOptions = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            resOptions.Add(option);
        }

        resolutionDropdown.AddOptions(resOptions);

        resolutionDropdown.onValueChanged.AddListener(delegate { DropdownItemSelected(resolutionDropdown); });
    }

    void DropdownItemSelected(Dropdown dropdown)
    {
        _index = dropdown.value;

        SetResolution();
    }

    public void SetResolution()
    {
        Screen.SetResolution(resolutions[_index].width, resolutions[_index].height, Screen.fullScreen);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetAudioLevel(float sliderValue)
    {
        mixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
    }
}
