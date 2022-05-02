using System;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Assertions;

namespace Weapons {
    public class LaserGun : MonoBehaviour {
        [SerializeField] public VoidEvent shootEvent;

        public GameObject bullet;
        private Transform _firePoint;

        // Start is called before the first frame update
        void Awake() {
            Assert.IsNotNull(shootEvent);
            Assert.IsNotNull(bullet);

            // Register shootEvent's callback
            shootEvent.Register(Shoot);

            _firePoint = transform.Find("FirePoint");
            if (!_firePoint) {
                throw new NullReferenceException(
                    "LaserGun couldn't find the \"FirePoint\" Transform of its attached gun");
            }
        }

        void OnDestroy() {
            shootEvent.Unregister(Shoot);
        }

        /// <summary>
        /// Function responsible for spawning a new bullet from with firepoint as it's starting point
        /// </summary>
        private void Shoot() {
            Instantiate(bullet, _firePoint.position, _firePoint.rotation);
        }
    }
}