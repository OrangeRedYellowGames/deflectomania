using UnityEngine;

namespace FSM.Movement.States.Air {
    public class FallState : AbstractAirState {
        public override void Enter() {
            base.Enter();
            // To handle cases where the player hits a platform above him
            MovementFSM.newVelocity.Value = new Vector2(MovementFSM.newVelocity.Value.x, 0);
        }

        public override void LogicUpdate() {
            base.LogicUpdate();
            if (Motor.IsGrounded) {
                if (MovementFSM.horizontalInput.Value == 0.0f) {
                    MovementFSM.ChangeState(MovementFSM.IdleState);
                }
                else {
                    MovementFSM.ChangeState(MovementFSM.RunningState);
                }
            }
        }
    }
}