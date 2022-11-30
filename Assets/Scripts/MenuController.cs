using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
///     Adapted from https://www.youtube.com/watch?v=Cq_Nnw_LwnI
/// </summary>
public class MenuController : MonoBehaviour {
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    private AudioSource _clickSound;
    private Resolution[] _resolutions;

    private void Awake() {
        _clickSound = GetComponent<AudioSource>();
        Assert.IsNotNull(_clickSound);
    }

    private void Start() {
        fullscreenToggle.isOn = Screen.fullScreen;
        _resolutions = Screen.resolutions;
        var resolutionOptions = new List<string>();

        // Tuple to hold the current resolution
        (int, Resolution) currentResolution = (0, Screen.currentResolution);

        for (var i = 0; i < _resolutions.Length; i++) {
            var res = _resolutions[i];
            resolutionOptions.Add(res.ToString());
            if (res.width == currentResolution.Item2.width && res.height == currentResolution.Item2.height &&
                res.refreshRate == currentResolution.Item2.refreshRate)
                currentResolution.Item1 = i;
        }

        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResolution.Item1;
    }

    private IEnumerator StartGame() {
        _clickSound.Play();
        yield return new WaitWhile(() => _clickSound.isPlaying);
        var nextSceneIndex = Random.Range(1, SceneManager.sceneCountInBuildSettings);
        SceneManager.LoadScene(nextSceneIndex);
    }

    public void OnPlayOnlinePressed() {
        StartCoroutine(StartGame());
    }

    public void OnSetResolution(int resolutionIndex) {
        var resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, resolution.refreshRate);
    }

    public void OnSetFullscreen(bool isFullscreen) {
        // Adapted from https://answers.unity.com/questions/1510332/why-my-screenfullscreen-isnt-working.html
        Screen.fullScreen = isFullscreen;
        Screen.fullScreenMode = isFullscreen ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed;
    }

    public void OnExitPressed() {
        _clickSound.Play();
        Application.Quit();
    }
}