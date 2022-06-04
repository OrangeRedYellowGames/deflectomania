using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Assertions;

public class UnitHealth : MonoBehaviour {
    public FloatVariable HP;

    public VoidEvent DamageEvent;
    public VoidEvent DeathEvent;

    void Awake() {
        Assert.IsNotNull(HP, "HP Variable can't be missing in UnitHealth");
    }

    // Update is called once per frame
    void Update() {
    }
}