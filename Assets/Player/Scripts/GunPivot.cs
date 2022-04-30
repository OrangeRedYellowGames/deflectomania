using System;
using NLog;
using UnityAtoms.BaseAtoms;
using UnityAtoms.InputSystem;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using Utils;
using Logger = NLog.Logger;

// https://www.youtube.com/watch?v=6hp9-mslbzI
namespace Player.Scripts {
    public class GunPivot : MonoBehaviour {
        [SerializeField] private Vector2Variable LookPosition;
        [SerializeField] private PlayerInputEvent DeviceChangeEvent;
        private GameObject _player;
        private float _rotOffset;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private bool usingMouse = true;

        private void DeviceChanged(PlayerInput input) {
            if (input.currentControlScheme == "Keyboard&Mouse") {
                usingMouse = true;
            }
            else {
                usingMouse = false;
            }
        }

        private void Awake() {
            DeviceChangeEvent.Register(DeviceChanged);

            Transform objTransform = transform;

            // Get the player's transform, needed to figure out which direction the player is facing
            try {
                _player = objTransform.parent.gameObject;
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
                gunTransform = objTransform.GetChild(0);
            }
            catch (Exception) {
                Logger.Error("GunPivot couldn't find the gun attached to it");
                throw;
            }

            var firePoint = gunTransform.Find("FirePoint");
            if (!firePoint) {
                throw new NullReferenceException(
                    "GunPivot couldn't find the \"FirePoint\" Transform of its attached gun");
            }

            // Find the angle between the fire point and the gun pivot and use that as an offset
            Vector3 dif = (firePoint.position - objTransform.position);
            dif.Normalize();
            _rotOffset = Mathf.Atan2(dif.y, dif.x) * Mathf.Rad2Deg;
        }

        // Read this https://docs.unity3d.com/ScriptReference/Quaternion-eulerAngles.html
        void FixedUpdate() {
            Vector3 difference;
            if (usingMouse) {
                // Get the difference between the current mouse position and the pivot's transform
                difference = CameraUtils.MainCamera.ScreenToWorldPoint(LookPosition.Value) -
                             transform.position;
                difference.Normalize();
            }
            else {
                difference = LookPosition.Value;
            }

            // Calculate the rotation / angle from the +ve x axis and scale it to 0 - 360
            // https://stackoverflow.com/questions/30324015/mathf-atan2-return-incorrect-result/30325326
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            rotationZ = (rotationZ + 360) % 360;

            // If the player is facing right
            if (_player.transform.eulerAngles.y == 0f) {
                rotationZ -= _rotOffset;
                // Clamp weapon at top, check if mouse in Q2
                if (rotationZ > 90f && rotationZ < 180f) {
                    transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
                }
                // Clamp weapon at bottom, check if mouse is in Q3
                else if (rotationZ > 180f && rotationZ < 270) {
                    transform.localRotation = Quaternion.Euler(0f, 0f, 270f);
                }
                // If mouse is in Q1 or Q4
                else {
                    transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
                }
            }
            // if player is facing left
            else if (Mathf.Abs(_player.transform.eulerAngles.y) == 180f) {
                rotationZ += _rotOffset;
                // Clamp weapon at top, check if mouse is in Q1
                if (rotationZ < 90f && rotationZ > 0f) {
                    transform.localRotation = Quaternion.Euler(180f, 180f, -90f);
                }
                // Clamp weapon at bottom, check if mouse is in Q4
                else if (rotationZ > 270f && rotationZ < 360f) {
                    transform.localRotation = Quaternion.Euler(180f, 180f, -270f);
                }
                // If mouse is in Q2 or Q3
                else {
                    transform.localRotation = Quaternion.Euler(180f, 180f, -rotationZ);
                }
            }
        }
    }
}