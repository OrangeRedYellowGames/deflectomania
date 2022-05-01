using UnityAtoms;
using UnityAtoms.BaseAtoms;
using UnityAtoms.InputSystem;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace Utils {
    /// <summary>
    /// Script that hooks up to a PlayerInputEvent (Controls Changed Event makes the most sense) and checks which controls / device is being used, according the provided controlName.
    /// </summary>
    public class ControlsExtractor : MonoBehaviour {
        [SerializeField] public PlayerInputEvent controlsChangedEvent;

        [SerializeField] public BoolVariable target;

        public string controlName;


        // Start is called before the first frame update
        void Awake() {
            Assert.IsNotNull(controlsChangedEvent);
            Assert.IsNotNull(target);
            Assert.IsTrue(controlName.Length > 0,
                $"Control Name cannot be empty. Has to be set a string from one of the control schemes of PlayerInput");

            controlsChangedEvent.Register(ControlsChanged);
        }

        private void ControlsChanged(PlayerInput input) {
            if (input.currentControlScheme == controlName) {
                target.Value = true;
            }
            else {
                target.Value = false;
            }
        }
    }
}