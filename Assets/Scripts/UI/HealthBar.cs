using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthBar : MonoBehaviour {
    /// <summary>
    /// Maximum HP value. Will be set as the initial value of the health bar.
    /// </summary>
    public FloatConstant maxHp;

    /// <summary>
    /// Float Event to listen to.
    /// </summary>
    public FloatEventReference healthChangedEvent;

    private Slider _hpSlider;

    void Awake() {
        // Assert Slider component exists on game object
        _hpSlider = GetComponent<Slider>();
        Assert.IsNotNull(_hpSlider, "HP Slider component missing in HealthBar");

        // Asserts HealthChangedEvent is set
        Assert.IsNotNull(healthChangedEvent, "HealthChangedEvent is missing in HealthBar");
        healthChangedEvent.Event.Register(ChangeFillAmount);

        // Assert unity atom variables are set
        Assert.IsNotNull(maxHp, "MaxHP Variable cannot be null in HealthBar");

        // Set sliders max value to MaxHP
        _hpSlider.maxValue = maxHp.Value;
        _hpSlider.value = maxHp.Value;
    }

    void OnDestroy() {
        healthChangedEvent.Event.Unregister(ChangeFillAmount);
    }

    private void LateUpdate() {
        // Figure out a better way to do this
        // https://www.reddit.com/r/Unity2D/comments/7kj23o/ui_flipping_child_of_the_player/
        transform.eulerAngles = Vector3.zero;
    }

    /// <summary>
    /// Function that changes the value of the HP bar.
    ///
    /// Should be called whenever the healthChangedEvent is raised.
    /// </summary>
    /// <param name="amount">New value of the HP bar</param>
    public void ChangeFillAmount(float amount) {
        if (_hpSlider) {
            _hpSlider.value = amount;
        }
    }
}