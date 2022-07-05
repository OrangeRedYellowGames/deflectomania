using UnityEngine;

namespace FSM.Movement.States.Air {
    public class JumpState : AbstractAirState {
        // Jump height in unity units / cells, how many blocks
        // TODO: Code higher jump if space bar is held

        public override void Enter() {
            base.Enter();
            MovementFSM.newVelocity.Value =
                new Vector2(MovementFSM.newVelocity.Value.x,
                    Mathf.Sqrt(2f * MovementFSM.minJumpHeight.Value * -MovementFSM.gravity.Value));
        }

        public override void LogicUpdate() {
            base.LogicUpdate();
            // If velocity reaches 0 or if player hits a platform above him
            if (Motor.velocity.y <= 0 || Motor.collisionState.Above) {
                MovementFSM.ChangeState(MovementFSM.FallState);
            }
        }
    }
}