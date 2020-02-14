using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour {
    // ======================================================================
    // Field Variables
    // ======================================================================

    [SerializeField] private GameObject[]    _hearts;
    [SerializeField] private TextMeshProUGUI _textMeshProScore;

    // ======================================================================
    // Main Loop & MonoBehaviour Methods
    // ======================================================================

    void Start() {
        _textMeshProScore.text = "Distance: " + (int) PlayerStatus.Score;
    }

    // Update is called once per frame
    void Update() {
        DisplayHearts();
        UpdateStatusFigures();
    }

    // ======================================================================
    // Customised Methods
    // ======================================================================

    // ---------- Display Health Indicators ----------

    private void DisplayHearts() {
        if (PlayerStatus.Health < _hearts.Length) {
            Destroy(_hearts[(int) PlayerStatus.Health]);
        }
    }

    // ---------- Display Score TextMeshPro ----------

    private void UpdateStatusFigures() {
        if (_textMeshProScore != null) {
            _textMeshProScore.text = "Distance: " + (int) PlayerStatus.Score;
        }
    }
}