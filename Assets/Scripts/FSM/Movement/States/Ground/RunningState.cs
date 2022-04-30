using UnityEngine;

namespace FSM.Movement.States.Ground {
    [CreateAssetMenu(fileName = "RunningState", menuName = "FSM/States/Running", order = 2)]
    public class RunningState : AbstractGroundState {
        public float maxSpeed = 10f;
        public float accelerationFactor = 20f;
        public float frictionFactor = 50f;


        public override void LogicUpdate() {
            base.LogicUpdate();
            if (HorizontalInput.Value == 0 && Mathf.Abs(Motor.velocity.x) < 0.1) {
                MovementFSM.ChangeState(MovementFSM.idleState);
            }
        }

        public override void PhysicsUpdate() {
            base.PhysicsUpdate();
            var factor = HorizontalInput.Value == 0 ? frictionFactor : accelerationFactor;

            // Replace with smoothDamp
            NewVelocity.x = Mathf.Lerp(Motor.velocity.x, HorizontalInput.Value * maxSpeed,
                Time.fixedDeltaTime * factor);
        }
    }
}