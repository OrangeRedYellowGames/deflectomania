using System.Collections.Generic;
using FSM.Abstract;
using FSM.Movement.States.Air;
using FSM.Movement.States.Ground;
using Player.Scripts;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace FSM.Movement {
    [RequireComponent(typeof(MovementMotor2D))]
    public class MovementStateMachine : AbstractFiniteStateMachine {
        private MovementMotor2D _motor;

        // TODO: Refactor these states into their own class?
        // Player Inputs
        // static variables so that they're shared by each object
        [Header("Player Inputs")]
        [SerializeField]
        public FloatReference horizontalInput;

        [SerializeField] public BoolReference verticalInput;
        [SerializeField] public Vector2Reference newVelocity;

        // Movement variables. Would be better if these were on the server and not have the ability for the client to change theses
        [Header("Running Variables")]
        [SerializeField]
        public IntConstant maxSpeed;

        [SerializeField] public IntConstant accelerationFactor;
        [SerializeField] public IntConstant frictionFactor;

        [Header("Air Variables")]
        [SerializeField]
        public IntConstant gravity;

        [SerializeField] public IntConstant maxAirSpeed;
        [SerializeField] public IntConstant airFriction;

        [Header("Jump Variables")]
        [SerializeField]
        public FloatConstant minJumpHeight;

        public float maxJumpHeight = 4.5f;


        // Movement States
        public IdleState IdleState;
        public RunningState RunningState;
        public JumpState JumpState;
        public FallState FallState;


        public void Awake() {
            _motor = GetComponent<MovementMotor2D>();

            IdleState = new IdleState();
            RunningState = new RunningState();
            JumpState = new JumpState();
            FallState = new FallState();

            // Loop over each state and set the FSM, needed so that states are able to do transitions
            var stateList = new List<AbstractMovementState> {
                IdleState, RunningState, JumpState, FallState
            };

            // Set the FSM and Motor variables for each movement state
            foreach (var state in stateList) {
                state.SetFSM(this);
                state.SetMotor(_motor);
            }

            // Set starting state to idle
            CurrentState = IdleState;

            CurrentState.Enter();
        }
    }
}