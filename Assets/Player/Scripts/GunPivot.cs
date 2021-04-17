using System;
using UnityEngine;
using UnityEngine.Assertions;
using Utils;

// TODO: Make clamping a variable
// TODO: Make firepoint follow mouse instead of pivot
// https://www.youtube.com/watch?v=6hp9-mslbzI
namespace Player.Scripts {
    public class GunPivot : MonoBehaviour {
        private GameObject _player;
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private void Awake() {
            try {
                _player = transform.parent.gameObject;
            }
            catch (Exception) {
                Logger.Error("An error occured when trying to find the parenting Player object of the GunPivot");
                throw;
            }

            Assert.IsTrue(_player.CompareTag("Player"),
                "Gun Pivot object must be the direct child of an object with the tag \"Player\"");
        }

        // Read this https://docs.unity3d.com/ScriptReference/Quaternion-eulerAngles.html
        void FixedUpdate() {
            // Get the difference between the current mouse position and the pivot's transform
            Vector3 difference = CameraUtils.MainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            difference.Normalize();

            // Calculate the rotation / angle from the +ve x axis and scale it to 0 - 360
            // https://stackoverflow.com/questions/30324015/mathf-atan2-return-incorrect-result/30325326
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            rotationZ = (rotationZ + 360) % 360;

            Logger.Debug(rotationZ);

            // If the player is facing right
            if (_player.transform.eulerAngles.y == 0f) {
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