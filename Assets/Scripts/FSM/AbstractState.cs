using UnityEngine;
using NLog;
using Logger = NLog.Logger;

// https://stackoverflow.com/questions/30471439/allways-call-base-method-in-override-without-mentioning-it-for-simple-scripts
namespace FSM {
    public abstract class AbstractState : ScriptableObject {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected string StateName => GetType().Name;

        public virtual void Enter() {
            Logger.Debug($"Inside {StateName}");
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