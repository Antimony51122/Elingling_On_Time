using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScorePage : MonoBehaviour {
    // ======================================================================
    // Field Variables
    // ======================================================================

    [SerializeField] private TextMeshProUGUI _textMeshProScore2;
    [SerializeField] private TextMeshProUGUI _textMeshTime;
    [SerializeField] private TextMeshProUGUI _textMeshProBuffCollected;
    [SerializeField] private TextMeshProUGUI _textMeshProBuffMissed;

    // ==============================================================
    // Main Loop & MonoBehaviour Methods
    // ==============================================================
    void Start() {
        _textMeshProScore2.text        = GameSession.ScoreResult.ToString();
        _textMeshTime.text             = GameSession.TimeResult.ToString();
        _textMeshProBuffCollected.text = GameSession.BuffCollectedResult.ToString();
        _textMeshProBuffMissed.text    = GameSession.BuffMissedResult.ToString();
    }

    void Update() { }

    // ======================================================================
    // Customised Methods
    // ======================================================================

    public void HandleReplayButtonOnClickEvent() {
        DestroyGameSession();
        MenuManager.GoToMenu(MenuName.Gameplay);
    }

    public void HandleBackToMainMenuButtonOnClickEvent() {
        DestroyGameSession();
        MenuManager.GoToMenu(MenuName.MainMenu);
    }

    private void DestroyGameSession() {
        if (FindObjectOfType<GameSession>() != null) {
            // destroy the current Game Session when restarting the game
            FindObjectOfType<GameSession>().ResetGame();
        }
    }
}