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

        // Vector Directions, used to change direction in ChangeObjectDirection
        private static readonly Vector3 RightRotationVector = new Vector3(0f, 0f, 0f);
        private static readonly Vector3 LeftRotationVector = new Vector3(0f, 180f, 0f);

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
            switch (playerDirection) {
                case PlayerDirection.Right:
                    transform.rotation = Quaternion.Euler(RightRotationVector);
                    break;
                case PlayerDirection.Left:
                    transform.rotation = Quaternion.Euler(LeftRotationVector);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(playerDirection), playerDirection, null);
            }
        }
    }
}