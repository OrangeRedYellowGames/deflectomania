using System;
using System.Collections.Generic;
using UnityEngine;

namespace FSM.Abstract {
    [Serializable]
    // ReSharper disable once InconsistentNaming
    public class VerboseStateMachine<TOwnId, TStateId, TEvent> : StateMachine<TOwnId, TStateId, TEvent> {
        public bool logToConsole = false;
        public int maxStateLogHistory;
        public List<TStateId> stateLogList;

        public VerboseStateMachine(bool logToConsole = false, int maxStateLogHistory = 5,
            bool needsExitTime = true, bool isGhostState = false) : base(needsExitTime: needsExitTime,
            isGhostState: isGhostState) {
            this.logToConsole = logToConsole;
            this.maxStateLogHistory = maxStateLogHistory;
            stateLogList = new List<TStateId>();

            this.onStateChange += LogStateChange;
        }

        ~VerboseStateMachine() {
            this.onStateChange -= LogStateChange;
        }

        private void LogStateChange(TStateId newState) {
            if (logToConsole) {
                Debug.Log(newState.ToString());
            }

            // TODO: Figure out a better way to do this. Might be a might computationally heavy?
            if (stateLogList.Count >= maxStateLogHistory) {
                stateLogList.RemoveAt(0);
            }

            stateLogList.Add(newState);
        }
    }

    [Serializable]
    // ReSharper disable once InconsistentNaming
    public class VerboseStateMachine<TStateId, TEvent> : VerboseStateMachine<TStateId, TStateId, TEvent> {
        public VerboseStateMachine(bool logToConsole = false, int maxStateLogHistory = 5, bool needsExitTime = true,
            bool isGhostState = false)
            : base(logToConsole, maxStateLogHistory, needsExitTime, isGhostState) {
        }
    }

    [Serializable]
    // ReSharper disable once InconsistentNaming
    public class VerboseStateMachine<TStateId> : VerboseStateMachine<TStateId, TStateId, string> {
        public VerboseStateMachine(bool logToConsole = false, int maxStateLogHistory = 5, bool needsExitTime = true,
            bool isGhostState = false)
            : base(logToConsole, maxStateLogHistory, needsExitTime, isGhostState) {
        }
    }

    [Serializable]
    // ReSharper disable once InconsistentNaming
    public class VerboseStateMachine : VerboseStateMachine<string, string, string> {
        public VerboseStateMachine(bool logToConsole = false, int maxStateLogHistory = 5, bool needsExitTime = true,
            bool isGhostState = false)
            : base(logToConsole, maxStateLogHistory, needsExitTime, isGhostState) {
        }
    }
}