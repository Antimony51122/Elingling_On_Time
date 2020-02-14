using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : ZPosChangeable {
    // ==============================================================
    // Field Variables
    // ==============================================================

    // --------------- Serialized Cached References ---------------

    [SerializeField] private Animator _animator;

    // --------------- Fields to be attached Component Instances ---------------

    private CapsuleCollider2D _capColl2D;

    // --------------- Config Params ---------------

    // static fields to describe the player's current condition
    public static float Health;
    public static int   Score;
    public static bool  Invincible;

    // buff timer
    private CustomTimer _buffTimer;

    // ======================================================================
    // Main Loop & MonoBehaviour Methods
    // ======================================================================

    void Start() {
        // initialise player health with 3 hearts
        Health = 3;

        // initialise player invincibility mode with false
        Invincible = false;

        // attach the BoxCollider2D component for later crashing bus and car effects
        _capColl2D = GetComponent<CapsuleCollider2D>();

        _buffTimer          = gameObject.AddComponent<CustomTimer>();
        _buffTimer.Duration = ConfigUtils.BuffDuration;
        _buffTimer.AddTimerFinishedEventListener(HandleBuffTimerFinishedEvent);

        EventManager.AddFloatArgListener(EventName.HealthChangedEvent,    HandleHealthChangedEvent);
        EventManager.AddFloatArgListener(EventName.SpeedUpActivatedEvent, HandleSpeedUpEffectEvent);
        EventManager.AddFloatArgListener(EventName.GameOverEvent,         HandleGameOverEvent);
    }

    void Update() {
        // 5 is the initial distance the player was away from the origin
        Score = (int) transform.position.x + 5;
    }

    // ======================================================================
    // Customised Methods
    // ======================================================================

    // reduces health by the given damage
    private void HandleHealthChangedEvent(float damage) {
        // only deduct health when the player is not invincible
        if (!Invincible) {
            // don't go below zero in health
            Health = Mathf.Max(0, Health - damage);
        }
    }

    // boost the player movement speed and turn invincible
    private void HandleSpeedUpEffectEvent(float factor) {
        // set invincibility mode to true thus player will not deduct health during invincible mode
        Invincible = true;

        // Player movement speed set to buffed state
        PlayerControl.HoriMvtState = HoriMvtState.Buffed;

        // change the sprite animation to riding bicycle in the animator
        _animator.SetBool("OnBicycle", Invincible);

        // set isTrigger property to false so the player can crash away the car and bus
        _capColl2D.isTrigger = false;

        // start the buff timer and exit buffed mode after buff duration
        _buffTimer.Run();
    }

    // callback this function when buff timer finished
    private void HandleBuffTimerFinishedEvent() {
        Invincible = false;

        // change the sprite animation to riding bicycle in the animator
        _animator.SetBool("OnBicycle", Invincible);

        // set isTrigger property back to true thus the player won't physically interact with vehicles
        _capColl2D.isTrigger = true;

        // Player movement speed set back to normal state
        PlayerControl.HoriMvtState = HoriMvtState.Normal;
    }

    // 
    private void HandleGameOverEvent(float unused) {
        // TODO: game over menu
        //Debug.Log("game over"); // check whether invoker is working correctly
    }
}