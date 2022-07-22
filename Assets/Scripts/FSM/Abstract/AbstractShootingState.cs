using FSM.Shooting;

namespace FSM.Abstract {
    public abstract class AbstractShootingState : AbstractState {
        protected ShootingStateMachine ShootingFSM;

        public void SetFSM(ShootingStateMachine shootingStateMachine) {
            ShootingFSM = shootingStateMachine;
        }


        public override void Enter() {
            base.Enter();
            // This gets called whenever a state change occurs
            if (ShootingFSM == null) {
                Logger.Error($"A valid Shoting FSM was not set on the {StateName} state");
            }
        }
    }
}