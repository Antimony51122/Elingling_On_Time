using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultsUI : MonoBehaviour {
    // ======================================================================
    // Field Variables
    // ======================================================================

    [SerializeField] private TextMeshProUGUI _textMeshProScore2;
    [SerializeField] private TextMeshProUGUI _textMeshTime;
    [SerializeField] private TextMeshProUGUI _textMeshProBuffCollected;
    [SerializeField] private TextMeshProUGUI _textMeshProBuffMissed;

    // Start is called before the first frame update
    void Start() {
        _textMeshProScore2.text        = GameSession.ScoreResult.ToString();
        _textMeshTime.text             = "0:24";
        _textMeshProBuffCollected.text = GameSession.BuffCollectedResult.ToString();
        _textMeshProBuffMissed.text    = GameSession.BuffMissedResult.ToString();

        // update the score result in the leaderboard 
        FacebookAndPlayFabManager.Instance.UpdateStat(Constants.LeaderboardName, GameSession.ScoreResult);
    }

    // Update is called once per frame
    void Update() { }
}