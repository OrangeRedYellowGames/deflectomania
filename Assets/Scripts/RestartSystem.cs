using System;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class RestartSystem : MonoBehaviour {
    public IntEvent playerWinEvent;
    public float restartDelay = 1f;

    private void Awake() {
        Assert.IsNotNull(playerWinEvent);

        playerWinEvent.Register(OnWinEvent);
    }

    private void OnDestroy() {
        playerWinEvent.Unregister(OnWinEvent);
    }

    private void OnWinEvent(int playerIdx) {
        Invoke(nameof(RestartLevel), restartDelay);
    }

    private void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}