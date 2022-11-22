using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Modular component that should be attached to any game object that requires "speed".
///
/// Should be used in tandem with the SpeedModifier component in order to modify the speed.
/// </summary>
public class UnitSpeed : MonoBehaviour {
    [Tooltip("Variable that holds the Speed of the object.")]
    public FloatReference speed;


    void Awake() {
        Assert.IsNotNull(speed, "Speed Variable can't be missing in UnitSpeed");
    }

    /// <summary>
    /// Use this function to modify the speed of the attached object.
    /// </summary>
    /// <param name="value">Modification value. Can be positive or negative values.</param>
    public void ModifySpeed(float value) {
        speed.Value *= value;
    }
}