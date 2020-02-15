using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BicycleBuff : SpawnedObj {
    // ======================================================================
    // Field Variables
    // ======================================================================

    private bool _isMissed = false;

    // ======================================================================
    // Main Loop & MonoBehaviour Methods
    // ======================================================================

    protected override void Start() {
        UnityEvents.Add(EventName.SpeedUpActivatedEvent, new SpeedUpActivatedEvent());
        EventManager.AddFloatArgInvoker(EventName.SpeedUpActivatedEvent, this);
    }

    protected override void Update() {
        if (!_isMissed && transform.position.x < PlayerControl.PlayerTransform.position.x) {
            // add to buff missed count when the buff is behind the player
            PlayerStatus.BuffMissedCount++;

            // set isMissed to true to prevent duplications of counting
            _isMissed = true;
        }

        base.Update();
    }

    protected override void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.CompareTag("Player")) {
            // TODO: this float argument here is actually unused, make it useful
            UnityEvents[EventName.SpeedUpActivatedEvent].Invoke(ConfigUtils.BuffDuration);

            // add to buff collected count when the buff destroys due to being collected
            PlayerStatus.BuffCollectedCount++;

            // buff object disappears after the player collects it
            Destroy(gameObject);
        }
    }

    protected override void OnDestroy() {
        EventManager.RemoveFloatArgInvoker(EventName.SpeedUpActivatedEvent, this);
    }
}