using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour {
    // ==============================================================
    // Field Variables
    // ==============================================================

    public static float Health;
    public static int Score;

    // ======================================================================
    // Main Loop & MonoBehaviour Methods
    // ======================================================================

    void Start() {
        // initialise player health with 3 hearts
        Health = 3;

        EventManager.AddFloatArgListener(EventName.HealthChangedEvent,    HandleHealthChangedEvent);
        EventManager.AddFloatArgListener(EventName.SpeedUpActivatedEvent, HandleSpeedUpEffectEvent);
        EventManager.AddFloatArgListener(EventName.GameOverEvent,         HandleGameOverEvent);
    }

    void Update() {
        // 5 is the initial distance the player was away from the origin
        Score = (int)transform.position.x + 5;
    }

    // process trigger collisions with other game objects
    void OnTriggerEnter2D(Collider2D otherColl) { }

    // ======================================================================
    // Customised Methods
    // ======================================================================

    // reduces health by the given damage
    private void HandleHealthChangedEvent(float damage) {
        // don't go below zero in health
        Health = Mathf.Max(0, Health - damage);
    }

    // boost the player movement speed and turn invincible
    private void HandleSpeedUpEffectEvent(float factor) {
        // TODO: change the sprite

    }

    //
    private void HandleGameOverEvent(float unused) { }
}