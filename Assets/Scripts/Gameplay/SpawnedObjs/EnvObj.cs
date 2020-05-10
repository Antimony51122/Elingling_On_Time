using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// just to add the self destroy functionality to the environment objects
public class EnvObj : SpawnedObj {
    // no implementations, just to avoid the problem caused by abstract classes
    protected override void Start() { }

    protected override void OnDestroy() { }
}
