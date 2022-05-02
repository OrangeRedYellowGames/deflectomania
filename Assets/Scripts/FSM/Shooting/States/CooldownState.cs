using System.Collections;
using FSM.Abstract;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace FSM.Shooting.States {
    [CreateAssetMenu(fileName = "CooldownState", menuName = "FSM/Shooting/States/Cooldown", order = 2)]
    public class CooldownState : AbstractShootingState {
        public float remainingCooldownSeconds;
        public float cooldownSeconds;

        public override void Enter() {
            base.Enter();
            remainingCooldownSeconds = cooldownSeconds;
        }

        public override void LogicUpdate() {
            remainingCooldownSeconds -= Time.deltaTime;
            if (remainingCooldownSeconds <= 0) {
                ShootingFSM.ChangeState(ShootingFSM.shootingState);
            }
        }
    }
}