using NLog;

// https://stackoverflow.com/questions/30471439/allways-call-base-method-in-override-without-mentioning-it-for-simple-scripts
namespace FSM.Abstract {
    public abstract class AbstractState {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public string StateName => GetType().Name;

        public virtual void Enter() {
            // Logger.Debug($"Inside {StateName}");
        }

        public virtual void LogicUpdate() {
        }

        public virtual void PhysicsUpdate() {
        }

        public virtual void Exit() {
        }
    }
}