using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCactus : SpawnedObj {
    // ======================================================================
    // MonoBehaviour Methods
    // ======================================================================

    protected override void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.CompareTag("Player")) {
            _playerStatus.CollisionWithObstacle();
        }
    }
}