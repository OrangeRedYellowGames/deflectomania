using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class RestartSystem : MonoBehaviour {
    public IntEvent playerWinEvent;

    private void Awake() {
        Assert.IsNotNull(playerWinEvent);
        playerWinEvent.Register(RestartGame);
    }

    private void OnDestroy() {
        playerWinEvent.Unregister(RestartGame);
    }

    private void RestartGame(int winnerIndex) {
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }
}