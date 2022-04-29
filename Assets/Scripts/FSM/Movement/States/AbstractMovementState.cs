using Player.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace FSM.Movement.States {
    /// <summary>
    /// AbstractMovementAbstractState requires a "Movement Motor" to control movement. Needs to be passed both the setMotor
    /// and setFSM functions.
    /// </summary>
    public abstract class AbstractMovementState : AbstractState {
        // Movement Motor, needed to actually move the attached entity
        protected MovementMotor2D Motor { get; private set; }

        protected MovementStateMachine MovementFSM;

        // Player Inputs
        // static variables so that they're shared by each object
        protected static float HorizontalInput;
        protected static bool VerticalInput;
        protected static Vector2 NewVelocity;
        protected static Vector2 MouseScreenPosition;

        void OnJump() {
            Debug.Log("HEREREEE");
        }

        void OnMove() {
            Debug.Log("HEREREEE");
        }

        public void SetFSM(MovementStateMachine movementStateMachine) {
            MovementFSM = movementStateMachine;
        }

        public void SetMotor(MovementMotor2D motor) {
            Motor = motor;
        }

        public override void Enter() {
            base.Enter();
            // This gets called whenever a state change occurs
            if (MovementFSM == null) {
                Logger.Error($"A valid Movement FSM was not set on the {StateName} state");
            }

            if (Motor == null) {
                Logger.Error($"A valid movement motor was not set on {StateName}");
            }
        }


        public override void HandleInput() {
            base.HandleInput();
            // HorizontalInput = Input.GetAxis("Horizontal");
            // VerticalInput = Input.GetButton("Jump");
            MouseScreenPosition = Mouse.current.position.ReadValue();
        }

        public override void LogicUpdate() {
            var mouseWorldPosition = CameraUtils.MainCamera.ScreenToWorldPoint(MouseScreenPosition);

            // Default will be right
            // if (mouseWorldPosition.x < Controller.transform.position.x) {
            //     Controller.ChangeObjectDirection(PlayerDirection.Left);
            // }
            // else {
            //     Controller.ChangeObjectDirection(PlayerDirection.Right);
            // }
        }

        public override void PhysicsUpdate() {
            base.PhysicsUpdate();
            Motor.Move(NewVelocity * Time.fixedDeltaTime);
        }
    }
}