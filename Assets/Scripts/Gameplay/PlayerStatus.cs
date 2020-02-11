using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatus : MonoBehaviour {
    // ==============================================================
    // Field Variables
    // ==============================================================

    // --------------- Serialized Cached References ---------------


    // --------------- Fields to be attached Component Instances ---------------


    // --------------- Config Params ---------------

    public static int Health;
    private       int _healthMin; // if health lower than this, game fail

    public static int Score;

    // power buff collection and miss count
    public static int BuffCollectedCount;
    public static int BuffMissedCount;

    // public to be accessed by PlayerControl class to add buff
    public static bool Invincible;

    [SerializeField] private const float InvincibleDuration = 5.0f;

    // ==============================================================
    // MonoBehaviour Methods
    // ==============================================================

    void Start() {
        // remember to initialise Health in the Start Method instead of in the fields session
        // or otherwise the static variables will maintain in all scenes
        Health     = 3;
        _healthMin = 0; // if health lower than this, game fail
        Score = 0;
        Invincible = false;

        BuffCollectedCount = 0;
        BuffMissedCount    = 0;
    }

    void Update() {
        // 5 is the initial distance the player was away from the origin
        Score = (int) transform.position.x + 5;

        CheckDeath();
    }

    // ==============================================================
    // Customised Methods
    // ==============================================================


    // ---------- Health Calculations----------


    // ---------- Collision Behaviour ----------

    public void CollisionWithObstacle() {
        // only deduct health when the player is not invincible
        if (!Invincible) {
            Health -= 1;
        }
    }

    public void CollisionWithBuff() {
        // TODO: implement the buff animation change

        // can only turn into invincible mode when not currently in invincible mode
        if (!Invincible) {
            EnterInvincibleMode();
        }
    }

    private void CheckDeath() {
        if (Health == _healthMin) {
            SceneManager.LoadScene(2);

            GameSession.ScoreResult = Score;
            // TODO: implement game timer
            GameSession.BuffCollectedResult = BuffCollectedCount;
            GameSession.BuffMissedResult    = BuffMissedCount;
        }
    }

    private void EnterInvincibleMode() {
        Invincible = true;

        // Player movement speed set to buffed state
        PlayerControl.HoriMvtState = HoriMvtState.Buffed;

        // exit invincible mode after several seconds
        Invoke("ExitInvincibleMode", InvincibleDuration);
    }

    private void ExitInvincibleMode() {
        Invincible = false;

        // Player movement speed set back to normal state
        PlayerControl.HoriMvtState = HoriMvtState.Normal;
    }
}