using System;
using System.Collections.Generic;
using FSM.Abstract;
using FSM.Movement.States.Air;
using FSM.Movement.States.Ground;
using Player.Scripts;
using UnityEngine;

namespace FSM.Movement {
    public enum PlayerDirection {
        Left,
        Right
    }

    [RequireComponent(typeof(MovementMotor2D))]
    public class MovementStateMachine : AbstractFiniteStateMachine {
        private MovementMotor2D _motor;

        public IdleState idleState;
        public RunningState runningState;
        public JumpState jumpState;
        public FallState fallState;

        // Vector Directions, used to change direction in ChangeObjectDirection
        private static readonly Vector3 RightRotationVector = new Vector3(0f, 0f, 0f);
        private static readonly Vector3 LeftRotationVector = new Vector3(0f, 180f, 0f);

        public void Awake() {
            _motor = GetComponent<MovementMotor2D>();

            // Loop over each state and set the FSM, needed so that states are able to do transitions
            var stateList = new List<AbstractMovementState> {
                idleState, runningState, jumpState, fallState
            };


            foreach (var state in stateList) {
                state.SetFSM(this);
                state.SetMotor(_motor);
            }

            // Set starting state to idle
            CurrentState = idleState;

            CurrentState.Enter();
        }

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