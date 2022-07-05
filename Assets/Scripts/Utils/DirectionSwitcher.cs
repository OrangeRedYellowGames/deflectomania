using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Assertions;

namespace Utils {
    /// <summary>
    /// Changes the direction of the attached GameObject based on reference.
    /// Example usage would be to switch the player's (GameObject) direction to face the mouse (reference)
    /// </summary>
    public class DirectionSwitcher : MonoBehaviour {
        /// <summary>
        /// Reference Vector2. Can either be a look position from a mouse (absolute) or a look direction from a gamepad.
        /// </summary>
        [SerializeField]
        public Vector2Reference lookDirection;

        [SerializeField] public BoolReference isUsingMouse;

        private static readonly Vector3 RightRotationVector = new(0f, 0f, 0f);
        private static readonly Vector3 LeftRotationVector = new(0f, 180f, 0f);

        private Vector3 _newRotationVector;

        void Awake() {
            Assert.IsNotNull(lookDirection);
            Assert.IsNotNull(isUsingMouse);
        }

        void Update() {
            if (isUsingMouse.Value) {
                // Need to translate mouse coordinates into a world point
                var mouseWorldPosition = CameraUtils.MainCamera.ScreenToWorldPoint(lookDirection.Value);

                // Default will be right
                _newRotationVector = mouseWorldPosition.x < transform.position.x
                    ? LeftRotationVector
                    : RightRotationVector;
            }
            else {
                // Don't change direction if stick is in the middle
                if (lookDirection.Value.x != 0.0f) {
                    _newRotationVector = lookDirection.Value.x < 0 ? LeftRotationVector : RightRotationVector;
                }
            }

            transform.rotation = Quaternion.Euler(_newRotationVector);
        }
    }
}