using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSpawner : Spawner {
    // ======================================================================
    // Field Variables
    // ======================================================================

    // --------------- Serialized Cached References ---------------

    [SerializeField] private GameObject _prefabBuff = default;

    // ======================================================================
    // MonoBehaviour Methods
    // ======================================================================

    protected override void Update() {
        if (_timer.Finished) {
            SpawnNewObject(_prefabBuff);

            SpawnInterval = Random.Range(
                //ConfigUtils.MinSpawnIntervalBuff, 
                //ConfigUtils.MaxSpawnIntervalBuff);
                5.5f, 8.0f); // 5.5 > 5 of invincible duration

            _timer.Duration = SpawnInterval;
            _timer.Run();
        }
    }
}