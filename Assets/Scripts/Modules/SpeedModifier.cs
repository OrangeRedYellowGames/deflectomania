using UnityEngine;

/// <summary>
/// Module component that should be attached to any game object that's required to modify the speed of any other game object.
/// Examples would be bullet, walls, death planes, etc...
/// 
/// This component should be used in tandem with UnitSpeed for any game object that are required to modification to its speed.
/// </summary>
public class SpeedModifier : MonoBehaviour {
    /// <summary>
    /// Value of the speed modification.
    /// </summary>
    public float modificationAmount = 1.2f;

    /// <summary>
    /// Triggered whenever a collision occurs. If the collision occurs with a game object that has the UnitSpeed component
    /// attached to it, it will take modify its speed according to the modicationAmount.
    /// </summary>
    /// <param name="other">Collider of the "other" game object</param>
    private void OnCollisionEnter2D(Collision2D other) {
        var speed = other.gameObject.GetComponent<UnitSpeed>();
        if (speed) {
            speed.ModifySpeed(modificationAmount);
        }
    }
}