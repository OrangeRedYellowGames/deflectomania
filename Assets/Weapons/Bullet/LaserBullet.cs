using NLog;
using UnityEngine;
using Logger = NLog.Logger;

namespace Weapons.Bullet {
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class LaserBullet : MonoBehaviour {
        public int speed = 20;
        public int numOfReflections = 3;
        private Rigidbody2D _rb;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private void OnDrawGizmos() {
            var normalizedVelocityVector = _rb.velocity.normalized;
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position,
                new Vector3(normalizedVelocityVector.x, normalizedVelocityVector.y, 0) * 2 + transform.position);
        }

        void Awake() {
            _rb = GetComponent<Rigidbody2D>();
        }

        void Start() {
            _rb.velocity = transform.right * speed;
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            // Destroy the bullet if the reflection count reached 0
            if (numOfReflections == 0) {
                Destroy(gameObject);
            }

            if (collision.contactCount > 1) {
                Logger.Warn(
                    $"Bullet collided with {collision.contactCount.ToString()} contact points. Using contact point at index 0 to calculate the reflection angle.");
            }

            // Get the contact point at index 0
            ContactPoint2D contact = collision.GetContact(0);

            // Cannot use _rb.velocity here since it will be zero. This is because OnCollisionEnter2D gets called after FixedUpdate gets called
            // where the _rb.velocity will be zero because a collision occured. See https://forum.unity.com/threads/vector3-reflection-not-working-as-it-should.517743/
            // for more details
            var newDirection = Vector3.Reflect(-collision.relativeVelocity, contact.normal);

            transform.right = newDirection;
            _rb.velocity = transform.right * speed;

            // Decrement the number of reflections variable
            numOfReflections--;
        }
    }
}