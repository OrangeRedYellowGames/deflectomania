using FSM;

namespace Player.Scripts.States.Air {
    public class FallState : BaseAirState {
        public FallState(PlayerController controller, StateMachine stateMachine) : base(controller, stateMachine) {
        }

        public override void Enter() {
            base.Enter();
            // To handle cases where the player hits a platform above him
            NewVelocity.y = 0;
        }

        public override void LogicUpdate() {
            base.LogicUpdate();
            if (Controller.IsGrounded) {
                if (HorizontalInput == 0) {
                    StateMachine.ChangeState(Controller.idleState);
                }
                else {
                    StateMachine.ChangeState(Controller.RunningState);
                }
            }
        }
    }
}