using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    // ======================================================================
    // Field Variables
    // ======================================================================

    protected CustomTimer CustomTimer;
    protected Vector3     SpawnLocation;
    protected float       SpawnXPos;
    protected float       SpawnYPos = 0;
    protected float       SpawnZPos;

    // ======================================================================
    // Main Loop & MonoBehaviour Methods
    // ======================================================================

    protected virtual void Start() {
        // initially wait for 3s
        CustomTimer          = gameObject.AddComponent<CustomTimer>();
        CustomTimer.Duration = 3.0f;
        CustomTimer.Run();
    }

    protected virtual void Update() { }

    // ======================================================================
    // Customised Methods
    // ======================================================================

    protected virtual void SpawnNewObj(GameObject obj) {
        // the object will always spawn 4 * half screen width away from the player position
        if (PlayerControl.PlayerTransform != null) {
            SpawnXPos = PlayerControl.PlayerTransform.position.x + 4 * ScreenUtils.ScreenRight;
        }

        SpawnLocation = new Vector3(SpawnXPos, SpawnYPos, SpawnZPos);

        Instantiate(obj, SpawnLocation, Quaternion.identity);
    }
}