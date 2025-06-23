using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace MainMenu_Woyboy
{
    public class SettingsManager : MonoBehaviour
    {
        [Header("Assignables - Resolution")]
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [SerializeField] private TMP_Dropdown fullscreenDropdown;

        [Header("Assignables - Audio")]
        [SerializeField] private Slider volumeSlider;
        [SerializeField] private AudioMixer audioMixer;

        [Header("Assignables - Mouse Sensitivity")]
        [SerializeField] private Slider mouseSensitivitySlider;

        private Resolution[] allResolutions;
        private bool isFullScreen;
        private int selectedResolution;

        private List<Resolution> selectedResolutionList = new List<Resolution>();

        // Player Prefs
        private const string MasterVolume = "MasterVolume";
        private const string MouseSensitivity = "MouseSensitivityKey";
        private const string ResolutionIndex = "ResolutionIndex";
        private const string Fullscreen = "Fullscreen";

        private void Start()
        {
            InitializeResolutions();
            LoadSavedResolution();
            LoadSavedFullscreen();
            LoadSavedVolume();
            LoadSavedMouseSensitivity();
        }

        private void InitializeResolutions()
        {
            isFullScreen = true;
            allResolutions = Screen.resolutions;

            List<string> resolutionStringList = new List<string>();
            string newRes;
            foreach (Resolution res in allResolutions)
            {
                newRes = res.width.ToString() + " x " + res.height.ToString();
                if (!resolutionStringList.Contains(newRes))
                {
                    resolutionStringList.Add(newRes);
                    selectedResolutionList.Add(res);
                }
            }

            resolutionDropdown.AddOptions(resolutionStringList);
        }

        private void LoadSavedResolution()
        {
            if (PlayerPrefs.HasKey("ResolutionIndex"))
            {
                selectedResolution = PlayerPrefs.GetInt("ResolutionIndex");
                resolutionDropdown.value = selectedResolution;
                resolutionDropdown.RefreshShownValue();

                Resolution res = selectedResolutionList[selectedResolution];
                Screen.SetResolution(res.width, res.height, isFullScreen);
            }
        }

        private void LoadSavedFullscreen()
        {
            if (PlayerPrefs.HasKey("Fullscreen"))
            {
                isFullScreen = PlayerPrefs.GetInt("Fullscreen") == 1;
                fullscreenDropdown.value = isFullScreen ? 0 : 1;
                fullscreenDropdown.RefreshShownValue();
            }
            else
            {
                // Default to fullscreen
                isFullScreen = true;
                fullscreenDropdown.value = 0;
            }
        }

        private void LoadSavedVolume()
        {
            if (PlayerPrefs.HasKey(MasterVolume))
            {
                float savedVolume = PlayerPrefs.GetFloat(MasterVolume);
                volumeSlider.value = savedVolume;
                ChangeVolume(savedVolume);
            }
        }

        /// <summary>
        /// Loading mouse sensitiivty is up to the user. If you're on a multiplayer
        /// project, apply the loading mechanic somewhere else (Like the player script).
        /// 
        /// If you're on a singleplayer project you can apply the logic here, but now this
        /// serves no purpose for expandability. 
        /// </summary>
        private void LoadSavedMouseSensitivity()
        {
            if (PlayerPrefs.HasKey(MouseSensitivity))
            {
                float sensitivity = PlayerPrefs.GetFloat(MouseSensitivity);
                mouseSensitivitySlider.value = sensitivity;
                // PlayerController.Instance.SetMouseSensitivity(sensitivity); -- example
            }
        }

        public void ChangeResolution()
        {
            selectedResolution = resolutionDropdown.value;
            Resolution res = selectedResolutionList[selectedResolution];

            Screen.SetResolution(res.width, res.height, isFullScreen);

            // PlayerPref saving
            PlayerPrefs.SetInt(ResolutionIndex, selectedResolution);
        }

        public void ChangeFullscreen()
        {
            // 0 = Fullscreen, 1 = Windowed
            isFullScreen = fullscreenDropdown.value == 0;

            // PlayerPref saving
            PlayerPrefs.SetInt(Fullscreen, isFullScreen ? 1 : 0);

            Resolution res = selectedResolutionList[selectedResolution];
            Screen.SetResolution(res.width, res.height, isFullScreen);
        }

        public void ChangeVolume(float volumeDb)
        {
            audioMixer.SetFloat(MasterVolume, volumeDb);
            PlayerPrefs.SetFloat(MasterVolume, volumeDb);
        }

        public void ChangeMouseSensitivity(float value)
        {
            // PlayerPref saving
            PlayerPrefs.SetFloat(MouseSensitivity, value);
        }
    }
}