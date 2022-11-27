using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Assertions;

public class RestartSystem : MonoBehaviour {
    public IntEvent playerWinEvent;
    public IntValueList alivePlayers;
    public IntValueList currentPlayers;

    // Start is called before the first frame update
    private void Awake() {
        Assert.IsNotNull(playerWinEvent);
        Assert.IsNotNull(alivePlayers);
        Assert.IsNotNull(currentPlayers);

        playerWinEvent.Register(Restart);
    }

    // Update is called once per frame
    private void Restart(int playerIdx) {
        Debug.Log("Restart");
        alivePlayers.Clear();
        currentPlayers.Clear();
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}