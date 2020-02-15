using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierSpawner : Spawner {
    // ======================================================================
    // Field Variables
    // ======================================================================

    // --------------- Serialized Cached References ---------------

    [SerializeField] private GameObject _prefabSoldier = default;

    // ======================================================================
    // Main Loop & MonoBehaviour Methods
    // ======================================================================

    // Update is called once per frame
    protected override void Update() {
        if (CustomTimer.Finished) {
            SpawnNewObj(_prefabSoldier);

            CustomTimer.Duration = Random.Range(
                ConfigUtils.MinSpawnIntervalSoldier, ConfigUtils.MaxSpawnIntervalSoldier);
            CustomTimer.Run();
        }
    }

    // ======================================================================
    // Customised Methods
    // ======================================================================

    protected override void SpawnNewObj(GameObject obj) {
        // always spawn at same y-pos
        SpawnYPos = 5.2f;

        // the SpawnZPos of soldier is originally same as the player's 
        SpawnZPos = -1;
        base.SpawnNewObj(obj);
    }
}