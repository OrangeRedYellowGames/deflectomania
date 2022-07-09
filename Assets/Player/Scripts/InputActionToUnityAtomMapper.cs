using Mirror;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace Player.Scripts {
    /// <summary>
    /// // Adapted from https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/Components.html
    /// </summary>
    public class InputActionToUnityAtomMapper : NetworkBehaviour {
        [Header("Inputs")] public FloatReference horizontalInput;
        public BoolReference verticalInput;
        public Vector2Reference lookDirection;
        public BoolReference fireInput;

        // Target control name used to infer if player is using mouse or not
        [Header("Controls")] public BoolReference isUsingMouse;
        public string targetControlName = "KeyboardAndMouse";

        private PlayerInput _playerInput;

        private void Awake() {
            Assert.IsNotNull(horizontalInput);
            Assert.IsNotNull(verticalInput);
            Assert.IsNotNull(lookDirection);
            Assert.IsNotNull(fireInput);

            Assert.IsNotNull(isUsingMouse, "IsUsingMouse cannot be null");
            Assert.IsTrue(targetControlName.Length > 0,
                $"Control Name cannot be empty. Has to be set a string from one of the control schemes of PlayerInput");

            _playerInput = GetComponent<PlayerInput>();
        }

        private void Update() {
            // Update controls scheme here because the event call caused issues. isLocalPlayer was being called while it was still null
            if (!isLocalPlayer) return;
            var isUsingTargetControlScheme = _playerInput.currentControlScheme == targetControlName;
            if (isUsingMouse.Value != isUsingTargetControlScheme)
                isUsingMouse.Value = isUsingTargetControlScheme;
        }

        void OnMove(InputValue value) {
            if (!isLocalPlayer) {
                return;
            }

            horizontalInput.Value = value.Get<float>();
        }

        // Make sure the Jump action is set to trigger whenever it is pressed or released.
        void OnJump(InputValue value) {
            if (!isLocalPlayer) {
                return;
            }

            verticalInput.Value = value.isPressed;
        }

        void OnLook(InputValue value) {
            if (!isLocalPlayer) {
                return;
            }

            lookDirection.Value = value.Get<Vector2>();
        }

        void OnFire(InputValue value) {
            if (!isLocalPlayer) {
                return;
            }

            fireInput.Value = value.isPressed;
        }
    }
}