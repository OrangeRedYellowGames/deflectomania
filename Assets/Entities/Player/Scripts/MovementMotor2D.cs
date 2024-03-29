#define DEBUG_CC2D_RAYS
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player.Scripts {
    [RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
    public class MovementMotor2D : MonoBehaviour {
        #region internal types

        struct CharacterRaycastOrigins {
            public Vector3 TopLeft;
            public Vector3 BottomRight;
            public Vector3 BottomLeft;
        }

        public class CharacterCollisionState2D {
            public bool Right;
            public bool Left;
            public bool Above;
            public bool Below;
            public bool BecameGroundedThisFrame;
            public bool WasGroundedLastFrame;
            public bool MovingDownSlope;
            public float SlopeAngle;


            public bool HasCollision() {
                return Below || Right || Left || Above;
            }


            public void Reset() {
                Right = Left = Above = Below = BecameGroundedThisFrame = MovingDownSlope = false;
                SlopeAngle = 0f;
            }


            public override string ToString() {
                return
                    $"[CharacterCollisionState2D] r: {Right}, l: {Left}, a: {Above}, b: {Below}, movingDownSlope: {MovingDownSlope}, angle: {SlopeAngle}, wasGroundedLastFrame: {WasGroundedLastFrame}, becameGroundedThisFrame: {BecameGroundedThisFrame}";
            }
        }

        #endregion


        #region events, properties and fields

        public event Action<RaycastHit2D> ONControllerCollidedEvent;
        public event Action<Collider2D> ONTriggerEnterEvent;
        public event Action<Collider2D> ONTriggerStayEvent;
        public event Action<Collider2D> ONTriggerExitEvent;


        /// <summary>
        /// when true, one way platforms will be ignored when moving vertically for a single frame
        /// </summary>
        public bool ignoreOneWayPlatformsThisFrame;

        [FormerlySerializedAs("_skinWidth")]
        [SerializeField]
        [Range(0.001f, 0.3f)]
        private float skinWidth = 0.02f;

        /// <summary>
        /// defines how far in from the edges of the collider rays are cast from. If cast with a 0 extent it will often result in ray hits that are
        /// not desired (for example a foot collider casting horizontally from directly on the surface can result in a hit)
        /// </summary>
        private float SkinWidth {
            get => skinWidth;
            set {
                skinWidth = value;
                RecalculateDistanceBetweenRays();
            }
        }


        /// <summary>
        /// mask with all layers that the player should interact with
        /// </summary>
        public LayerMask platformMask = 0;

        /// <summary>
        /// mask with all layers that trigger events should fire when intersected
        ///
        /// EDIT: Remove this according to https://github.com/prime31/CharacterController2D/issues/104 to CollisionMatrix
        /// being edited at runtime by this script
        /// </summary>
        // public LayerMask triggerMask = 0;

        /// <summary>
        /// mask with all layers that should act as one-way platforms. Note that one-way platforms should always be EdgeCollider2Ds. This is because it does not support being
        /// updated anytime outside of the inspector for now.
        /// </summary>
        [SerializeField]
        private LayerMask oneWayPlatformMask = 0;

        /// <summary>
        /// the max slope angle that the CC2D can climb
        /// </summary>
        /// <value>The slope limit.</value>
        [Range(0f, 90f)]
        public float slopeLimit = 30f;

        /// <summary>
        /// the threshold in the change in vertical movement between frames that constitutes jumping
        /// </summary>
        /// <value>The jumping threshold.</value>
        public float jumpingThreshold = 0.07f;


        /// <summary>
        /// curve for multiplying speed based on slope (negative = down slope and positive = up slope)
        /// </summary>
        public AnimationCurve slopeSpeedMultiplier =
            new AnimationCurve(new Keyframe(-90f, 1.5f), new Keyframe(0f, 1f), new Keyframe(90f, 0f));

        [Range(2, 20)] public int totalHorizontalRays = 8;

        [Range(2, 20)] public int totalVerticalRays = 4;


        /// <summary>
        /// this is used to calculate the downward ray that is cast to check for slopes. We use the somewhat arbitrary value 75 degrees
        /// to calculate the length of the ray that checks for slopes.
        /// </summary>
        float _slopeLimitTangent = Mathf.Tan(75f * Mathf.Deg2Rad);


        [HideInInspector] [NonSerialized] public Transform Transform;

        [HideInInspector] [NonSerialized] public BoxCollider2D BoxCollider;

        [HideInInspector] [NonSerialized] public Rigidbody2D RigidBody2D;

        [HideInInspector]
        [NonSerialized]
        public readonly CharacterCollisionState2D collisionState = new CharacterCollisionState2D();

        [HideInInspector] [NonSerialized] public Vector3 velocity;

        public bool IsGrounded => collisionState.Below;

        const float KSkinWidthFloatFudgeFactor = 0.001f;

        #endregion


        /// <summary>
        /// holder for our raycast origin corners (TR, TL, BR, BL)
        /// </summary>
        CharacterRaycastOrigins _raycastOrigins;

        /// <summary>
        /// stores our raycast hit during movement
        /// </summary>
        RaycastHit2D _raycastHit;

        /// <summary>
        /// stores any raycast hits that occur this frame. we have to store them in case we get a hit moving
        /// horizontally and vertically so that we can send the events after all collision state is set
        /// </summary>
        List<RaycastHit2D> _raycastHitsThisFrame = new List<RaycastHit2D>(2);

        // horizontal/vertical movement data
        float _verticalDistanceBetweenRays;
        float _horizontalDistanceBetweenRays;

        // we use this flag to mark the case where we are travelling up a slope and we modified our delta.y to allow the climb to occur.
        // the reason is so that if we reach the end of the slope we can make an adjustment to stay grounded
        bool _isGoingUpSlope = false;


        #region Monobehaviour

        void Awake() {
            // add our one-way platforms to our normal platform mask so that we can land on them from above
            platformMask |= oneWayPlatformMask;

            // cache some components
            Transform = GetComponent<Transform>();
            BoxCollider = GetComponent<BoxCollider2D>();
            RigidBody2D = GetComponent<Rigidbody2D>();

            // here, we trigger our properties that have setters with bodies
            SkinWidth = skinWidth;

            // EDIT: Remove this according to https://github.com/prime31/CharacterController2D/issues/104 to CollisionMatrix
            // being edited at runtime by this script
            //
            // we want to set our CC2D to ignore all collision layers except what is in our triggerMask
            // for (var i = 0; i < 32; i++) {
            //     // see if our triggerMask contains this layer and if not ignore it
            //     if ((triggerMask.value & 1 << i) == 0)
            //         Physics2D.IgnoreLayerCollision(gameObject.layer, i);
            // }
        }


        public void OnTriggerEnter2D(Collider2D col) {
            if (ONTriggerEnterEvent != null)
                ONTriggerEnterEvent(col);
        }


        public void OnTriggerStay2D(Collider2D col) {
            if (ONTriggerStayEvent != null)
                ONTriggerStayEvent(col);
        }


        public void OnTriggerExit2D(Collider2D col) {
            if (ONTriggerExitEvent != null)
                ONTriggerExitEvent(col);
        }

        #endregion


        [System.Diagnostics.Conditional("DEBUG_CC2D_RAYS")]
        private void DrawRay(Vector3 start, Vector3 dir, Color color) {
            Debug.DrawRay(start, dir, color);
        }


        #region Public

        /// <summary>
        /// attempts to move the character to position + deltaMovement. Any colliders in the way will cause the movement to
        /// stop when run into.
        /// </summary>
        /// <param name="deltaMovement">Delta movement.</param>
        public void Move(Vector3 deltaMovement) {
            // save off our current grounded state which we will use for wasGroundedLastFrame and becameGroundedThisFrame
            collisionState.WasGroundedLastFrame = collisionState.Below;

            // clear our state
            collisionState.Reset();
            _raycastHitsThisFrame.Clear();
            _isGoingUpSlope = false;

            PrimeRaycastOrigins();


            // first, we check for a slope below us before moving
            // only check slopes if we are going down and grounded
            if (deltaMovement.y < 0f && collisionState.WasGroundedLastFrame)
                HandleVerticalSlope(ref deltaMovement);

            // now we check movement in the horizontal dir
            if (deltaMovement.x != 0f)
                MoveHorizontally(ref deltaMovement);

            // next, check movement in the vertical dir
            if (deltaMovement.y != 0f)
                MoveVertically(ref deltaMovement);

            // move then update our state
            deltaMovement.z = 0;
            Transform.Translate(deltaMovement, Space.World);

            // only calculate velocity if we have a non-zero deltaTime
            if (Time.fixedDeltaTime > 0f)
                velocity = deltaMovement / Time.fixedDeltaTime;

            // set our becameGrounded state based on the previous and current collision state
            if (!collisionState.WasGroundedLastFrame && collisionState.Below)
                collisionState.BecameGroundedThisFrame = true;

            // if we are going up a slope we artificially set a y velocity so we need to zero it out here
            if (_isGoingUpSlope)
                velocity.y = 0;

            // send off the collision events if we have a listener
            if (ONControllerCollidedEvent != null) {
                for (var i = 0; i < _raycastHitsThisFrame.Count; i++)
                    ONControllerCollidedEvent(_raycastHitsThisFrame[i]);
            }

            ignoreOneWayPlatformsThisFrame = false;
        }


        /// <summary>
        /// moves directly down until grounded
        /// </summary>
        public void WarpToGrounded() {
            do {
                Move(new Vector3(0, -1f, 0));
            } while (!IsGrounded);
        }


        /// <summary>
        /// this should be called anytime you have to modify the BoxCollider2D at runtime. It will recalculate the distance between the rays used for collision detection.
        /// It is also used in the skinWidth setter in case it is changed at runtime.
        /// </summary>
        public void RecalculateDistanceBetweenRays() {
            // figure out the distance between our rays in both directions
            // horizontal
            var colliderUseableHeight = BoxCollider.size.y * Mathf.Abs(Transform.localScale.y) - (2f * skinWidth);
            _verticalDistanceBetweenRays = colliderUseableHeight / (totalHorizontalRays - 1);

            // vertical
            var colliderUseableWidth = BoxCollider.size.x * Mathf.Abs(Transform.localScale.x) - (2f * skinWidth);
            _horizontalDistanceBetweenRays = colliderUseableWidth / (totalVerticalRays - 1);
        }

        #endregion


        #region Movement Methods

        /// <summary>
        /// resets the raycastOrigins to the current extents of the box collider inset by the skinWidth. It is inset
        /// to avoid casting a ray from a position directly touching another collider which results in wonky normal data.
        /// </summary>
        private void PrimeRaycastOrigins() {
            // our raycasts need to be fired from the bounds inset by the skinWidth
            var modifiedBounds = BoxCollider.bounds;
            modifiedBounds.Expand(-2f * skinWidth);

            _raycastOrigins.TopLeft = new Vector2(modifiedBounds.min.x, modifiedBounds.max.y);
            _raycastOrigins.BottomRight = new Vector2(modifiedBounds.max.x, modifiedBounds.min.y);
            _raycastOrigins.BottomLeft = modifiedBounds.min;
        }


        /// <summary>
        /// we have to use a bit of trickery in this one. The rays must be cast from a small distance inside of our
        /// collider (skinWidth) to avoid zero distance rays which will get the wrong normal. Because of this small offset
        /// we have to increase the ray distance skinWidth then remember to remove skinWidth from deltaMovement before
        /// actually moving the player
        /// </summary>
        void MoveHorizontally(ref Vector3 deltaMovement) {
            var isGoingRight = deltaMovement.x > 0;
            var rayDistance = Mathf.Abs(deltaMovement.x) + skinWidth;
            var rayDirection = isGoingRight ? Vector2.right : -Vector2.right;
            var initialRayOrigin = isGoingRight ? _raycastOrigins.BottomRight : _raycastOrigins.BottomLeft;

            for (var i = 0; i < totalHorizontalRays; i++) {
                var ray = new Vector2(initialRayOrigin.x, initialRayOrigin.y + i * _verticalDistanceBetweenRays);

                DrawRay(ray, rayDirection * rayDistance, Color.red);

                // if we are grounded we will include oneWayPlatforms only on the first ray (the bottom one). this will allow us to
                // walk up sloped oneWayPlatforms
                if (i == 0 && collisionState.WasGroundedLastFrame)
                    _raycastHit = Physics2D.Raycast(ray, rayDirection, rayDistance, platformMask);
                else
                    _raycastHit = Physics2D.Raycast(ray, rayDirection, rayDistance, platformMask & ~oneWayPlatformMask);

                if (_raycastHit) {
                    // the bottom ray can hit a slope but no other ray can so we have special handling for these cases
                    if (i == 0 &&
                        HandleHorizontalSlope(ref deltaMovement, Vector2.Angle(_raycastHit.normal, Vector2.up))) {
                        _raycastHitsThisFrame.Add(_raycastHit);
                        // if we weren't grounded last frame, that means we're landing on a slope horizontally.
                        // this ensures that we stay flush to that slope
                        if (!collisionState.WasGroundedLastFrame) {
                            float flushDistance = Mathf.Sign(deltaMovement.x) * (_raycastHit.distance - SkinWidth);
                            Transform.Translate(new Vector2(flushDistance, 0));
                        }

                        break;
                    }

                    // set our new deltaMovement and recalculate the rayDistance taking it into account
                    deltaMovement.x = _raycastHit.point.x - ray.x;
                    rayDistance = Mathf.Abs(deltaMovement.x);

                    // remember to remove the skinWidth from our deltaMovement
                    if (isGoingRight) {
                        deltaMovement.x -= skinWidth;
                        collisionState.Right = true;
                    }
                    else {
                        deltaMovement.x += skinWidth;
                        collisionState.Left = true;
                    }

                    _raycastHitsThisFrame.Add(_raycastHit);

                    // we add a small fudge factor for the float operations here. if our rayDistance is smaller
                    // than the width + fudge bail out because we have a direct impact
                    if (rayDistance < skinWidth + KSkinWidthFloatFudgeFactor)
                        break;
                }
            }
        }


        /// <summary>
        /// handles adjusting deltaMovement if we are going up a slope.
        /// </summary>
        /// <returns><c>true</c>, if horizontal slope was handled, <c>false</c> otherwise.</returns>
        /// <param name="deltaMovement">Delta movement.</param>
        /// <param name="angle">Angle.</param>
        private bool HandleHorizontalSlope(ref Vector3 deltaMovement, float angle) {
            // disregard 90 degree angles (walls)
            if (Mathf.RoundToInt(angle) == 90)
                return false;

            // if we can walk on slopes and our angle is small enough we need to move up
            if (angle < slopeLimit) {
                // we only need to adjust the deltaMovement if we are not jumping
                // TODO: this uses a magic number which isn't ideal! The alternative is to have the user pass in if there is a jump this frame
                if (deltaMovement.y < jumpingThreshold) {
                    // apply the slopeModifier to slow our movement up the slope
                    var slopeModifier = slopeSpeedMultiplier.Evaluate(angle);
                    deltaMovement.x *= slopeModifier;

                    // we dont set collisions on the sides for this since a slope is not technically a side collision.
                    // smooth y movement when we climb. we make the y movement equivalent to the actual y location that corresponds
                    // to our new x location using our good friend Pythagoras
                    deltaMovement.y = Mathf.Abs(Mathf.Tan(angle * Mathf.Deg2Rad) * deltaMovement.x);
                    var isGoingRight = deltaMovement.x > 0;

                    // safety check. we fire a ray in the direction of movement just in case the diagonal we calculated above ends up
                    // going through a wall. if the ray hits, we back off the horizontal movement to stay in bounds.
                    var ray = isGoingRight ? _raycastOrigins.BottomRight : _raycastOrigins.BottomLeft;
                    RaycastHit2D raycastHit;
                    if (collisionState.WasGroundedLastFrame)
                        raycastHit = Physics2D.Raycast(ray, deltaMovement.normalized, deltaMovement.magnitude,
                            platformMask);
                    else
                        raycastHit = Physics2D.Raycast(ray, deltaMovement.normalized, deltaMovement.magnitude,
                            platformMask & ~oneWayPlatformMask);

                    if (raycastHit) {
                        // we crossed an edge when using Pythagoras calculation, so we set the actual delta movement to the ray hit location
                        deltaMovement = (Vector3)raycastHit.point - ray;
                        if (isGoingRight)
                            deltaMovement.x -= skinWidth;
                        else
                            deltaMovement.x += skinWidth;
                    }

                    _isGoingUpSlope = true;
                    collisionState.Below = true;
                    collisionState.SlopeAngle = -angle;
                }
            }
            else // too steep. get out of here
            {
                deltaMovement.x = 0;
            }

            return true;
        }


        private void MoveVertically(ref Vector3 deltaMovement) {
            var isGoingUp = deltaMovement.y > 0;
            var rayDistance = Mathf.Abs(deltaMovement.y) + skinWidth;
            var rayDirection = isGoingUp ? Vector2.up : -Vector2.up;
            var initialRayOrigin = isGoingUp ? _raycastOrigins.TopLeft : _raycastOrigins.BottomLeft;

            // apply our horizontal deltaMovement here so that we do our raycast from the actual position we would be in if we had moved
            initialRayOrigin.x += deltaMovement.x;

            // if we are moving up, we should ignore the layers in oneWayPlatformMask
            var mask = platformMask;
            if ((isGoingUp && !collisionState.WasGroundedLastFrame) || ignoreOneWayPlatformsThisFrame)
                mask &= ~oneWayPlatformMask;

            for (var i = 0; i < totalVerticalRays; i++) {
                var ray = new Vector2(initialRayOrigin.x + i * _horizontalDistanceBetweenRays, initialRayOrigin.y);

                DrawRay(ray, rayDirection * rayDistance, Color.red);
                _raycastHit = Physics2D.Raycast(ray, rayDirection, rayDistance, mask);
                if (_raycastHit) {
                    // set our new deltaMovement and recalculate the rayDistance taking it into account
                    deltaMovement.y = _raycastHit.point.y - ray.y;
                    rayDistance = Mathf.Abs(deltaMovement.y);

                    // remember to remove the skinWidth from our deltaMovement
                    if (isGoingUp) {
                        deltaMovement.y -= skinWidth;
                        collisionState.Above = true;
                    }
                    else {
                        deltaMovement.y += skinWidth;
                        collisionState.Below = true;
                    }

                    _raycastHitsThisFrame.Add(_raycastHit);

                    // this is a hack to deal with the top of slopes. if we walk up a slope and reach the apex we can get in a situation
                    // where our ray gets a hit that is less then skinWidth causing us to be ungrounded the next frame due to residual velocity.
                    if (!isGoingUp && deltaMovement.y > 0.00001f)
                        _isGoingUpSlope = true;

                    // we add a small fudge factor for the float operations here. if our rayDistance is smaller
                    // than the width + fudge bail out because we have a direct impact
                    if (rayDistance < skinWidth + KSkinWidthFloatFudgeFactor)
                        break;
                }
            }
        }


        /// <summary>
        /// checks the center point under the BoxCollider2D for a slope. If it finds one then the deltaMovement is adjusted so that
        /// the player stays grounded and the slopeSpeedModifier is taken into account to speed up movement.
        /// </summary>
        /// <param name="deltaMovement">Delta movement.</param>
        private void HandleVerticalSlope(ref Vector3 deltaMovement) {
            // slope check from the center of our collider
            var centerOfCollider = (_raycastOrigins.BottomLeft.x + _raycastOrigins.BottomRight.x) * 0.5f;
            var rayDirection = -Vector2.up;

            // the ray distance is based on our slopeLimit
            var slopeCheckRayDistance = _slopeLimitTangent * (_raycastOrigins.BottomRight.x - centerOfCollider);

            var slopeRay = new Vector2(centerOfCollider, _raycastOrigins.BottomLeft.y);
            DrawRay(slopeRay, rayDirection * slopeCheckRayDistance, Color.yellow);
            _raycastHit = Physics2D.Raycast(slopeRay, rayDirection, slopeCheckRayDistance, platformMask);
            if (_raycastHit) {
                // bail out if we have no slope
                var angle = Vector2.Angle(_raycastHit.normal, Vector2.up);
                if (angle == 0)
                    return;

                // we are moving down the slope if our normal and movement direction are in the same x direction
                var isMovingDownSlope = Mathf.Sign(_raycastHit.normal.x) == Mathf.Sign(deltaMovement.x);
                if (isMovingDownSlope) {
                    // going down we want to speed up in most cases so the slopeSpeedMultiplier curve should be > 1 for negative angles
                    var slopeModifier = slopeSpeedMultiplier.Evaluate(-angle);
                    // we add the extra downward movement here to ensure we "stick" to the surface below
                    deltaMovement.y += _raycastHit.point.y - slopeRay.y - SkinWidth;
                    deltaMovement = new Vector3(0, deltaMovement.y, 0) +
                                    (Quaternion.AngleAxis(-angle, Vector3.forward) *
                                     new Vector3(deltaMovement.x * slopeModifier, 0, 0));
                    collisionState.MovingDownSlope = true;
                    collisionState.SlopeAngle = angle;
                }
            }
        }

        #endregion
    }
}