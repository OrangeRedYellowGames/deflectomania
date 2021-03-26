using FSM;
using UnityEngine;

namespace Player.Scripts.States.Air {
    public class JumpState : BaseAirState {
        // Jump height in unity units / cells, how many blocks
        // TODO: Code higher jump if space bar is held
        public float minJumpHeight = 3.5f;
        public float maxJumpHeight = 4.5f;

        public JumpState(PlayerController controller, StateMachine stateMachine) : base(controller, stateMachine) {
        }

        public override void Enter() {
            base.Enter();
            NewVelocity.y = Mathf.Sqrt(2f * minJumpHeight * -Gravity);
        }

        public override void LogicUpdate() {
            base.LogicUpdate();
            // If velocity reaches 0 or if player hits a platform above him
            if (Controller.velocity.y <= 0 || Controller.collisionState.Above) {
                StateMachine.ChangeState(Controller.FallState);
            }
        }
    }
}