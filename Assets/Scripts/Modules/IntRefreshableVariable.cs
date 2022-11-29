using UnityAtoms.BaseAtoms;
using UnityEngine;

/// <summary>
/// Modular component that should be attached to any game object that wishes to expose variables / atoms that can be refreshed.
///
/// Should be used in tandem with the OnCollisionRefresher component in order refresh any attached variables / atoms.
/// </summary>
public class IntRefreshableVariable : MonoBehaviour {
    [Tooltip("Variable whose value should be refreshed")]
    public IntVariable atomVariable;

    [Tooltip("Variable Instancer whose value should be refreshed")]
    public IntVariableInstancer atomVariableInstancer;

    /// <summary>
    /// Use this function to refresh the value of attached unity atom.
    /// </summary>
    public void Refresh() {
        if (atomVariable) {
            atomVariable.Reset();
        }

        if (atomVariableInstancer) {
            atomVariableInstancer.Value = atomVariableInstancer.Base.InitialValue;
        }
    }
}