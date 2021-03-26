using FSM;

namespace Player.Scripts.States.Ground {
    public abstract class BaseGroundState : PlayerMovementState {
        public BaseGroundState(PlayerController controller, StateMachine stateMachine) :
            base(controller, stateMachine) {
        }

        public override void Enter() {
            base.Enter();
            // To fix controller freaking out trying to figure out if player is grounded.
            // https://github.com/prime31/CharacterController2D/issues/106
            NewVelocity.y = -0.01f;
        }

        public override void LogicUpdate() {
            base.LogicUpdate();

            // If jump is pressed and we're on the ground
            if (VerticalInput && Controller.IsGrounded) {
                StateMachine.ChangeState(Controller.JumpState);
            }

            // If we're suddenly not grounded, we're falling
            else if (!Controller.IsGrounded) {
                StateMachine.ChangeState(Controller.FallState);
            }
        }
    }
}