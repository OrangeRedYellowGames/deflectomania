using FSM.Abstract;
using UnityEngine;

namespace FSM.Shooting.States {
    public class CooldownState : AbstractShootingState {
        public override void Enter() {
            base.Enter();
            ShootingFSM.remainingCooldownSeconds = ShootingFSM.cooldownSeconds;
        }

        public override void LogicUpdate() {
            ShootingFSM.remainingCooldownSeconds -= Time.deltaTime;
            if (ShootingFSM.remainingCooldownSeconds <= 0) {
                ShootingFSM.ChangeState(ShootingFSM.ShootingState);
            }
        }
    }
}