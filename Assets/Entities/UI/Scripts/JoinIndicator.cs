using DG.Tweening;
using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(TextMeshProUGUI))]
public class JoinIndicator : MonoBehaviour {
    public IntValueList currentPlayers;

    private TextMeshProUGUI _tmp;

    // Start is called before the first frame update
    void Awake() {
        Assert.IsNotNull(currentPlayers);
        _tmp = GetComponent<TextMeshProUGUI>();
    }

    void Start() {
        var sequence = DOTween.Sequence();
        sequence.Append(_tmp.DOFade(0, 1).SetDelay(1.5f, true));
        sequence.SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update() {
        _tmp.enabled = currentPlayers.Count != 4;
    }
}