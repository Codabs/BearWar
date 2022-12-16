using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Options : MonoBehaviour
{

    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown FrequencyDropdown;
    public TMP_Dropdown QualityDropdown;

    public Toggle fullscreenToggle;

    Resolution[] resolutions;
    List<Resolution> _resolutions;
    int SelectedResolution;
    List<int> HZ;
    int SelectedFrequency;

    void Start()
    {
        resolutions = Screen.resolutions;

        List<string> options = new List<string>();
        List<string> hz = new List<string>();
        HZ = new List<int>();
        _resolutions = new List<Resolution>();

        int currentResolutionIndex = 0;
        int currentFrequencyIndex = Screen.currentResolution.refreshRate;
        int y = -1;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            string _hz = resolutions[i].refreshRate + " hz";

            if (!options.Contains(option))
            {
                y++;
                options.Add(option);
                _resolutions.Add(resolutions[i]);

            }

            if (resolutions[i].Equals(Screen.currentResolution))
                currentResolutionIndex = y;


            if (!HZ.Contains(resolutions[i].refreshRate))
            {
                HZ.Add(resolutions[i].refreshRate);
                hz.Add(_hz);
            }
        }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        FrequencyDropdown.ClearOptions();
        FrequencyDropdown.AddOptions(hz);
        FrequencyDropdown.value = currentFrequencyIndex;
        FrequencyDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, Application.targetFrameRate);
        SelectedResolution = resolutionIndex;
    }

    public void SetFrequency(int FrequencyIndex)
    {
        int frequency = HZ[FrequencyIndex];
        Application.targetFrameRate = frequency;
        SelectedFrequency = FrequencyIndex;
    }
    public void SetQuality(int qualityIndex) => QualitySettings.SetQualityLevel(qualityIndex);
    public void SetFullscreen(bool isFullscreen) => Screen.fullScreen = isFullscreen;
}
