using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace Entities.Player.Scripts {
    /// <summary>
    /// Adapted from https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/Components.html
    /// Needed to map Unity's player input to unity atoms
    /// </summary>
    public class InputActionToUnityAtomMapper : MonoBehaviour {
        private PlayerInput _playerInput;

        [Header("Inputs")] public FloatReference horizontalInput;
        public BoolReference verticalInput;
        public Vector2Reference lookDirection;
        public BoolReference fireInput;
        public BoolReference blockInput;

        // Target control name used to infer if player is using mouse or not
        [Header("Controls")] public BoolReference isUsingMouse;
        public string targetControlName = "KeyboardAndMouse";

        [Header("Player")] public IntReference playerId;


        private void Awake() {
            Assert.IsNotNull(horizontalInput);
            Assert.IsNotNull(verticalInput);
            Assert.IsNotNull(lookDirection);
            Assert.IsNotNull(fireInput);


            Assert.IsNotNull(isUsingMouse, "IsUsingMouse cannot be null");
            Assert.IsTrue(targetControlName.Length > 0,
                $"Control Name cannot be empty. Has to be set a string from one of the control schemes of PlayerInput");

            Assert.IsNotNull(playerId);

            _playerInput = GetComponent<PlayerInput>();
        }

        private void Start() {
            playerId.Value = _playerInput.playerIndex;
        }

        private void Update() {
            // Update controls scheme here because the event call caused issues. isLocalPlayer was being called while it was still null
            var isUsingTargetControlScheme = _playerInput.currentControlScheme == targetControlName;
            if (isUsingMouse.Value != isUsingTargetControlScheme)
                isUsingMouse.Value = isUsingTargetControlScheme;
        }

        void OnMove(InputValue value) {
            horizontalInput.Value = value.Get<float>();
        }

        // Make sure the Jump action is set to trigger whenever it is pressed or released.
        void OnJump(InputValue value) {
            verticalInput.Value = value.isPressed;
        }

        void OnLook(InputValue value) {
            lookDirection.Value = value.Get<Vector2>();
        }

        void OnFire(InputValue value) {
            fireInput.Value = value.isPressed;
        }

        void OnBlock(InputValue value) {
            blockInput.Value = value.isPressed;
        }
    }
}