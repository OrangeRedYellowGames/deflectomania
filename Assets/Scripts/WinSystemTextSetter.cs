using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(TextMeshProUGUI))]
public class WinSystemTextSetter : MonoBehaviour {
    public IntEvent playerWinEvent;

    private TextMeshProUGUI _tmp;

    void Awake() {
        Assert.IsNotNull(playerWinEvent);
        _tmp = GetComponent<TextMeshProUGUI>();
        playerWinEvent.Register(SetText);
    }

    private void OnDestroy() {
        playerWinEvent.Unregister(SetText);
    }

    // Start is called before the first frame update
    void SetText(int winnerIndex) {
        _tmp.enabled = true;

        switch (winnerIndex) {
            case -1:
                _tmp.SetText("Draw!");
                break;
            default:
                _tmp.SetText($"Player {(winnerIndex + 1).ToString()} Won!");
                break;
        }
    }
}