﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Initializes the game
public class GameInitializer : MonoBehaviour {
    // Awake is called before Start
    void Awake() {
        // initialise the screen utils
        ScreenUtils.Initialize();

        // initialise the config utils
        // Beware: build on phone device has problem reading streaming assets
        ConfigUtils.Initialize();

        // initialise all event handling functionality
        EventManager.Initialize();
    }
}