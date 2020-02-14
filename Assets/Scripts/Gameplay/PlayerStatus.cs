using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour {
    // ==============================================================
    // Field Variables
    // ==============================================================

    // --------------- Serialized Fields ---------------

    [SerializeField] private Animator _animator;

    // --------------- Fields to be attached Component Instances ---------------

    private BoxCollider2D _boxColl2D;

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
        _boxColl2D = GetComponent<BoxCollider2D>();

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

    // determine the player sprite's z-pos by comparing with the colliding vehicle objects
    // this is to avoid the weird posture of the player sprite behind the vehicle sprite when
    // the player looks actually closer to the Camera
    void OnTriggerEnter2D(Collider2D coll) {
        // when the Player's center y-pos is lower than
        // - Bus's center y-pos by more than 1 unit
        // - Car's center y-pos by more than 0.25 units
        if ((coll.gameObject.name == "Bus(Clone)" &&
             transform.position.y < coll.transform.position.y - 1.0f) ||
            (coll.gameObject.name == "Car(Clone)" &&
             transform.position.y < coll.transform.position.y - 0.25f)) {
            // set z-pos of player to closer than the vehicles
            transform.position = new Vector3(transform.position.x, transform.position.y, -3);
        }
    }

    // set the player's z-pos back to original when exit the vehicle's collider
    void OnTriggerExit2D(Collider2D coll) {
        Debug.Log(coll.gameObject.name);
        if (coll.gameObject.CompareTag("Vehicle")) {
            transform.position = new Vector3(transform.position.x, transform.position.y, -1);
        }
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
        _boxColl2D.isTrigger = false;

        // start the buff timer and exit buffed mode after buff duration
        _buffTimer.Run();
    }

    // callback this function when buff timer finished
    private void HandleBuffTimerFinishedEvent() {
        Invincible = false;

        // change the sprite animation to riding bicycle in the animator
        _animator.SetBool("OnBicycle", Invincible);

        // set isTrigger property back to true thus the player won't physically interact with vehicles
        _boxColl2D.isTrigger = true;

        // Player movement speed set back to normal state
        PlayerControl.HoriMvtState = HoriMvtState.Normal;
    }

    // 
    private void HandleGameOverEvent(float unused) {
        // TODO: game over menu
        //Debug.Log("game over"); // check whether invoker is working correctly
    }
}