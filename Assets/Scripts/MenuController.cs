using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Adapted from https://www.youtube.com/watch?v=Cq_Nnw_LwnI
/// </summary>
public class MenuController : MonoBehaviour {
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    private Resolution[] _resolutions;

    private void Start() {
        fullscreenToggle.isOn = Screen.fullScreen;
        _resolutions = Screen.resolutions;
        List<string> resolutionOptions = new List<string>();

        // Tuple to hold the current resolution
        (int, Resolution) currentResolution = (0, Screen.currentResolution);

        for (int i = 0; i < _resolutions.Length; i++) {
            var res = _resolutions[i];
            resolutionOptions.Add(res.ToString());
            if (res.width == currentResolution.Item2.width && res.height == currentResolution.Item2.height &&
                res.refreshRate == currentResolution.Item2.refreshRate) {
                currentResolution.Item1 = i;
            }
        }

        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResolution.Item1;
    }

    public void OnPlayOnlinePressed() {
        SceneManager.LoadScene("Scenes/Level 1");
    }

    public void OnSetResolution(int resolutionIndex) {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, resolution.refreshRate);
    }

    public void OnSetFullscreen(bool isFullscreen) {
        // Adapted from https://answers.unity.com/questions/1510332/why-my-screenfullscreen-isnt-working.html
        Screen.fullScreen = isFullscreen;
        Screen.fullScreenMode = isFullscreen ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed;
    }

    public void OnExitPressed() {
        Application.Quit();
    }
}