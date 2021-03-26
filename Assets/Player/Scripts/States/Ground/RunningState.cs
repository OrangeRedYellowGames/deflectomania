using FSM;
using UnityEngine;

namespace Player.Scripts.States.Ground {
    public class RunningState : BaseGroundState {
        public float maxSpeed = 10f;
        public float accelerationFactor = 20f;
        public float frictionFactor = 50f;

        public RunningState(PlayerController controller, StateMachine stateMachine) : base(controller, stateMachine) {
        }

        // public override void Enter() {
        //     base.Enter();
        // }

        public override void LogicUpdate() {
            base.LogicUpdate();
            if (HorizontalInput == 0 && Mathf.Abs(Controller.velocity.x) < 0.1) {
                StateMachine.ChangeState(Controller.IdleState);
            }
        }

        public override void PhysicsUpdate() {
            base.PhysicsUpdate();
            var factor = HorizontalInput == 0 ? frictionFactor : accelerationFactor;

            // Replace with smoothDamp
            NewVelocity.x = Mathf.Lerp(Controller.velocity.x, HorizontalInput * maxSpeed, Time.fixedDeltaTime * factor);
        }
    }
}