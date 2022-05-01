using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace FSM.Movement.States.Air {
    [CreateAssetMenu(fileName = "JumpState", menuName = "FSM/States/Jumping", order = 4)]
    public class JumpState : AbstractAirState {
        // Jump height in unity units / cells, how many blocks
        // TODO: Code higher jump if space bar is held
        [SerializeField] public FloatConstant minJumpHeight;
        public float maxJumpHeight = 4.5f;


        public override void Enter() {
            base.Enter();
            NewVelocity.Value = new Vector2(NewVelocity.Value.x, Mathf.Sqrt(2f * minJumpHeight.Value * -gravity.Value));
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