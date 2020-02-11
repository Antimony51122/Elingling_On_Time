using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    // ======================================================================
    // Field Variables
    // ======================================================================

    // --------------- Serialized Cached References ---------------

    // --------------- Fields to be attached Component Instances ---------------

    protected Timer _timer;

    // --------------- Config Params ---------------

    protected Vector3 SpawnLocation;
    protected float   SpawnZPos;
    protected float   SpawnInterval;

    // ======================================================================
    // MonoBehaviour Methods
    // ======================================================================

    void Start() {
        SpawnZPos = -2;

        _timer          = gameObject.AddComponent<Timer>();
        _timer.Duration = 3.5f;
        _timer.Run();
    }

    protected virtual void Update() { }

    // ======================================================================
    // Customised Methods
    // ======================================================================

    protected virtual void SpawnNewObject(GameObject obj) {
        // the obstacle cannot spawn in the boundary area
        float yPos = Random.Range(ScreenUtils.ScreenBottom + 2, ScreenUtils.ScreenTop - 2);
        float xPos = PlayerControl.PlayerTransform.position.x + 4 * ScreenUtils.ScreenRight;

        SpawnLocation = new Vector3(xPos, yPos, SpawnZPos);

        Instantiate(obj, SpawnLocation, Quaternion.identity);
    }
}