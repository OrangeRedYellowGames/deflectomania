using FSM;
using UnityEngine;

namespace Player.Scripts.States {
    public abstract class PlayerMovementState : State {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public readonly PlayerController Controller;

        // static variables so that they're shared by each object
        protected static float HorizontalInput;
        protected static bool VerticalInput;
        protected static Vector2 NewVelocity;
        protected static Vector2 MouseScreenPosition;

        public PlayerMovementState(PlayerController controller, StateMachine stateMachine) : base(stateMachine) {
            Controller = controller;
        }

        public override void Enter() {
            base.Enter();
            Logger.Info($"Inside {StateName}");
        }


        public override void HandleInput() {
            base.HandleInput();
            HorizontalInput = Input.GetAxis("Horizontal");
            VerticalInput = Input.GetButton("Jump");
            MouseScreenPosition = Input.mousePosition;
        }

        public override void LogicUpdate() {
            var mouseWorldPosition = PlayerController.MainCamera.ScreenToWorldPoint(MouseScreenPosition);

            // Default will be right
            if (mouseWorldPosition.x < Controller.transform.position.x) {
                Controller.ChangeObjectDirection(PlayerDirection.Left);
            }
            else {
                Controller.ChangeObjectDirection(PlayerDirection.Right);
            }
        }

        public override void PhysicsUpdate() {
            base.PhysicsUpdate();
            Controller.Move(NewVelocity * Time.fixedDeltaTime);
        }
    }
}