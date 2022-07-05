using FSM.Abstract;
using UnityEngine;

namespace FSM.Movement.States.Air {
    public abstract class AbstractAirState : AbstractMovementState {
        public override void PhysicsUpdate() {
            base.PhysicsUpdate();
            // Add gravity to new velocity
            var horizontalValue = Mathf.Lerp(Motor.velocity.x,
                MovementFSM.horizontalInput.Value * MovementFSM.maxAirSpeed.Value,
                Time.fixedDeltaTime * MovementFSM.airFriction.Value);

            // TODO: Calculate max velocity due to free fall and use drag to limit gravity affect
            var verticalValue = MovementFSM.newVelocity.Value.y + (MovementFSM.gravity.Value * Time.fixedDeltaTime);
            if (verticalValue < MovementFSM.gravity.Value) {
                verticalValue = MovementFSM.gravity.Value;
            }

            MovementFSM.newVelocity.Value = new Vector2(horizontalValue, verticalValue);
        }
    }
}