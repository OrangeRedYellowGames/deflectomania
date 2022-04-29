using UnityEngine;

// https://stackoverflow.com/questions/30471439/allways-call-base-method-in-override-without-mentioning-it-for-simple-scripts
namespace FSM {
    public abstract class State : MonoBehaviour {
        protected StateMachine StateMachine;

        protected State(StateMachine stateMachine) {
            StateMachine = stateMachine;
            StateMachine = stateMachine;
        }

        public string StateName => GetType().Name;

        public virtual void Enter() {
        }

        public virtual void HandleInput() {
        }

        public virtual void LogicUpdate() {
        }

        public virtual void PhysicsUpdate() {
        }

        public virtual void Exit() {
        }
    }
}