using UnityEngine;

namespace Player.Scripts {
    public class GunPivot : MonoBehaviour {
        // Update is called once per frame
        void FixedUpdate() {
            Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            difference.Normalize();
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, rotationZ);
            if (rotationZ < -90 || rotationZ > 90) {
                
            }
        }
    }
}