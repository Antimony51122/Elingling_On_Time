using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour {
    // ==============================================================
    // Field Variables
    // ==============================================================

    public static float Health;
    public static int   Score;


    // buff timer
    private CustomTimer _buffTimer;

    // ======================================================================
    // Main Loop & MonoBehaviour Methods
    // ======================================================================

    void Start() {
        // initialise player health with 3 hearts
        Health = 3;

        _buffTimer          = gameObject.AddComponent<CustomTimer>();
        _buffTimer.Duration = ConfigUtils.MinSpawnIntervalBuff;
        _buffTimer.AddTimerFinishedEventListener(HandleBuffTimerFinishedEvent);

        EventManager.AddFloatArgListener(EventName.HealthChangedEvent,    HandleHealthChangedEvent);
        EventManager.AddFloatArgListener(EventName.SpeedUpActivatedEvent, HandleSpeedUpEffectEvent);
        EventManager.AddFloatArgListener(EventName.GameOverEvent,         HandleGameOverEvent);
    }

    void Update() {
        // 5 is the initial distance the player was away from the origin
        Score = (int) transform.position.x + 5;
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

        PlayerControl.HoriMvtState = HoriMvtState.Buffed;

        // start the buff timer and exit buffed mode after buff duration
        _buffTimer.Run();
    }

    //
    private void HandleGameOverEvent(float unused) {
        Debug.Log("game over"); // check whether invoker is working correctly
    }

    private void HandleBuffTimerFinishedEvent() {
        PlayerControl.HoriMvtState = HoriMvtState.Normal;
    }
}