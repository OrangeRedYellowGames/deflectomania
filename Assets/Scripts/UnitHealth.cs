using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Modular component that should be attached to any game object that requires "health".
///
/// Should be used in tandem with the DamageDealer component in order to inflict damage.
/// </summary>
public class UnitHealth : MonoBehaviour {
    /// <summary>
    /// Variable that holds the HP of the object.
    /// </summary>
    public FloatReference hp;

    /// <summary>
    /// Optional. VoidEvent to be triggered when HP reaches 0.
    /// </summary>
    public IntEvent deathEvent;
    
    /// <summary>
    /// Optional. Player ID that will be raised with the death event.
    /// </summary>
    public IntReference playerId;

    void Awake() {
        Assert.IsNotNull(hp, "HP Variable can't be missing in UnitHealth");
    }

    /// <summary>
    /// Use this function to inflict damage onto the attached object.
    ///
    /// If HP reaches 0, deathEvent will be raised.
    /// </summary>
    /// <param name="damage">Amount of damage to take</param>
    public void TakeDamage(float damage) {
        hp.Value -= damage;
        if (hp.Value <= 0 && deathEvent) {
            deathEvent.Raise(playerId);
        }
    }
}