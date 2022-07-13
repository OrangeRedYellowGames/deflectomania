using Mirror;
using NLog;
using UnityEngine;
using Logger = NLog.Logger;

namespace Weapons.Bullet {
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class LaserBullet : NetworkBehaviour {
        [SyncVar] public int speed = 20;
        [SyncVar] public int numOfReflections = 3;


        private Rigidbody2D _rb;
        private float _reflectionForce = 5f;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private void OnDrawGizmos() {
            // TODO: Figure out why this throws errors when LaserBullet is viewed in prefab mode.
            var normalizedVelocityVector = _rb.velocity.normalized;
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position,
                new Vector3(normalizedVelocityVector.x, normalizedVelocityVector.y, 0) * 1 + transform.position);
        }

        private void Awake() {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Start() {
            _rb.velocity = transform.right * speed;
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            ReflectBullet(collision);
        }

        /// <summary>
        /// Needed in addition to OnCollisionEnter2D in order to detect cases where the bullet collides with a wall,
        /// is reflected but is still colliding with the same wall. This happens when the bullets hits a corner, for example.
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionStay2D(Collision2D collision) {
            ReflectBullet(collision);
        }

        private void ReflectBullet(Collision2D collision) {
            if (!isServer) {
                return;
            }

            // Destroy the bullet if the reflection count reached 0
            if (numOfReflections == 0 || collision.gameObject.CompareTag("Player")) {
                // Destroy(gameObject);
                NetworkServer.Destroy(gameObject);
            }

            // TODO: Handle case where bullets hit the corner of the wall. Should reflect bullet back in the same direction
            if (collision.contactCount > 1) {
                Logger.Warn(
                    $"Bullet collided with {collision.contactCount.ToString()} contact points. Using contact point at index 0 to calculate the reflection angle.");
            }

            // Get the contact point at index 0
            ContactPoint2D contact = collision.GetContact(0);

            // Cannot use _rb.velocity here since it will be zero. This is because OnCollisionEnter2D gets called after FixedUpdate gets called
            // where the _rb.velocity will be zero because a collision occured. See https://forum.unity.com/threads/vector3-reflection-not-working-as-it-should.517743/
            // for more details
            var transformRight = transform.right;
            var newDirection = Vector3.Reflect(transformRight, contact.normal);

            transformRight = newDirection;
            transform.right = transformRight;

            // Push the bullet outside a bit so it doesn't get stuck reflecting inside of the wall
            // Seems to be working correctly because of Space.World
            transform.Translate(transformRight * Time.deltaTime * _reflectionForce, Space.World);

            // Set velocity on the RB.
            _rb.velocity = transformRight * speed;

            // Decrement the number of reflections variable
            numOfReflections--;
        }
    }
}