using UnityEngine;

namespace FSM.Movement.States.Air {
    public abstract class AbstractAirState : AbstractMovementState {
        public const float Gravity = -20f;
        public const bool hasAirControl = true;
        public const float MaxAirSpeed = 10f;
        public const float airFriction = 50f;


        public override void PhysicsUpdate() {
            base.PhysicsUpdate();
            // Add gravity to new velocity
            NewVelocity.x = Mathf.Lerp(Motor.velocity.x, HorizontalInput * MaxAirSpeed,
                Time.fixedDeltaTime * airFriction);

            // TODO: Calculate max velocity due to free fall and use drag to limit gravity affect
            NewVelocity.y += Gravity * Time.fixedDeltaTime;
            if (NewVelocity.y < Gravity) {
                NewVelocity.y = Gravity;
            }
        }
    }
}