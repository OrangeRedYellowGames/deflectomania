using NLog;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Assertions;
using Logger = NLog.Logger;

public class WinSystem : MonoBehaviour {
    public IntValueList alivePlayers;
    public IntEvent playerDeathEvent;
    public IntEvent playerWinEvent;

    /// <summary>
    /// Amount of time after a player has died to start checking for a win condition.
    /// Useful in case two players kill each other but with a small time difference.
    /// </summary>
    public float winDelay = 0.5f;

    private bool _gameOver = false;
    protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

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
        switch (alivePlayers.Count) {
            case 0:
                // -1 denotes a draw!
                playerWinEvent.Raise(-1);
                _gameOver = true;
                break;
            case 1:
                playerWinEvent.Raise(alivePlayers[0]);
                _gameOver = true;
                break;
        }
    }

    /// <summary>
    ///     This function sets a flag that a player has died recently.
    /// </summary>
    /// <param name="playerIndex"></param>
    private void OnPlayerDeath(int playerIndex) {
        // If there's already a winner, don't check for the win condition again.
        // Useful for cases where the player might kill himself after killing everyone.
        if (_gameOver) return;

        // Check the win condition after a certain amount of time. Useful for cases where two people killed each other, causing a draw.
        Invoke(nameof(CheckWinCondition), winDelay);
    }
}