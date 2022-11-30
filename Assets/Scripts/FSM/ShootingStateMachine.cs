using UnityAtoms.BaseAtoms;
using UnityEngine;
using Utils;

namespace FSM {
    internal enum ShootingStates {
        Idle,
        Cooldown,
        Shoot
    }

    public class ShootingStateMachine : MonoBehaviour {
        [Header("Atoms")] [SerializeField] public BoolReference fireInput;
        [SerializeField] public VoidBaseEventReference shootingEvent;


        [Header("Cooldown")] [ReadOnly] public float remainingCooldownSeconds;
        public float cooldownSeconds;

        // Replace with VerboseStateMachine if you want to debug the states.
        [Header("Internal State Machine")]
        [SerializeField]
        private StateMachine<ShootingStates> _fsm;

        void Start() {
            _fsm = new StateMachine<ShootingStates>();

            // Idle State
            _fsm.AddState(ShootingStates.Idle);

            // Cooldown State
            _fsm.AddState(ShootingStates.Cooldown, new State<ShootingStates>(
                (_) => remainingCooldownSeconds = cooldownSeconds
            ).AddAction("OnFixedUpdate", () => remainingCooldownSeconds -= Time.deltaTime));

            // Shooting State
            _fsm.AddState(ShootingStates.Shoot,
                onLogic: (state) => shootingEvent.Event.Raise()
            );

            // Transitions
            // Idle -> Shoot
            _fsm.AddTransition(
                ShootingStates.Idle,
                ShootingStates.Shoot,
                (_) => fireInput.Value
            );

            // Shot -> Cooldown
            _fsm.AddTransition(
                ShootingStates.Shoot,
                ShootingStates.Cooldown
            );

            // Cooldown -> Idle
            _fsm.AddTransition(
                ShootingStates.Cooldown
                , ShootingStates.Idle,
                (_) => remainingCooldownSeconds <= 0);


            // This configures the entry point of the state machine
            _fsm.SetStartState(ShootingStates.Idle);

            // Initialises the state machine and must be called before OnLogic() is called
            _fsm.Init();
        }

        void Update() {
            _fsm.OnLogic();
        }

        void FixedUpdate() {
            _fsm.OnAction("OnFixedUpdate");
        }
    }
}