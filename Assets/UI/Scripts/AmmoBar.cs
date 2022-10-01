using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace UI.Scripts {
    [RequireComponent(typeof(Slider))]
    public class AmmoBar : MonoBehaviour {
        public GameObject ammoPrefab;

        /// <summary>
        /// Maximum Ammo value. Will be set as the initial value of the Ammo bar.
        /// </summary>
        public IntConstant maxAmmo;

        /// <summary>
        /// Float Event to listen to.
        /// </summary>
        public IntEventReference ammoChangedEvent;

        public FloatConstant reloadTime;
        public FloatEventReference remainingReloadTimeEvent;

        private List<Slider> _ammoSliders;
        private int _currentNumberOfAmmo;

        void Awake() {
            // Asserts ammoChangedEvent is set
            Assert.IsNotNull(ammoChangedEvent, "ammoChangedEvent is missing in AmmoBar");
            ammoChangedEvent.Event.Register(ChangeAmmoValue);

            // Asserts ammoChangedEvent is set
            Assert.IsNotNull(remainingReloadTimeEvent, "remainingReloadTime is missing in AmmoBar");
            remainingReloadTimeEvent.Event.Register(ChangeRemainingReloadTime);

            // Assert unity atom variables are set
            Assert.IsNotNull(maxAmmo, "MaxAmmo Variable cannot be null in AmmoBar");
            
            _ammoSliders = new List<Slider>(maxAmmo.Value);
        }

        void Start() {
            for (int i = 0; i < maxAmmo.Value; i++) {
                var ammo = Instantiate(ammoPrefab, this.transform, false);
                _ammoSliders.Add(ammo.GetComponent<Slider>());
            }
        }

        void OnDestroy() {
            ammoChangedEvent.Event.Unregister(ChangeAmmoValue);
        }

        private void LateUpdate() {
            // Figure out a better way to do this
            // https://www.reddit.com/r/Unity2D/comments/7kj23o/ui_flipping_child_of_the_player/
            transform.eulerAngles = Vector3.zero;
        }

        /// <summary>
        /// Function that changes the value of the Ammo bar.
        ///
        /// Should be called whenever the ammoChangedEvent is raised.
        /// </summary>
        /// <param name="amount">New value of the HP bar</param>
        public void ChangeAmmoValue(int amount) {
            for (int i = 0; i < _ammoSliders.Count; i++) {
                _ammoSliders[i].value = amount > i ? 1 : 0;
            }

            _currentNumberOfAmmo = amount;
        }

        public void ChangeRemainingReloadTime(float value) {
            if (_currentNumberOfAmmo == maxAmmo.Value) return;
            _ammoSliders[_currentNumberOfAmmo].value = 1 - (value / reloadTime.Value);
        }
    }
}