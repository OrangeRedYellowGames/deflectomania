using FSM.Abstract;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace FSM.Shooting.States {
    [CreateAssetMenu(fileName = "ShootingState", menuName = "FSM/Shooting/States/Shooting", order = 1)]
    public class ShootingState : AbstractShootingState {
        [SerializeField] public BoolVariable fireInput;
        [SerializeField] public VoidEvent shootingEvent;

        public override void LogicUpdate() {
            if (!fireInput.Value) return;
            shootingEvent.Raise();
            ShootingFSM.ChangeState(ShootingFSM.cooldownState);
        }
    }
}