using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour {
    // ==============================================================
    // Field Variables
    // ==============================================================

    public static int ScoreResult;
    public static int TimeResult;
    public static int BuffCollectedResult;
    public static int BuffMissedResult;

    // ==============================================================
    // MonoBehaviour Methods
    // ==============================================================
    void Awake() {
        // Storing how many Game Status Objects are there
        // Beware this time using plural FindObjectsOfType<>() because there are multiple
        int gameSessionsCount = FindObjectsOfType<GameSession>().Length;

        // gameSessionsCount more than one means this is the second game session
        if (gameSessionsCount > 1) {
            // prevent the issues destroying action come in bit later then activating the game object
            gameObject.SetActive(false);
            Destroy(gameObject); // Destroy "yourself" referring to the current Game Status
        } else {
            DontDestroyOnLoad(gameObject); // Maintain "yourself" if this is the first Game Status
        }
    }

    void Start() {
        ScoreResult         = 0;
        TimeResult          = 0;
        BuffCollectedResult = 0;
        BuffMissedResult    = 0;
    }

    void Update() { }

    // ==============================================================
    // Customised Methods
    // ==============================================================

    // destroy the current Game Status when restarting the game, called from SceneLoader
    public void ResetGame() {
        Destroy(gameObject);
    }
}