using UnityEngine;

public class DamageDealer : MonoBehaviour {
    public float damageAmount = 10;

    private void OnCollisionEnter2D(Collision2D other) {
        UnitHealth health = other.gameObject.GetComponent<UnitHealth>();
        if (health) {
            health.HP.Subtract(damageAmount);
        }
    }
}