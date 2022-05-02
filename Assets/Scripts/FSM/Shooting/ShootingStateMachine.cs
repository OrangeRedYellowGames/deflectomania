using System.Collections.Generic;
using FSM.Abstract;
using FSM.Shooting.States;

namespace FSM.Shooting {
    public class ShootingStateMachine : AbstractFiniteStateMachine {
        // Shooting states
        public ShootingState shootingState;
        public CooldownState cooldownState;

        public void Awake() {
            // Loop over each state and set the FSM, needed so that states are able to do transitions
            var stateList = new List<AbstractShootingState> {
                shootingState, cooldownState
            };

            // Set the FSM and Motor variables for each movement state
            foreach (var state in stateList) {
                state.SetFSM(this);
            }

            // Set starting state to idle
            CurrentState = shootingState;

            CurrentState.Enter();
        }
    }
}