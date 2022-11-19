using UnityEngine;

/// <summary>
/// Module component that should be attached to any game object that's required to refresh a variable in another game object
/// Example:
/// - A zone that refreshes the skills of any player that pass through
/// - Deflection shield that refreshes the amount of reflection of any colliding bullets
/// 
/// This component should be used in tandem with the RefreshableVariable module for any game object that are requires a variable to be refreshed.
/// </summary>
public class OnCollisionRefresher : MonoBehaviour {
    /// <summary>
    /// Triggered whenever a collision occurs. If the collision occurs with a game object that has the Refreshable component
    /// attached to it, its respective value will be refreshed
    /// </summary>
    /// <param name="other">Collider of the "other" game object</param>
    private void OnCollisionEnter2D(Collision2D other) {
        var refreshableValue = other.gameObject.GetComponent<IntRefreshableVariable>();
        if (refreshableValue) {
            refreshableValue.Refresh();
        }
    }
}