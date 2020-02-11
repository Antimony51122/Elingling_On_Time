using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A timer
public class Timer : MonoBehaviour {
    #region Fields

    // timer duration
    private float _totalSeconds = 0;

    // timer execution
    private float _elapsedSeconds = 0;
    private bool  _running        = false;

    // support for Finished property
    private bool _started = false;

    #endregion

    #region Properties

    // Sets the duration of the timer
    // The duration can only be set if the timer isn't currently running
    public float Duration {
        set {
            if (!_running) {
                _totalSeconds = value;
            }
        }
    }

    // Gets whether or not the timer has finished running
    // This property returns false if the timer has never been started
    public bool Finished {
        get { return _started && !_running; }
    }

    // Gets whether or not the timer is currently running
    public bool Running {
        get { return _running; }
    }

    #endregion

    #region Methods

    // Update is called once per frame
    void Update() {
        // update timer and check for finished
        if (_running) {
            _elapsedSeconds += Time.deltaTime;
            if (_elapsedSeconds >= _totalSeconds) {
                _running = false;
            }
        }
    }

    // Runs the timer
    // Because a timer of 0 duration doesn't really make sense, the timer only runs
    // if the total seconds is larger than 0. This also makes sure the consumer of the
    // class has actually set the duration to something higher than 0
    public void Run() {
        // only run with valid duration
        if (_totalSeconds > 0) {
            _started        = true;
            _running        = true;
            _elapsedSeconds = 0;
        }
    }

    #endregion
}