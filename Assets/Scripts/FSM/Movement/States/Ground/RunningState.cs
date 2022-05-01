using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace FSM.Movement.States.Ground {
    [CreateAssetMenu(fileName = "RunningState", menuName = "FSM/States/Running", order = 2)]
    public class RunningState : AbstractGroundState {
        [SerializeField] public IntConstant maxSpeed;
        [SerializeField] public IntConstant accelerationFactor;
        [SerializeField] public IntConstant frictionFactor;


        public override void LogicUpdate() {
            base.LogicUpdate();
            if (horizontalInput.Value == 0 && Mathf.Abs(Motor.velocity.x) < 0.1) {
                MovementFSM.ChangeState(MovementFSM.idleState);
            }
        }

        public override void PhysicsUpdate() {
            base.PhysicsUpdate();
            var factor = horizontalInput.Value == 0 ? frictionFactor.Value : accelerationFactor.Value;

            // Replace with smoothDamp
            var horizontalValue = Mathf.Lerp(Motor.velocity.x, horizontalInput.Value * maxSpeed.Value,
                Time.fixedDeltaTime * factor);

            NewVelocity.Value = new Vector2(horizontalValue, NewVelocity.Value.y);
        }
    }
}