using UnityEngine;
using UnityEngine.Assertions;

public class BulletSpawner : MonoBehaviour {
    public GameObject bullet;
    public Transform firePoint;

    public float spawnInterval = 1f;

    private float _remainingTime;

    void Awake() {
        Assert.IsNotNull(bullet);
        Assert.IsNotNull(firePoint);
    }

    private void Start() {
        _remainingTime = spawnInterval;
    }

    private void FixedUpdate() {
        _remainingTime -= Time.deltaTime;

        if (_remainingTime <= 0.0f) {
            Instantiate(bullet, firePoint.position,
                Quaternion.Euler(0, 0, firePoint.eulerAngles.z + Random.Range(-10, 10)));
            _remainingTime = spawnInterval;
        }
    }
}