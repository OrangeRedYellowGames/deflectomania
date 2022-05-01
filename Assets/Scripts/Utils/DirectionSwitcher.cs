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
        /// Reference Vector2. Should be relative to the attached GameObject.
        /// Currently, this is assumed to be a mouse position which will be transformed into a world point via
        /// the ScreenToWorldPoint function.
        /// </summary>
        [SerializeField]
        public Vector2Variable reference;

        private static readonly Vector3 RightRotationVector = new(0f, 0f, 0f);
        private static readonly Vector3 LeftRotationVector = new(0f, 180f, 0f);

        void Awake() {
            Assert.IsNotNull(reference);
        }

        void Update() {
            var mouseWorldPosition = CameraUtils.MainCamera.ScreenToWorldPoint(reference.Value);

            // Default will be right
            if (mouseWorldPosition.x < transform.position.x) {
                transform.rotation = Quaternion.Euler(LeftRotationVector);
            }
            else {
                transform.rotation = Quaternion.Euler(RightRotationVector);
            }
        }
    }
}