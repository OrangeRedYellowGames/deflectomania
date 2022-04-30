using UnityEngine;

namespace FSM.Abstract {
    public abstract class AbstractFiniteStateMachine : MonoBehaviour {
        // TODO: State History using stacks (limited size)
        [SerializeField] public AbstractState CurrentState { get; protected set; }


        public void Update() {
            CurrentState.LogicUpdate();
        }

        public void FixedUpdate() {
            CurrentState.PhysicsUpdate();
        }

        public void ChangeState(AbstractState newState) {
            CurrentState.Exit();

            CurrentState = newState;
            newState.Enter();
        }
    }
}