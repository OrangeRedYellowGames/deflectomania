using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Assertions;

namespace Weapons {
    public class LaserGun : MonoBehaviour {
        [SerializeField] public VoidBaseEventReference shootEvent;
        public GameObject bullet;
        public Transform firePoint;

        // Start is called before the first frame update
        void Awake() {
            Assert.IsNotNull(shootEvent);
            Assert.IsNotNull(bullet);
            Assert.IsNotNull(firePoint);

            // Register shootEvent's callback
            shootEvent.Event.Register(Shoot);
        }

        void OnDestroy() {
            shootEvent.Event.Unregister(Shoot);
        }

        /// <summary>
        /// Function responsible for spawning a new bullet from with firepoint as it's starting point
        /// </summary>
        private void Shoot() {
            Instantiate(bullet, firePoint.position, firePoint.rotation);
        }
    }
}