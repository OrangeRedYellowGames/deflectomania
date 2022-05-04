using FSM.Abstract;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Serialization;

namespace FSM.Movement.States.Air {
    public abstract class AbstractAirState : AbstractMovementState {
        [SerializeField] public IntConstant gravity;
        [SerializeField] public IntConstant maxAirSpeed;
        [SerializeField] public IntConstant airFriction;


        public override void PhysicsUpdate() {
            base.PhysicsUpdate();
            // Add gravity to new velocity
            var horizontalValue = Mathf.Lerp(Motor.velocity.x, horizontalInput.Value * maxAirSpeed.Value,
                Time.fixedDeltaTime * airFriction.Value);

            // TODO: Calculate max velocity due to free fall and use drag to limit gravity affect
            var verticalValue = NewVelocity.Value.y + (gravity.Value * Time.fixedDeltaTime);
            if (verticalValue < gravity.Value) {
                verticalValue = gravity.Value;
            }

            NewVelocity.Value = new Vector2(horizontalValue, verticalValue);
        }
    }
}