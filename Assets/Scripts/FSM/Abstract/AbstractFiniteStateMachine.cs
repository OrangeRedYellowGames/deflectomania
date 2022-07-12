using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace FSM.Abstract {
    public abstract class AbstractFiniteStateMachine : MonoBehaviour {
        // TODO: State History using stacks (limited size)
        public AbstractState CurrentState { get; protected set; }

        [Header("Information")]
        [ReadOnly]
        [SerializeField]
        public List<string> previousStates;

        [ReadOnly] public string currentStateName = "";

        private int maxStateHistory = 5;

        private void Awake() {
            previousStates = new List<string>();
        }

        public void Update() {
            CurrentState.LogicUpdate();
        }

        public void FixedUpdate() {
            CurrentState.PhysicsUpdate();
        }

        public void ChangeState(AbstractState newState) {
            CurrentState.Exit();

            // TODO: Figure out a better way to do this. Might be a might computationally heavy?
            if (previousStates.Count >= maxStateHistory) {
                previousStates.RemoveAt(0);
            }

            previousStates.Add(CurrentState.StateName);
            CurrentState = newState;
            currentStateName = CurrentState.StateName;
            newState.Enter();
        }
    }
}