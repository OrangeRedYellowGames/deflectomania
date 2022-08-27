using System;
using NLog;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Assertions;
using Utils;
using Logger = NLog.Logger;

// https://www.youtube.com/watch?v=6hp9-mslbzI
namespace Player.Scripts {
    public class GunPivot : MonoBehaviour {
        #region Properties

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Raw input of where the player is looking.
        /// Can be a Mouse Position or a normalized direction vector from a gamepad's left / right sticks.
        /// How is variable is interpreted depended on whether the isUsingMouse bool is true or not.
        /// </summary>
        [SerializeField]
        public Vector2Reference lookDirection;

        /// <summary>
        /// Whether lookDirection should be treated as a raw mouse position or a normalized direction vector.
        /// </summary>
        [SerializeField]
        public BoolReference isUsingMouse;

        /// <summary>
        /// Contains the player gameobject. Used to figure out which direction the player is facing
        /// TODO: Replace with a unity atom event or variable
        /// </summary>
        private GameObject _player;

        /// <summary>
        /// Transform of the location where bullets are fired.
        /// </summary>
        private Transform _firePoint;


        /// <summary>
        /// The target vector relative to the gunpivot.
        /// </summary>
        private Vector3 _target;

        /// <summary>
        /// The angle between the gun pivot (or the firepoint) and the mouse cursor
        /// </summary>
        private float _angle;

        /// <summary>
        /// Same as _angle but with values clamped to prevent the gun from going inside the player.
        /// </summary>
        private float _clamped;

        /// <summary>
        /// Distance between the pivot and the attached gun's firepoint.
        /// </summary>
        private float _distanceFromPivotToFirepoint;

        /// <summary>
        /// Distance between the pivot and the mouse. Calculated on each frame.
        /// </summary>
        private float _distanceFromPivotToMouse;

        #endregion

        private void Awake() {
            // Get the player's transform, needed to figure out which direction the player is facing
            try {
                _player = transform.parent.gameObject;
            }
            catch (Exception) {
                Logger.Error("An error occured when trying to find the parenting Player object of the GunPivot");
                throw;
            }

            Assert.IsTrue(_player.CompareTag("Player"),
                "Gun Pivot object must be the direct child of an object with the tag \"Player\"");

            // Get the FirePoint's transform, needed to calculate the rotation offset of the GunPivot
            Transform gunTransform;
            try {
                gunTransform = transform.GetChild(0);
            }
            catch (Exception) {
                Logger.Error("GunPivot couldn't find the gun attached to it");
                throw;
            }

            _firePoint = gunTransform.Find("FirePoint");
            if (!_firePoint) {
                throw new NullReferenceException(
                    "GunPivot couldn't find the \"FirePoint\" Transform of its attached gun");
            }

            // Assign the distance between the pivot and the attached gun's firepoint.
            // Explanation is below in the Update function.
            _distanceFromPivotToFirepoint = Vector3.Distance(transform.position, _firePoint.position);
        }

        // Read this https://docs.unity3d.com/ScriptReference/Quaternion-eulerAngles.html
        void Update() {
            if (isUsingMouse.Value) {
                var pivotPosition = transform.position;

                // Transform the mouse position into world coordinates
                // Note: the Z index will have the same value as that of the camera. To get everything working as inteded,
                // we're going to reset the mouse position Z axis to that of the pivot
                var mousePosition = CameraUtils.MainCamera.ScreenToWorldPoint(lookDirection.Value);
                mousePosition.z = pivotPosition.z;

                // Calculate the vector between the mouse cursor and the pivot.
                // Note that this will result in the gun pivot pointing at the mouse, not the actual gun's firepoint.
                // This will result in bullets being offset when firing. 
                _target = mousePosition - pivotPosition;

                // To solve the bullets being offset issue, we need to take into account the firepoint's position.
                // This is easily solved by rotating the target vector we calculated above by the angle between
                // the mouse and the firepoint. Or, we add to the target vector above the vector between the mouse and the firepoint
                // and calculate the overall angle later. This can be done by the following equation:
                // target = (mousePosition - pivotPosition) + (mousePosition - _firepoint.position);
                // However, there's a small caveat. There's a "grey area" between the gunpivot and the firepoint where everything goes nuts
                // if the mouse is located there which causes the weapon to oscillate between two rotation values. To solve this issue,
                // we create a "circle" around the gunpivot with the distance between it and the firepoint being the radius.
                // If the mouse is located OUTSIDE of this circle, we TAKE into account the vector between the mouse and the firepoint.
                // If the mouse is located INSIDE of this circle, we IGNORE the vector between the mouse and the firerpoint.
                _distanceFromPivotToMouse = Vector3.Distance(gameObject.transform.position, mousePosition);

                // If mouse is located outside the circle
                if (_distanceFromPivotToMouse > _distanceFromPivotToFirepoint) {
                    _target += mousePosition - _firePoint.position;
                }

                // Normalize the vector so that the atan2 call below doesn't give incorrect results
                _target.Normalize();
            }
            else {
                // Don't do anything if the stick is in the middle. Keeps the last direction the player pointed at.
                if (lookDirection.Value.x == 0 && lookDirection.Value.y == 0) {
                    return;
                }

                // Should already be normalized from -1 to 1
                // NOTE: This does not take into account the firepoint. What this means is using analog sticks, we're actually moving
                // the gunpivot not the firepoint resulting in bullets being offset a bit. From testing, this is not that noticeable.
                _target = lookDirection.Value;
            }

            _angle = Mathf.Atan2(_target.y, _target.x) * Mathf.Rad2Deg;
            // If the player is facing right
            if (_player.transform.eulerAngles.y == 0f) {
                _clamped = Helpers.ClampAngle(_angle, -90, 90);

                transform.rotation = Quaternion.Euler(0f, 0f, _clamped);
            }
            else {
                // Clamp around the 180 angle, which is to the far left
                _clamped = Helpers.ClampAngle(_angle, -90, 90, 180);

                // 180 Degrees rotation in the X axis + using negative values in the Z axis
                // are needed due to player rotation being done by rotating the player object around the Y axis by 180 degrees.
                transform.rotation = Quaternion.Euler(180, 0, -_clamped);
            }
        }
    }
}