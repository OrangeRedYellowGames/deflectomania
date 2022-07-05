using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace Player.Scripts {
    /// <summary>
    /// // Adapted from https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/Components.html
    /// </summary>
    public class InputActionToUnityAtomMapper : MonoBehaviour {
        public FloatReference horizontalInput;
        public BoolReference verticalInput;
        public Vector2Reference lookDirection;
        public BoolReference fireInput;

        // Target control name used to infer if player is using mouse or not
        public BoolReference isUsingMouse;
        public string targetControlName = "KeyboardAndMouse";

        private void Awake() {
            Assert.IsNotNull(horizontalInput);
            Assert.IsNotNull(verticalInput);
            Assert.IsNotNull(lookDirection);
            Assert.IsNotNull(fireInput);

            Assert.IsNotNull(isUsingMouse);
            Assert.IsTrue(targetControlName.Length > 0,
                $"Control Name cannot be empty. Has to be set a string from one of the control schemes of PlayerInput");
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

        void OnControlsChanged(PlayerInput input) {
            if (input.currentControlScheme == targetControlName) {
                isUsingMouse.Value = true;
            }
            else {
                isUsingMouse.Value = false;
            }
        }
    }
}