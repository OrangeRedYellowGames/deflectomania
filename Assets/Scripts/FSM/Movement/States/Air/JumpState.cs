using UnityEngine;

namespace FSM.Movement.States.Air {
    [CreateAssetMenu(fileName = "JumpState", menuName = "FSM/States/Jumping", order = 4)]
    public class JumpState : AbstractAirState {
        // Jump height in unity units / cells, how many blocks
        // TODO: Code higher jump if space bar is held
        public float minJumpHeight = 3.5f;
        public float maxJumpHeight = 4.5f;


        public override void Enter() {
            base.Enter();
            NewVelocity.y = Mathf.Sqrt(2f * minJumpHeight * -Gravity);
        }

        public override void LogicUpdate() {
            base.LogicUpdate();
            // If velocity reaches 0 or if player hits a platform above him
            if (Motor.velocity.y <= 0 || Motor.collisionState.Above) {
                MovementFSM.ChangeState(MovementFSM.fallState);
            }
        }
    }
}