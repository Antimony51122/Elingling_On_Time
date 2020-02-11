using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffLightning : SpawnedObj {
    // ======================================================================
    // Field Variables
    // ======================================================================

    private bool _isMissed = false;

    // ======================================================================
    // MonoBehaviour Methods
    // ======================================================================

    protected override void Update() {
        if (!_isMissed && transform.position.x < PlayerControl.PlayerTransform.position.x) {
            // add to buff missed count when the buff is behind the player
            PlayerStatus.BuffMissedCount++;

            // set isMissed to true to prevent duplications of counting
            _isMissed = true;
        }

        DestroySelf();
    }

    protected override void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.CompareTag("Player")) {
            _playerStatus.CollisionWithBuff();

            // buff object disappears after the player collects it
            Destroy(gameObject);

            // add to buff collected count when the buff destroys due to being collected
            PlayerStatus.BuffCollectedCount++;
        }
    }
}