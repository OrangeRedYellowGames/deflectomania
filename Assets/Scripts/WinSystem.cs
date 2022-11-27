using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Assertions;

public class WinSystem : MonoBehaviour {
    public IntValueList alivePlayers;
    public IntEvent playerDeathEvent;
    public IntEvent playerWinEvent;
    private bool _recentlyDeceasedPlayer;

    /// <summary>
    ///     On awake it asserts that all the required events are there.
    /// </summary>
    private void Awake() {
        Assert.IsNotNull(alivePlayers);
        Assert.IsNotNull(playerDeathEvent);
        Assert.IsNotNull(playerWinEvent);

        playerDeathEvent.Register(OnPlayerDeath);
    }

    /// <summary>
    ///     On start it invokes a repeating function every 0.5f seconds to check win condition.
    /// </summary>
    private void Start() {
        _recentlyDeceasedPlayer = false;
        // InvokeRepeating(nameof(CheckWinCondition), 0, 0.5f);
    }

    /// <summary>
    ///     This function is unregisters the playerDeathEvent when this object is destroyed.
    /// </summary>
    private void OnDestroy() {
        playerDeathEvent.Unregister(OnPlayerDeath);
    }

    /// <summary>
    ///     Function that gets called every 0.5f seconds to check if there is a win condition. It checks if a player
    ///     has died recently and if yes goes a head and computes the winner if any.
    /// </summary>
    private void CheckWinCondition() {
        if (!_recentlyDeceasedPlayer)
            return;

        if (alivePlayers.Count == 1) {
            Debug.Log("Winner! is Player " + alivePlayers[0]);
            playerWinEvent.Raise(alivePlayers[0]);
        }
        else {
            if (alivePlayers.Count == 0) {
                Debug.Log("Draw!");
                // -1 denotes a draw!
                playerWinEvent.Raise(-1);
            }
        }

        _recentlyDeceasedPlayer = false;
    }

    /// <summary>
    ///     This function sets a flag that a player has died recently.
    /// </summary>
    /// <param name="playerIndex"></param>
    private void OnPlayerDeath(int playerIndex) {
        _recentlyDeceasedPlayer = true;
        Invoke(nameof(CheckWinCondition), 0.5f);
    }
}