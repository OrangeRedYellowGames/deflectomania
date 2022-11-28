using NLog;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Assertions;
using Logger = NLog.Logger;

namespace Entities.Bullet {
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class LaserBullet : MonoBehaviour {
        [Header("Atoms")]
        [Tooltip("The speed of the bullet.")]
        public FloatVariableInstancer speed;

        [Tooltip("Event triggered whenever the speed variable changes.")]
        public FloatEventReference speedChangedEvent;

        public IntVariableInstancer numOfReflections;

        // Increase this if bullets are getting stuck inside walls / deflection shield
        private readonly float _reflectionForce = 20f;
        private Rigidbody2D _rb;
        private AudioSource _hitWallSound;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();


        private void OnDrawGizmos() {
            // TODO: Figure out why this throws errors when LaserBullet is viewed in prefab mode.
            var normalizedVelocityVector = _rb.velocity.normalized;
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position,
                new Vector3(normalizedVelocityVector.x, normalizedVelocityVector.y, 0) * 1 + transform.position);
        }

        private void Awake() {
            _hitWallSound = GetComponent<AudioSource>();
            Assert.IsNotNull(_hitWallSound);
            Assert.IsNotNull(speed);
            _rb = GetComponent<Rigidbody2D>();
        }

        /// <summary>
        /// Use onEnable to run the code whenever the object is enabled again.
        /// </summary>
        private void OnEnable() {
            // Reset the number of reflections everytime.
            numOfReflections.Value = numOfReflections.Base.InitialValue;

            // Reset speed OnEnable
            speed.Value = speed.Base.Value;
            _rb.velocity = transform.right * speed.Value;

            // Registering of event needs to in OnEnable instead of Awake() or Start() because this is a pooled object.
            // Event won't be triggered correctly otherwise.
            speedChangedEvent.Event.Register(ChangeBulletSpeed);
        }

        private void OnDisable() {
            speedChangedEvent.Event.Unregister(ChangeBulletSpeed);
        }

        /// <summary>
        /// Callback function that should be called whenever the speed variable changes.
        ///
        /// This is needed because if another entity changes this object's speed, it won't have any effect until the bullet reflects off a surface.
        /// </summary>
        /// <param name="value"></param>
        public void ChangeBulletSpeed(float value) {
            _rb.velocity = transform.right * value;
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
            // Destroy the bullet if the reflection count reached 0
            if (numOfReflections.Value == 0 || collision.gameObject.CompareTag("Player")) {
                // Set gameObject value to inActive to be added back to the pool.
                gameObject.SetActive(false);
            }
            else {
                _hitWallSound.Play();
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
            _rb.velocity = transformRight * speed.Value;

            // Decrement the number of reflections variable
            numOfReflections.Value--;
        }
    }
}