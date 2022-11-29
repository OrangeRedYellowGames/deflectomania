using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]
public class PlayerInputManagerToUnityAtom : MonoBehaviour {
    public IntValueList currentPlayers;
    public IntValueList alivePlayers;
    public List<Transform> spawnPoints;
    private AudioSource _deathSound;

    private PlayerInputManager _playerInputManager;

    /// <summary>
    ///     On awake it asserts that all the required atom lists are available and cleared.
    /// </summary>
    private void Awake() {
        _playerInputManager = GetComponent<PlayerInputManager>();
        _deathSound = GetComponent<AudioSource>();

        Assert.IsNotNull(_playerInputManager);
        Assert.IsNotNull(currentPlayers);
        Assert.IsNotNull(alivePlayers);
        Assert.IsNotNull(spawnPoints);
        Assert.AreEqual(spawnPoints.Count, 4);
        Assert.IsNotNull(_deathSound);

        currentPlayers.Clear();
        alivePlayers.Clear();
    }

    /// <summary>
    ///     On player joining it checks if that player is already in, it disallows it to rejoin and deletes the object.
    ///     Otherwise it creates the player and adds it to the list of alive and current players.
    /// </summary>
    /// <param name="pi"></param>
    public void OnPlayerJoined(PlayerInput pi) {
        if (!currentPlayers.Contains(pi.playerIndex)) {
            pi.transform.position = spawnPoints[pi.playerIndex].position;
            currentPlayers.Add(pi.playerIndex);
            alivePlayers.Add(pi.playerIndex);
        }
        else {
            pi.gameObject.SetActive(false);
        }
    }

    /// <summary>
    ///     On player left it removes the player from the alive list.
    /// </summary>
    /// <param name="pi"></param>
    public void OnPlayerLeft(PlayerInput pi) {
        if (alivePlayers.Contains(pi.playerIndex)) {
            alivePlayers.Remove(pi.playerIndex);
            _deathSound.Play();
        }
    }
}