using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZPosChangeable : FloatEventInvoker {
    // determine the player and soldier sprite's z-pos by comparing with the colliding vehicle objects
    // this is to avoid the weird posture of the player sprite behind the vehicle sprite when the
    // player or the soldier looks actually closer to the Camera
    protected virtual void OnTriggerEnter2D(Collider2D coll) {
        // only affect the player and the soldiers
        if (gameObject.name == "Player" || gameObject.name == "Soldier(Clone)") {
            // when the Player's or Soldier's center y-pos is lower than
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
    }

    // set the player's z-pos back to original when exit the vehicle's collider
    void OnTriggerExit2D(Collider2D coll) {
        // only affect the player and the soldiers
        if (gameObject.name == "Player" || gameObject.name == "Soldier(Clone)") {
            Debug.Log(coll.gameObject.name);
            if (coll.gameObject.CompareTag("Vehicle")) {
                transform.position = new Vector3(transform.position.x, transform.position.y, -1);
            }
        }
    }
}