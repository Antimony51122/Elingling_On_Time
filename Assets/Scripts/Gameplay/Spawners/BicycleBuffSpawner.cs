using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BicycleBuffSpawner : Spawner {
    // ======================================================================
    // Field Variables
    // ======================================================================

    // --------------- Serialized Cached References ---------------

    [SerializeField] private GameObject _prefabBicycleBuff = default;

    // ======================================================================
    // Main Loop & MonoBehaviour Methods
    // ======================================================================

    // Update is called once per frame
    protected override void Update()
    {
        if (CustomTimer.Finished) {
            SpawnNewObj(_prefabBicycleBuff);

            CustomTimer.Duration = Random.Range(
                ConfigUtils.MinSpawnIntervalBuff, ConfigUtils.MaxSpawnIntervalBuff);
            CustomTimer.Run();
        }
    }

    // ======================================================================
    // Customised Methods
    // ======================================================================

    protected override void SpawnNewObj(GameObject obj) {
        // always spawn at same y-pos
        SpawnYPos = -4.2f;

        // the SpawnZPos of bicycle is closer than the player's 
        SpawnZPos = -5;
        base.SpawnNewObj(obj);
    }
}
