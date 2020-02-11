using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : Spawner {
    // ======================================================================
    // Field Variables
    // ======================================================================

    // --------------- Serialized Cached References ---------------

    [SerializeField] private GameObject[] _prefabObstacles = default;

    // ======================================================================
    // MonoBehaviour Methods
    // ======================================================================

    protected override void Update() {
        if (_timer.Finished) {
            int prefabIndex = Random.Range(0, 2);
            SpawnNewObject(prefabIndex >= 1 ? _prefabObstacles[0] : _prefabObstacles[1]);

            // when in buffed state, spawn the obj at higher frequency
            SpawnInterval =
                PlayerControl.HoriMvtState == HoriMvtState.Buffed
                    ? Random.Range(
                        // for phone build, config utils will vastly slow down the program speed
                        // thus using directly assigned numbers
                        //ConfigUtils.MinSpawnIntervalObstacleBuffed,
                        //ConfigUtils.MaxSpawnIntervalObstacleBuffed)                        
                        0.15f, 0.3f)
                    : Random.Range(
                        //ConfigUtils.MinSpawnIntervalObstacleNormal,
                        //ConfigUtils.MaxSpawnIntervalObstacleNormal);
                        0.45f, 0.9f);

            _timer.Duration = SpawnInterval;
            _timer.Run();
        }
    }
}