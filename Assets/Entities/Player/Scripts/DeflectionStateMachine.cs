using FSM;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Assertions;
using Utils;

namespace Entities.Player.Scripts {
    internal enum DeflectionStates {
        Idle,
        Cooldown,
        Deflect
    }

    public class DeflectionStateMachine : MonoBehaviour {
        [Header("ReadOnly")] [ReadOnly] public float remainingCooldownSeconds;
        [ReadOnly] public float remainingDeflectionDuration;

        [Header("GameObject")] public GameObject deflectionShield;

        [Header("Atoms")] [SerializeField] public BoolReference deflectionInput;

        [Header("Configuration")] public float deflectionDuration = 1f;
        public float cooldownSeconds;

        // FSM from Unity HFSM. Replace with VerboseStateMachine if you want to debug the states.
        [Header("Internal State Machine")]
        [SerializeField]
        private StateMachine<DeflectionStates> _fsm;

        private void Awake() {
            Assert.IsNotNull(deflectionShield,
                "DeflectionStateMachine needs to be supplied a deflection shield object.");
            Assert.IsNotNull(deflectionInput, "deflectionInput not set in DeflectionStateMachine.");
        }

        void Start() {
            _fsm = new StateMachine<DeflectionStates>();

            var idleState = new State<DeflectionStates>((_) => deflectionShield.SetActive(false));
            var cooldownState =
                new State<DeflectionStates>((_) => {
                    deflectionShield.SetActive(false);
                    remainingCooldownSeconds = cooldownSeconds;
                }).AddAction(
                    "OnFixedUpdate", () => remainingCooldownSeconds -= Time.deltaTime);

            var deflectionState = new State<DeflectionStates>((_) => {
                deflectionShield.SetActive(true);
                remainingDeflectionDuration = deflectionDuration;
            }).AddAction(
                "OnFixedUpdate", () => remainingDeflectionDuration -= Time.deltaTime);

            // States
            _fsm.AddState(DeflectionStates.Idle, idleState);
            _fsm.AddState(DeflectionStates.Cooldown, cooldownState);
            _fsm.AddState(DeflectionStates.Deflect, deflectionState);

            // Transitions
            _fsm.AddTransition(DeflectionStates.Idle, DeflectionStates.Deflect, (_) => deflectionInput.Value);
            _fsm.AddTransition(DeflectionStates.Deflect, DeflectionStates.Cooldown,
                (_) => remainingDeflectionDuration <= 0);
            _fsm.AddTransition(DeflectionStates.Cooldown, DeflectionStates.Idle, (_) => remainingCooldownSeconds <= 0);

            // This configures the entry point of the state machine
            _fsm.SetStartState(DeflectionStates.Idle);

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