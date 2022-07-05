using UnityEngine;

namespace FSM.Movement.States.Ground {
    public class RunningState : AbstractGroundState {
        public override void LogicUpdate() {
            base.LogicUpdate();
            if (MovementFSM.horizontalInput.Value == 0 && Mathf.Abs(Motor.velocity.x) < 0.1) {
                MovementFSM.ChangeState(MovementFSM.IdleState);
            }
        }

        public override void PhysicsUpdate() {
            base.PhysicsUpdate();
            var factor = MovementFSM.horizontalInput.Value == 0 ? MovementFSM.frictionFactor.Value : MovementFSM.accelerationFactor.Value;

            // Replace with smoothDamp
            var horizontalValue = Mathf.Lerp(Motor.velocity.x, MovementFSM.horizontalInput.Value * MovementFSM.maxSpeed.Value,
                Time.fixedDeltaTime * factor);

            MovementFSM.newVelocity.Value = new Vector2(horizontalValue, MovementFSM.newVelocity.Value.y);
        }
    }
}