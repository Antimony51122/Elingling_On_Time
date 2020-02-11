using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {
    public void LoadStartScene() {
        DestroyGameSession();
        SceneManager.LoadScene(0);
    }

    public void LoadGameScene() {
        DestroyGameSession();
        SceneManager.LoadScene(1);
    }

    public void LoadHistoryStatsScene() {
        DestroyGameSession();
        SceneManager.LoadScene(4);
    }

    private void DestroyGameSession() {
        if (FindObjectOfType<GameSession>() != null) {
            // destroy the current Game Session when restarting the game
            FindObjectOfType<GameSession>().ResetGame();
        }
    }
}