using System.Collections;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class RestartSystem : MonoBehaviour {
    public IntEvent playerWinEvent;
    public float restartDelay = 1f;
    public Animator transition;

    private void Awake() {
        Assert.IsNotNull(playerWinEvent);

        playerWinEvent.Register(OnWinEvent);
    }

    private void OnDestroy() {
        playerWinEvent.Unregister(OnWinEvent);
    }

    private void OnWinEvent(int playerIdx) {
        StartCoroutine(LoadLevel(restartDelay));
    }

    private void RestartLevel() {
        var nextSceneIndex = Random.Range(1, SceneManager.sceneCountInBuildSettings);
        SceneManager.LoadScene(nextSceneIndex);
    }

    private IEnumerator LoadLevel(float delay) {
        yield return new WaitForSeconds(delay);
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(0.5f);

        RestartLevel();
    }
}