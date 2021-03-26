using FSM;
using UnityEngine;

namespace Player.Scripts.States.Air {
    public abstract class BaseAirState : PlayerMovementState {
        public const float Gravity = -20f;
        public const bool hasAirControl = true;
        public const float MaxAirSpeed = 10f;
        public const float airFriction = 50f;

        public BaseAirState(PlayerController controller, StateMachine stateMachine) : base(controller, stateMachine) {
        }
        
        public override void PhysicsUpdate() {
            base.PhysicsUpdate();
            // Add gravity to new velocity
            NewVelocity.x = Mathf.Lerp(Controller.velocity.x, HorizontalInput * MaxAirSpeed,
                Time.fixedDeltaTime * airFriction);

            // TODO: Calculate max velocity due to free fall and use drag to limit gravity affect
            NewVelocity.y += Gravity * Time.fixedDeltaTime;
            if (NewVelocity.y < Gravity) {
                NewVelocity.y = Gravity;
            }
        }
    }
}