using UnityEngine;

namespace Utils {
    public class CameraUtils {
        // Caches Camera.main to optimize calls to it in FSM
        // Adapted from https://forum.unity.com/threads/how-to-cache-the-main-camera-as-a-global-variable.853774/
        private static Camera _mainCamera;

        public static Camera MainCamera {
            get {
                if (!_mainCamera) {
                    _mainCamera = Camera.main;
                }

                return _mainCamera;
            }
        }
    }
}