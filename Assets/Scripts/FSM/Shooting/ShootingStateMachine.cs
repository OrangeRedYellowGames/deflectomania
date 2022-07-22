using System.Collections.Generic;
using FSM.Abstract;
using FSM.Shooting.States;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace FSM.Shooting {
    public class ShootingStateMachine : AbstractFiniteStateMachine {
        [SerializeField] public BoolReference fireInput;
        [SerializeField] public VoidBaseEventReference shootingEvent;

        public float remainingCooldownSeconds;
        public float cooldownSeconds;

        // Shooting states
        public ShootingState ShootingState;
        public CooldownState CooldownState;

        public void Awake() {
            ShootingState = new ShootingState();
            CooldownState = new CooldownState();

            // Loop over each state and set the FSM, needed so that states are able to do transitions
            var stateList = new List<AbstractShootingState> {
                ShootingState, CooldownState
            };

            // Set the FSM and Motor variables for each movement state
            foreach (var state in stateList) {
                state.SetFSM(this);
            }

            // Set starting state to idle
            CurrentState = ShootingState;

            CurrentState.Enter();
        }
    }
}