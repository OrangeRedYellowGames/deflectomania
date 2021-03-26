using FSM;
using UnityEngine;

namespace Player.Scripts.States.Ground {
    public class IdleState : BaseGroundState {
        public IdleState(PlayerController controller, StateMachine stateMachine) : base(controller, stateMachine) {
        }

        public override void Enter() {
            base.Enter();
        }

        public override void LogicUpdate() {
            base.LogicUpdate();
            if (HorizontalInput != 0) {
                StateMachine.ChangeState(Controller.RunningState);
            }
        }
    }
}