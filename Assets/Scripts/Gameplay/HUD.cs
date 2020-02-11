using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    // ======================================================================
    // Field Variables
    // ======================================================================

    [SerializeField] private GameObject[]    _hearts;
    [SerializeField] private TextMeshProUGUI _textMeshProScore;

    // ======================================================================
    // MonoBehaviour Methods
    // ======================================================================

    void Start() {
        //Debug.Log(SceneManager.GetActiveScene().name);

        _textMeshProScore.text = "Distance: " + (int) PlayerStatus.Score;
    }

    void Update() {
        DisplayHearts();
        UpdateStatusFigures();
    }

    // ======================================================================
    // Customised Methods
    // ======================================================================

    // ---------- Display Health Indicators ----------

    private void DisplayHearts() {
        switch (PlayerStatus.Health) {
            case 3:
                break;
            case 2:
                Destroy(_hearts[2]);
                break;
            case 1:
                Destroy(_hearts[1]);
                break;
            case 0:
                Destroy(_hearts[0]);
                break;
            default:
                break;
        }
    }

    // ---------- Display Score TextMeshPro ----------

    private void UpdateStatusFigures() {
        if (_textMeshProScore != null) {
            _textMeshProScore.text = "Distance: " + (int) PlayerStatus.Score;
        }
    }
}