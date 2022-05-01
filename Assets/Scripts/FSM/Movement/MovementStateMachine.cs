using System.Collections.Generic;
using FSM.Abstract;
using FSM.Movement.States.Air;
using FSM.Movement.States.Ground;
using Player.Scripts;
using UnityEngine;

namespace FSM.Movement {
    [RequireComponent(typeof(MovementMotor2D))]
    public class MovementStateMachine : AbstractFiniteStateMachine {
        private MovementMotor2D _motor;

        public IdleState idleState;
        public RunningState runningState;
        public JumpState jumpState;
        public FallState fallState;

        public void Awake() {
            _motor = GetComponent<MovementMotor2D>();

            // Loop over each state and set the FSM, needed so that states are able to do transitions
            var stateList = new List<AbstractMovementState> {
                idleState, runningState, jumpState, fallState
            };

            // Set the FSM and Motor variables for each movement state
            foreach (var state in stateList) {
                state.SetFSM(this);
                state.SetMotor(_motor);
            }

            // Set starting state to idle
            CurrentState = idleState;

            CurrentState.Enter();
        }
    }
}