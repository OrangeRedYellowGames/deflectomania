using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

// References: https://www.monkeykidgc.com/2021/03/unity-moving-platform.html
// Moving Platform needs to be placed inside the grid!
namespace Environment.MovingPlatforms {
    public class MovePlatform : MonoBehaviour {
        public GameObject platformPathEnd;
        public int speed = 5;
        private Vector2 _endPosition;
        private Rigidbody2D _rBody;

        private void Awake() {
            _rBody = GetComponent<Rigidbody2D>();
            Assert.IsNotNull(platformPathEnd);
            Assert.IsNotNull(_rBody);
        }

        /// <summary>
        /// On Start the MovingPlatforms object, starts an infinite co routing that keeps the moving platform
        /// looping between the start and end point. The start of the loop is the current position of the
        /// object and the end is the platformPathEnd object position.
        /// </summary>
        private void Start() {
            _endPosition = platformPathEnd.transform.position;
            Vector2[] paths = { _rBody.transform.position, _endPosition };
            _rBody.DOPath(paths, speed).SetEase(Ease.Linear).SetSpeedBased().SetLoops(-1, LoopType.Yoyo);
        }

        /// <summary>
        /// This function gets called whenever there is a collision. It only works when the collision is with a
        /// player. It checks if the player is above the platform. If yes, it parents the player to the platform
        /// and sets its RigidbodyType to Kinematic, to allow it to move with the platfrom.
        /// </summary>
        /// <param name="col"></param>
        private void OnCollisionEnter2D(Collision2D col) {
            if (col.gameObject.CompareTag("Player")) {
                Vector3 contactPoint = col.contacts[0].point;
                Vector3 center = col.collider.bounds.max;

                // Check if player position is above the platform.
                if (contactPoint.y < center.y) {
                    // Parent and set RigidbodyType to Kinematic to move player with platform.
                    col.gameObject.transform.SetParent(gameObject.transform, true);
                    col.rigidbody.bodyType = RigidbodyType2D.Kinematic;
                }
            }
        }

        /// <summary>
        /// This function gets called whenever there is a collision exit. It only works when exiting a collision
        /// with a player. It returns the player type back to static and un-parent the player from the platform.
        /// </summary>
        /// <param name="col"></param>
        private void OnCollisionExit2D(Collision2D col) {
            if (col.gameObject.CompareTag("Player")) {
                col.rigidbody.bodyType = RigidbodyType2D.Static;
                col.gameObject.transform.parent = null;
            }
        }
    }
}