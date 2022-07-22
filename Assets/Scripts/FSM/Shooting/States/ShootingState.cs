using FSM.Abstract;

namespace FSM.Shooting.States {
    public class ShootingState : AbstractShootingState {
        public override void LogicUpdate() {
            if (!ShootingFSM.fireInput.Value) return;
            ShootingFSM.shootingEvent.Event.Raise();
            ShootingFSM.ChangeState(ShootingFSM.CooldownState);
        }
    }
}