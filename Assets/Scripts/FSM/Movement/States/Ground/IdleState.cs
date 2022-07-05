namespace FSM.Movement.States.Ground {
    public class IdleState : AbstractGroundState {
        public override void LogicUpdate() {
            base.LogicUpdate();
            if (MovementFSM.horizontalInput.Value != 0) {
                MovementFSM.ChangeState(MovementFSM.RunningState);
            }
        }
    }
}