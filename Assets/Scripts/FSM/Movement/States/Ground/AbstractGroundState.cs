using FSM.Abstract;
using UnityEngine;

namespace FSM.Movement.States.Ground {
    public abstract class AbstractGroundState : AbstractMovementState {
        public override void Enter() {
            base.Enter();
            // To fix controller freaking out trying to figure out if player is grounded.
            // https://github.com/prime31/CharacterController2D/issues/106
            MovementFSM.newVelocity.Value = new Vector2(MovementFSM.newVelocity.Value.x, -0.01f);
        }

        public override void LogicUpdate() {
            base.LogicUpdate();

            // If jump is pressed and we're on the ground
            if (MovementFSM.verticalInput.Value && Motor.IsGrounded) {
                MovementFSM.ChangeState(MovementFSM.JumpState);
            }

            // If we're suddenly not grounded, we're falling
            else if (!Motor.IsGrounded) {
                MovementFSM.ChangeState(MovementFSM.FallState);
            }
        }
    }
}