using System;
using NLog;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using Logger = NLog.Logger;

namespace Entities.Player.Scripts {
    public class DeathScript : MonoBehaviour {
        /// <summary>
        /// DeathEvent to listen to to check if the attached player has died
        /// </summary>
        public IntEvent deathEvent;

        /// <summary>
        /// Player ID that will be sent in the death event.
        /// </summary>
        public IntReference playerId;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private GameObject _player;

        /// <summary>
        /// Checks if it's attached to a player and if the deathEvent exists.
        /// </summary>
        private void Awake() {
            try {
                _player = transform.gameObject;
            }
            catch (Exception) {
                Logger.Error("An error occured when trying to find the parenting Player object of the GunPivot");
                throw;
            }

            Assert.IsTrue(_player.CompareTag("Player"));
            Assert.IsNotNull(deathEvent);
            Assert.IsNotNull(playerId);

            deathEvent.Register(Die);
        }

        private void OnDestroy() {
            deathEvent.Unregister(Die);
        }

        /// <summary>
        /// Function that gets called whenever the DeathEvent is triggered.
        /// </summary>
        private void Die(int deadPlayerId) {
            if (playerId.Value == deadPlayerId) {
                _player.SetActive(false);
            }
        }
    }
}