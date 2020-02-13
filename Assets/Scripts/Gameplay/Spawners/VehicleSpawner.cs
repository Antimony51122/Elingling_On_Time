using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawner : Spawner {
    // ======================================================================
    // Field Variables
    // ======================================================================

    // --------------- Serialized Cached References ---------------

    [SerializeField] private GameObject[] _prefabVehicles = default;



    // ======================================================================
    // Main Loop & MonoBehaviour Methods
    // ======================================================================

    protected override void Update() {
        if (CustomTimer.Finished) {
            SpawnNewObj(_prefabVehicles[Random.Range(0, _prefabVehicles.Length)]);

            // when in buffed state, spawn the obj at 3 times frequency
            CustomTimer.Duration = PlayerControl.HoriMvtState == HoriMvtState.Buffed
                ? Random.Range(
                    ConfigUtils.MinSpawnIntervalObstacle / 3,
                    ConfigUtils.MaxSpawnIntervalObstacle / 3)
                : Random.Range(
                    ConfigUtils.MinSpawnIntervalObstacle,
                    ConfigUtils.MaxSpawnIntervalObstacle);
            CustomTimer.Run();
        }
    }

    // ======================================================================
    // Customised Methods
    // ======================================================================

    protected override void SpawnNewObj(GameObject obj) {
        // the SpawnZPos of vehicles is further than either the player's or the street lights
        SpawnZPos = -2;
        base.SpawnNewObj(obj);
    }
}