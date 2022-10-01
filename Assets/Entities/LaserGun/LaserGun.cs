using NLog;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Assertions;
using Logger = NLog.Logger;

namespace Entities.LaserGun {
    public class LaserGun : MonoBehaviour {
        [SerializeField] public VoidBaseEventReference shootEvent;
        [SerializeField] public IntConstant maxNumberOfBullets;
        [SerializeField] public FloatConstant reloadTime;

        [SerializeField] public IntReference currentNumberOfBullets;
        [SerializeField] public FloatReference remainingReloadTime;


        public GameObject bullet;
        public Transform firePoint;

        public bool resetReloadTimerAfterFiring = true;

        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        // Start is called before the first frame update
        void Awake() {
            Assert.IsNotNull(shootEvent);
            Assert.IsNotNull(bullet);
            Assert.IsNotNull(firePoint);

            // Register shootEvent's callback
            shootEvent.Event.Register(Shoot);
        }

        private void Start() {
            currentNumberOfBullets.Value = maxNumberOfBullets.Value;
            remainingReloadTime.Value = reloadTime.Value;
        }

        private void FixedUpdate() {
            if (currentNumberOfBullets.Value == maxNumberOfBullets.Value) return;
            remainingReloadTime.Value -= Time.deltaTime;

            if (remainingReloadTime.Value <= 0.0f) {
                currentNumberOfBullets.Value += 1;
                remainingReloadTime.Value = reloadTime.Value;
            }
        }

        void OnDestroy() {
            shootEvent.Event.Unregister(Shoot);
        }

        /// <summary>
        /// Function responsible for spawning a new bullet from with firepoint as it's starting point
        /// </summary>
        private void Shoot() {
            if (currentNumberOfBullets.Value > 0) {
                Instantiate(bullet, firePoint.position, firePoint.rotation);

                // Reduce number of bullets in the gun
                currentNumberOfBullets.Value -= 1;

                if (resetReloadTimerAfterFiring) {
                    remainingReloadTime.Value = reloadTime.Value;
                }
            }
            else {
                // Send an event that player is trying to shoot an empty gun
            }
        }
    }
}