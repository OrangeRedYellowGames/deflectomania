using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthBar : MonoBehaviour {
    public FloatConstant MaxHP;

    private Slider _hpSlider;

    void Awake() {
        // Assert Slider component exists on game object
        _hpSlider = GetComponent<Slider>();
        Assert.IsNotNull(_hpSlider, "HP Slider component missing in HealthBar");

        // Assert unity atom variables are set
        Assert.IsNotNull(MaxHP, "MaxHP Variable cannot be null in HealthBar");

        // Set sliders max value to MaxHP
        _hpSlider.maxValue = MaxHP.Value;
        _hpSlider.value = MaxHP.Value;
    }

    private void Update() {
        // Figure out a better way to do this
        // https://www.reddit.com/r/Unity2D/comments/7kj23o/ui_flipping_child_of_the_player/
        transform.eulerAngles = new Vector3(0, 0, 0);
    }

    // To be called by a unity-atom event listener 
    public void ChangeFillAmount(float amount) {
        if (_hpSlider) {
            _hpSlider.value = amount;
        }
    }
}