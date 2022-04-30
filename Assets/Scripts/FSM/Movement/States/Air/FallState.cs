using UnityEngine;

namespace FSM.Movement.States.Air {
    [CreateAssetMenu(fileName = "FallState", menuName = "FSM/States/Falling", order = 3)]
    public class FallState : AbstractAirState {
        public override void Enter() {
            base.Enter();
            // To handle cases where the player hits a platform above him
            NewVelocity.y = 0;
        }

        public override void LogicUpdate() {
            base.LogicUpdate();
            if (Motor.IsGrounded) {
                if (HorizontalInput.Value == 0.0f) {
                    MovementFSM.ChangeState(MovementFSM.idleState);
                }
                else {
                    MovementFSM.ChangeState(MovementFSM.runningState);
                }
            }
        }
    }
}