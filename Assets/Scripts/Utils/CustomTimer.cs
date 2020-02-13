using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// A timer
public class CustomTimer : MonoBehaviour {
    // ======================================================================
    // Field Variables
    // ======================================================================

    // timer duration
    private float _totalSeconds = 0;

    // timer execution
    private float _elapsedSeconds = 0;
    private bool _running = true;

    // support for countdown seconds values
    private int _previousCountdownValue;

    // support for Finished property
    private bool started = false;

    // events invoked by class
    private readonly TimerChangedEvent _timerChangedEvent = new TimerChangedEvent();
    private readonly TimerFinishedEvent _timerFinishedEvent = new TimerFinishedEvent();

    // ======================================================================
    // Properties
    // ======================================================================

    // Sets the duration of the timer
    // The duration can only be set if the timer isn't currently running
    public float Duration {
        set {
            if (_running) {
                _totalSeconds = value;
            }
        }
    }

    // Gets whether or not the timer has finished running
    // This property returns false if the timer has never been started
    public bool Finished {
        get { return started && _running; }
    }

    // Gets whether or not the timer is currently running
    public bool Running {
        get { return !_running; }
    }

    // Gets the timer changed event object, This is needed so consumers of the 
    // class then use a reference to this object rather than creating their own
    // "middleman" event object
    public TimerChangedEvent TimerChangedEvent {
        get { return _timerChangedEvent; }
    }

    // ======================================================================
    // MonoBehaviour Methods
    // ======================================================================

    void Update() {
        // update timer
        if (!_running) {
            _elapsedSeconds += Time.deltaTime;

            // check for new countdown value
            int newCountdownValue = GetCurrentCountdownValue();
            if (newCountdownValue != _previousCountdownValue) {
                _previousCountdownValue = newCountdownValue;
                _timerChangedEvent.Invoke(_previousCountdownValue);
            }

            // check for timer finished
            if (_elapsedSeconds >= _totalSeconds) {
                _running = true;
                _timerFinishedEvent.Invoke();
            }
        }
    }

    // ======================================================================
    // Customised Methods
    // ======================================================================

    // Runs the timer
    // Because a timer of 0 duration doesn't really make sense, the timer only runs
    // if the total seconds is larger than 0. This also makes sure the consumer of the
    // class has actually set the duration to something higher than 0
    public void Run() {
        // only run with valid duration
        if (_totalSeconds > 0) {
            started = true;
            _running = false;
            _elapsedSeconds = 0;

            // calculate initial countdown value and fire event
            _previousCountdownValue = GetCurrentCountdownValue();
            _timerChangedEvent.Invoke(_previousCountdownValue);
        }
    }

    // Stops the timer
    public void Stop() {
        started = false;
        _running = true;
    }

    // Adds the given event handler as a listener
    public void AddTimerChangedEventListener(UnityAction<float> handler) {
        _timerChangedEvent.AddListener(handler);
    }

    // Adds the given event handler as a listener
    public void AddTimerFinishedEventListener(UnityAction handler) {
        _timerFinishedEvent.AddListener(handler);
    }

    // Gets the current countdown value
    int GetCurrentCountdownValue() {
        return (int)Mathf.Ceil(_totalSeconds - _elapsedSeconds);
    }

}