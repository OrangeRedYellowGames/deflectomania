using System.Collections.Generic;
using Player.Scripts;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Assertions;

namespace FSM {
    internal enum MovementStates {
        OnGround,
        Jumping,
        Falling,
    }

    internal enum GroundStates {
        Start,
        Idle,
        Running,
    }

    [RequireComponent(typeof(MovementMotor2D))]
    public class MovementStateMachine : MonoBehaviour {
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
        [Tooltip("Also known as coyote time. \n" +
                 "Indicates how many frames the player is allowed to jump after he's fallen off an edge.")]
        public int forgivenessFrames = 5;

        public FloatConstant minJumpHeight;

        public float maxJumpHeight = 4.5f;

        // Replace these with VerboseStateMachine if you want to debug the states
        [Header("Internal State Machines")]
        [SerializeField]
        private StateMachine<MovementStates> _movementHfsm;

        [SerializeField] private StateMachine<MovementStates, GroundStates, string> _groundFsm;

        /// <summary>
        /// Reference to the movement motor. Used to give movement commands to the attached game object.
        /// </summary>
        private MovementMotor2D _motor;

        /// <summary>
        /// Used to determine the state of the player (whether he was grounded or not) in the last X frames (X refers to the forgivenessFrames variable)
        /// </summary>
        private Queue<bool> _wasGroundedForLastXFrames;

        private AudioSource _jumpSound;

        private void CalculateAirSpeed() {
            // Add gravity to new velocity
            var horizontalValue = Mathf.Lerp(_motor.velocity.x,
                horizontalInput.Value * maxAirSpeed.Value,
                Time.fixedDeltaTime * airFriction.Value);

            // TODO: Calculate max velocity due to free fall and use drag to limit gravity affect
            var verticalValue = newVelocity.Value.y + (gravity.Value * Time.fixedDeltaTime);
            if (verticalValue < gravity.Value) {
                verticalValue = gravity.Value;
            }

            newVelocity.Value = new Vector2(horizontalValue, verticalValue);
        }

        private void SetupGroundFsm() {
            // Define the ground states
            var idleState = new State<GroundStates>(onEnter: (_) => {
                // TODO: Add idle animation here
                newVelocity.Value = new Vector2(newVelocity.Value.x, -0.01f);
            });

            var runningState = new State<GroundStates>(onEnter: (_) => {
                // TODO: Add running animation here
                newVelocity.Value = new Vector2(newVelocity.Value.x, -0.01f);
            }).AddAction("OnFixedUpdate", () => {
                var factor = horizontalInput.Value == 0
                    ? frictionFactor.Value
                    : accelerationFactor.Value;

                // Replace with smoothDamp
                var horizontalValue = Mathf.Lerp(_motor.velocity.x,
                    horizontalInput.Value * maxSpeed.Value,
                    Time.fixedDeltaTime * factor);

                newVelocity.Value = new Vector2(horizontalValue, newVelocity.Value.y);
            });

            // Setting up ground fsm
            _groundFsm.AddState(GroundStates.Start, isGhostState: true);
            _groundFsm.AddState(GroundStates.Idle, idleState);
            _groundFsm.AddState(GroundStates.Running, runningState);

            // Transitions for ground state
            _groundFsm.AddTransition(GroundStates.Start, GroundStates.Idle, (_) => horizontalInput.Value == 0.0f);
            _groundFsm.AddTransition(GroundStates.Start, GroundStates.Running);
            _groundFsm.AddTransition(GroundStates.Running, GroundStates.Idle,
                (_) => horizontalInput.Value == 0 && Mathf.Abs(_motor.velocity.x) < 0.1);
            _groundFsm.AddTransition(GroundStates.Idle, GroundStates.Running, (_) => horizontalInput.Value != 0);

            // Set the starting state for the ground fsm
            _groundFsm.SetStartState(GroundStates.Start);
        }

        private void SetupRootFsm() {
            // Ground FSM
            _movementHfsm.AddState(MovementStates.OnGround, _groundFsm);

            _movementHfsm.AddTransition(MovementStates.OnGround, MovementStates.Jumping,
                (_) => verticalInput.Value && _motor.IsGrounded);
            _movementHfsm.AddTransition(MovementStates.OnGround, MovementStates.Falling, (_) => !_motor.IsGrounded);

            // Jumping State
            var jumpState = new State<MovementStates>(onEnter: (_) => {
                // TODO: Add jumping animation here
                if (!_jumpSound.isPlaying) {
                    _jumpSound.Play();
                }
                newVelocity.Value = new Vector2(newVelocity.Value.x,
                    Mathf.Sqrt(2f * minJumpHeight.Value * -gravity.Value));
            }).AddAction("OnFixedUpdate", CalculateAirSpeed);

            _movementHfsm.AddState(MovementStates.Jumping, jumpState);
            _movementHfsm.AddTransition(MovementStates.Jumping, MovementStates.Falling,
                (_) => _motor.velocity.y <= 0 || _motor.collisionState.Above);
            _movementHfsm.AddTransition(MovementStates.Jumping, MovementStates.OnGround, (_) => _motor.IsGrounded);

            // Falling State
            var fallingState = new State<MovementStates>(onEnter: (_) => {
                // TODO: Add falling animation here
                // To handle cases where the player hits a platform above him
                newVelocity.Value = new Vector2(newVelocity.Value.x, 0);
            }).AddAction("OnFixedUpdate", CalculateAirSpeed);

            _movementHfsm.AddState(MovementStates.Falling, fallingState);
            _movementHfsm.AddTransition(MovementStates.Falling, MovementStates.OnGround, (_) => _motor.IsGrounded);
            _movementHfsm.AddTransition(
                MovementStates.Falling,
                MovementStates.Jumping,
                (_) => verticalInput.Value && _wasGroundedForLastXFrames.Contains(true)
            );

            // This configures the entry point of the state machine
            _movementHfsm.SetStartState(MovementStates.OnGround);
        }

        public void Awake() {
            _motor = GetComponent<MovementMotor2D>();
            _jumpSound = GetComponent<AudioSource>();
            Assert.IsNotNull(_jumpSound);

            _wasGroundedForLastXFrames = new Queue<bool>(forgivenessFrames);
            var isGrounded = _motor.IsGrounded;
            for (var i = 0; i < forgivenessFrames; i++) {
                _wasGroundedForLastXFrames.Enqueue(isGrounded);
            }
        }


        public void Start() {
            // Adapted from https://github.com/Inspiaaa/UnityHFSM/issues/17#issuecomment-1226114238
            _movementHfsm = new StateMachine<MovementStates>();

            // FSM for handling ground states.
            _groundFsm = new StateMachine<MovementStates, GroundStates, string>();

            // Setup movement and on ground fsms
            // Didn't create an FSM for air states due to it adding an extra frame of logic
            // when transitioning from ground to air (relating to detecting if the verticalInput is pressed.
            SetupGroundFsm();
            SetupRootFsm();

            // Initialises the state machine and must be called before OnLogic() is called
            _movementHfsm.Init();
        }

        void Update() {
            _movementHfsm.OnLogic();
        }

        void FixedUpdate() {
            // Update the queue needed for coyote time
            _wasGroundedForLastXFrames.Dequeue();
            _wasGroundedForLastXFrames.Enqueue(_motor.IsGrounded);

            // Trigger OnFixedUpdate action of the current state
            _movementHfsm.OnAction("OnFixedUpdate");

            // Update the motor using the update newVelocity atom
            _motor.Move(newVelocity.Value * Time.fixedDeltaTime);
        }
    }
}