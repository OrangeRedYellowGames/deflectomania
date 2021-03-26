using FSM;
using UnityEngine;

namespace Player.Scripts.States {
    public abstract class PlayerMovementState : State {
        public readonly PlayerController Controller;
        
        // static variables so that they're shared by each object
        protected static float HorizontalInput;
        protected static bool VerticalInput;
        protected static Vector2 NewVelocity;

        public PlayerMovementState(PlayerController controller, StateMachine stateMachine) : base(stateMachine) {
            Controller = controller;
        }

        public override void Enter() {
            base.Enter();
            Debug.Log($"Inside {StateName}");
            // HorizontalInput = 0.0f;
            // _verticalInput = false;
        }


        public override void HandleInput() {
            base.HandleInput();
            HorizontalInput = Input.GetAxis("Horizontal");
            VerticalInput = Input.GetButton("Jump");
        }

        public override void LogicUpdate() {
            if (HorizontalInput > 0) {
                Controller.ChangeObjectDirection(PlayerDirection.Right);
            }
            else if (HorizontalInput < 0) {
                Controller.ChangeObjectDirection(PlayerDirection.Left);
            }
        }

        public override void PhysicsUpdate() {
            base.PhysicsUpdate();
            Controller.Move(NewVelocity * Time.fixedDeltaTime);
        }
    }
}