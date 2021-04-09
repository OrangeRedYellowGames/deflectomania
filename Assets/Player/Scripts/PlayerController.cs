using System;
using FSM;
using Player.Scripts.States.Air;
using Player.Scripts.States.Ground;
using UnityEngine;

namespace Player.Scripts {
    public enum PlayerDirection {
        Left,
        Right
    }

    public class PlayerController : MovementMotor2D {
        // State Machines
        // ReSharper disable once InconsistentNaming
        private StateMachine _movementSM;

        public IdleState IdleState;
        public RunningState RunningState;
        public JumpState JumpState;
        public FallState FallState;

        // Caches Camera.main to optimize calls to it in FSM
        // Adapted from https://forum.unity.com/threads/how-to-cache-the-main-camera-as-a-global-variable.853774/
        private static Camera _mainCamera;

        public static Camera MainCamera {
            get {
                if (!_mainCamera) {
                    _mainCamera = Camera.main;
                }

                return _mainCamera;
            }
        }

        // MonoBehaviour
        private void Start() {
            _movementSM = new StateMachine();

            IdleState = new IdleState(this, _movementSM);
            RunningState = new RunningState(this, _movementSM);
            JumpState = new JumpState(this, _movementSM);
            FallState = new FallState(this, _movementSM);

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