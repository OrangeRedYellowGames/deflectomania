using System;
using UnityEngine;

namespace Player.Scripts {
    public enum PlayerDirection {
        Left,
        Right
    }

    public class PlayerController : MovementMotor2D {
        // Vector Directions, used to change direction in ChangeObjectDirection
        private static readonly Vector3 RightRotationVector = new Vector3(0f, 0f, 0f);
        private static readonly Vector3 LeftRotationVector = new Vector3(0f, 180f, 0f);

        // Methods
        public void ChangeObjectDirection(PlayerDirection playerDirection) {
            switch (playerDirection) {
                case PlayerDirection.Right:
                    transform.rotation = Quaternion.Euler(RightRotationVector);
                    break;
                case PlayerDirection.Left:
                    transform.rotation = Quaternion.Euler(LeftRotationVector);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(playerDirection), playerDirection, null);
            }
        }
    }
}