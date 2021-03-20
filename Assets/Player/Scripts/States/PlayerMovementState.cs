using FSM;
using UnityEngine;

namespace Player.Scripts.States {
    public class PlayerMovementState : State {
        public readonly PlayerController Controller;

        protected float HorizontalInput;
        protected bool _verticalInput;
        protected Vector2 NewVelocity;

        public PlayerMovementState(PlayerController controller, StateMachine stateMachine) : base(stateMachine) {
            Controller = controller;
        }

        public override void Enter() {
            base.Enter();
            Debug.Log($"Inside {StateName}");
            HorizontalInput = 0.0f;
            _verticalInput = false;
        }


        public override void HandleInput() {
            base.HandleInput();
            HorizontalInput = Input.GetAxis("Horizontal");
            _verticalInput = Input.GetButtonDown("Jump");
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

            // apply gravity before moving
            // TODO move to base jumping / falling state
            NewVelocity.y += Controller.gravity * Time.deltaTime;
            Controller.Move(NewVelocity * Time.deltaTime);
        }
    }
}