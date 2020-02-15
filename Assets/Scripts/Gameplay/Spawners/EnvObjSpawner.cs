using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvObjSpawner : Spawner {
    // ======================================================================
    // Field Variables
    // ======================================================================

    // --------------- Serialized Cached References ---------------

    [SerializeField] private GameObject[] _prefabEnvObjs = default;

    // --------------- Config Params ---------------

    private VehicleLane _vehicleLane;

    private List<KeyValuePair<GameObject, float>> _envObjs =
        new List<KeyValuePair<GameObject, float>>();

    private List<KeyValuePair<VehicleLane, float>> _laneChoices =
        new List<KeyValuePair<VehicleLane, float>>();
    // ======================================================================
    // Main Loop & MonoBehaviour Methods
    // ======================================================================

    protected override void Start() {
        _envObjs = new List<KeyValuePair<GameObject, float>> {
            new KeyValuePair<GameObject, float>(_prefabEnvObjs[0], 60),
            new KeyValuePair<GameObject, float>(_prefabEnvObjs[1], 20),
            new KeyValuePair<GameObject, float>(_prefabEnvObjs[2], 20),
        };

        _laneChoices = new List<KeyValuePair<VehicleLane, float>> {
            new KeyValuePair<VehicleLane, float>(VehicleLane.Top, 20),
            new KeyValuePair<VehicleLane, float>(VehicleLane.Bottom, 80),
        };

        base.Start();
    }

    protected override void Update() {
        if (CustomTimer.Finished) {
            // using reusable separate function from Probability Utility class
            SpawnNewObj(Probability.RandomEventsWithProb(_envObjs, 100));

            // when in buffed state, spawn the obj at 3 times frequency
            CustomTimer.Duration = 2;
            CustomTimer.Run();
        }
    }

    // ======================================================================
    // Customised Methods
    // ======================================================================

    protected override void SpawnNewObj(GameObject obj) {
        // 50-50 random
        //int enumLen = System.Enum.GetNames(typeof(VehicleLane)).Length;
        //_vehicleLane = (VehicleLane) Random.Range(0, enumLen);

        // we would like the env obj to spawn at bottom more
        _vehicleLane = Probability.RandomEventsWithProb(_laneChoices, 100);

        // the SpawnZPos of env obj on top is -1, bot is -5.5 to be closer than bicycles
        if (_vehicleLane == VehicleLane.Top) {
            SpawnYPos = 6.30f;
            SpawnZPos = -0.5f;
        } else {
            SpawnYPos = -4.75f;
            SpawnZPos = -5.5f;
        }

        base.SpawnNewObj(obj);
    }
}