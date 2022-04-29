namespace FSM.Movement.States.Ground {
    public abstract class AbstractGroundState : AbstractMovementState {
        public override void Enter() {
            base.Enter();
            // To fix controller freaking out trying to figure out if player is grounded.
            // https://github.com/prime31/CharacterController2D/issues/106
            NewVelocity.y = -0.01f;
        }

        public override void LogicUpdate() {
            base.LogicUpdate();

            // If jump is pressed and we're on the ground
            if (VerticalInput && Motor.IsGrounded) {
                MovementFSM.ChangeState(MovementFSM.jumpState);
            }

            // If we're suddenly not grounded, we're falling
            else if (!Motor.IsGrounded) {
                MovementFSM.ChangeState(MovementFSM.fallState);
            }
        }
    }
}