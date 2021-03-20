using System;
using FSM;
using Player.Scripts.States.Ground;
using UnityEngine;

namespace Player.Scripts {
    public enum PlayerDirection {
        Left,
        Right
    }

    public class PlayerController : MovementMotor2D {
        public float gravity = -9.81f;

        // State Machines
        // ReSharper disable once InconsistentNaming
        private StateMachine _movementSM;

        public IdleState IdleState;
        public RunningState RunningState;


        // MonoBehaviour
        private void Start() {
            _movementSM = new StateMachine();

            IdleState = new IdleState(this, _movementSM);
            RunningState = new RunningState(this, _movementSM);

            _movementSM.Initialize(IdleState);
        }

        // Update is called once per frame
        private void Update() {
            _movementSM.CurrentState.HandleInput();

            _movementSM.CurrentState.LogicUpdate();
        }

        private void FixedUpdate() {
            _movementSM.CurrentState.PhysicsUpdate();
        }

        // Methods
        public void ChangeObjectDirection(PlayerDirection playerDirection) {
            var localScale = transform.localScale;

            switch (playerDirection) {
                case PlayerDirection.Right:
                    localScale = new Vector3(1, localScale.y, localScale.z);
                    transform.localScale = localScale;
                    break;
                case PlayerDirection.Left:
                    localScale = new Vector3(-1, localScale.y, localScale.z);
                    transform.localScale = localScale;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(playerDirection), playerDirection, null);
            }
        }
    }
}