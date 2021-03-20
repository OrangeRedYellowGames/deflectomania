using FSM;
using UnityEngine;

namespace Player.Scripts.States.Ground {
    public class RunningState : PlayerMovementState {
        public RunningState(PlayerController controller, StateMachine stateMachine) : base(controller, stateMachine) {
        }

        // public override void Enter() {
        //     base.Enter();
        // }

        public override void LogicUpdate() {
            base.LogicUpdate();
            if (HorizontalInput == 0 && Mathf.Abs(Controller.velocity.x) < 0.5) {
                StateMachine.ChangeState(Controller.IdleState);
            }
        }

        public override void PhysicsUpdate() {
            base.PhysicsUpdate();
            var runSpeed = 10f;
            var smoothedMovementFactor = 20f;

            // Replace with smoothDamp
            NewVelocity.x = Mathf.Lerp(Controller.velocity.x, this.HorizontalInput * runSpeed,
                Time.deltaTime * smoothedMovementFactor);
        }
    }
}