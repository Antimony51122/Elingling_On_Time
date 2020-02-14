using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Soldiers will change their z-pos according to y-pos relative to car as well
public class Soldier : ZPosChangeable {
    // ======================================================================
    // Field Variables
    // ======================================================================

    [SerializeField] private Animator _animator;
    [SerializeField] private bool _isRunning;
    

    // ======================================================================
    // Main Loop & MonoBehaviour Methods
    // ======================================================================

    void Start() {
        // initialise soldier state with idle, thus _isRunning false
        _isRunning = false;


    }

    void Update() {
        StartChasing();
    }

    // ======================================================================
    // Customised Methods
    // ======================================================================

    private void StartChasing() {
        if (!_isRunning && PlayerControl.PlayerTransform.position.x > transform.position.x) {
            _isRunning = true;
            _animator.SetBool("IsRunning", _isRunning);
        }
    }


}