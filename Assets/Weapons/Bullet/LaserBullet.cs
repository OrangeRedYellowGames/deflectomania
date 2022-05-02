using System;
using System.Net;
using UnityEngine;

namespace Weapons.Bullet {
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class LaserBullet : MonoBehaviour {
        public int speed = 20;
        public int numOfReflections = 3;
        private Rigidbody2D _rb;

        void Awake() {
            _rb = GetComponent<Rigidbody2D>();
        }

        void Start() {
            _rb.velocity = transform.right * speed;
        }

        private void OnTriggerEnter2D(Collider2D col) {
            Destroy(gameObject);
        }
    }
}