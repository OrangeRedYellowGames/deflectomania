using UnityEngine;

namespace FSM.Movement.States.Ground {
    [CreateAssetMenu(fileName = "IdleState", menuName = "FSM/States/Idle", order = 1)]
    public class IdleState : AbstractGroundState {
        void OnJump() {
            Debug.Log("INSIDE IDLE");
        }

        public override void LogicUpdate() {
            base.LogicUpdate();
            if (HorizontalInput != 0) {
                MovementFSM.ChangeState(MovementFSM.runningState);
            }
        }
    }
}