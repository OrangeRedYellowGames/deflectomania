using UnityEngine;

namespace Utils {
    public class Helpers {
        /// <summary>
        /// Adapted from https://stackoverflow.com/questions/25818897/problems-limiting-object-rotation-with-mathf-clamp
        /// </summary>
        /// <param name="currentValue"></param>
        /// <param name="minAngle"></param>
        /// <param name="maxAngle"></param>
        /// <param name="clampAroundAngle"></param>
        /// <returns></returns>
        public static float ClampAngle(
            float currentValue,
            float minAngle,
            float maxAngle,
            float clampAroundAngle = 0
        ) {
            float angle = currentValue - (clampAroundAngle + 180);

            while (angle < 0) {
                angle += 360;
            }

            angle = Mathf.Repeat(angle, 360);

            return Mathf.Clamp(
                angle - 180,
                minAngle,
                maxAngle
            ) + 360 + clampAroundAngle;
        }
    }
}