using FSM.Movement;
using Player.Scripts;
using UnityEngine;

namespace FSM.Abstract {
    /// <summary>
    /// AbstractMovementAbstractState requires a "Movement Motor" to control movement. Needs to be passed both the setMotor
    /// and setFSM functions.
    /// </summary>
    public abstract class AbstractMovementState : AbstractState {
        // Movement Motor, needed to actually move the attached entity
        protected MovementMotor2D Motor { get; private set; }

        protected MovementStateMachine MovementFSM;

        public void SetFSM(MovementStateMachine movementStateMachine) {
            MovementFSM = movementStateMachine;
        }

        public void SetMotor(MovementMotor2D motor) {
            Motor = motor;
        }

        public override void Enter() {
            base.Enter();
            // This gets called whenever a state change occurs
            if (MovementFSM == null) {
                Logger.Error($"A valid Movement FSM was not set on the {StateName} state");
            }

            if (Motor == null) {
                Logger.Error($"A valid movement motor was not set on {StateName}");
            }
        }

        public override void PhysicsUpdate() {
            base.PhysicsUpdate();
            Motor.Move(MovementFSM.newVelocity.Value * Time.fixedDeltaTime);
        }
    }
}