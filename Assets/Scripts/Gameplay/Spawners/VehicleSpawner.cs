using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawner : Spawner {
    // ======================================================================
    // Field Variables
    // ======================================================================

    // --------------- Serialized Cached References ---------------

    [SerializeField] private GameObject[] _prefabVehicles = default;

    private VehicleLane _vehicleLane;

    private float _topLaneTop;
    private float _topLaneBot;
    private float _botLaneTop;
    private float _botLaneBot;

    // ======================================================================
    // Main Loop & MonoBehaviour Methods
    // ======================================================================

    protected override void Start() {
        _topLaneTop = ScreenUtils.ScreenTop - 2;
        _topLaneBot = 2;
        _botLaneTop = -2;
        _botLaneBot = ScreenUtils.ScreenBottom + 3.5f;

        base.Start();

        Debug.Log(CustomTimer);
    }

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
        int enumLen = System.Enum.GetNames(typeof(VehicleLane)).Length;
        _vehicleLane = (VehicleLane) Random.Range(0, enumLen);

        // if VehicleLane is top, spawn on top lane range, otherwise on bot lane range
        SpawnYPos = _vehicleLane == VehicleLane.Top
            ? Random.Range(_topLaneBot, _topLaneTop)
            : Random.Range(_botLaneBot, _botLaneTop);

        // the SpawnZPos of vehicles is further than either the player's or the street lights
        SpawnZPos = -2;

        base.SpawnNewObj(obj);
    }
}