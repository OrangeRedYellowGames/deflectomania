using Mirror;
using UnityEngine;

/// <summary>
/// Module component that should be attached to any game object that's required to DEAL damage.
/// Examples would be bullet, walls, death planes, etc...
/// 
/// This component should be used in tandem with UnitHealth for any game object that are required to TAKE damage.
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class DamageDealer : NetworkBehaviour {
    /// <summary>
    /// Amount of damage that should be inflicted
    /// </summary>
    public float damageAmount = 10;

    // TODO: Add a flag to determine if damageAmount should be randomized a bit (for example, random damage from 8 to 12)


    /// <summary>
    /// Triggered whenever a collision occurs. If the collision occurs with a game object that has the UnitHealth component
    /// attached to it, it will take damage according to the damageAmount.
    /// </summary>
    /// <param name="other">Collider of the "other" game object</param>
    private void OnCollisionEnter2D(Collision2D other) {
        if (!isServer) {
            return;
        }

        var health = other.gameObject.GetComponent<UnitHealth>();
        if (health) {
            health.TakeDamage(damageAmount);
        }
    }
}