using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Soldiers will change their z-pos according to y-pos relative to car as well
public class Soldier : SpawnedObj {
    // ======================================================================
    // Field Variables
    // ======================================================================

    // --------------- Serialized Cached References ---------------

    [SerializeField] private Animator _animator;
    [SerializeField] private bool     _isRunning;

    // --------------- Fields to be attached Component Instances ---------------

    private Rigidbody2D _rb2D;

    // --------------- Config Params ---------------

    private float _impulseForce;

    // ======================================================================
    // Main Loop & MonoBehaviour Methods
    // ======================================================================

    protected override void Start() {
        // initialise soldier state with idle, thus _isRunning false
        _isRunning = false;

        _rb2D = GetComponent<Rigidbody2D>();

        _impulseForce = 11.5f;

        // register for GameOverEvent and invoke when colliding with the player
        UnityEvents.Add(EventName.GameOverEvent, new GameOverEvent());
        EventManager.AddFloatArgInvoker(EventName.GameOverEvent, this);
    }

    protected override void Update() {
        StartChasing();
        Chasing();

        base.Update();
    }

    protected override void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.CompareTag("Player")) {
            // directly go to game over when being caught by the soldier
            UnityEvents[EventName.GameOverEvent].Invoke(0);
        }

        base.OnTriggerEnter2D(coll);
    }

    protected override void OnDestroy() {
        EventManager.RemoveFloatArgInvoker(EventName.GameOverEvent, this);
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

    private void Chasing() {
        if (_isRunning) {
            _rb2D.velocity = Vector2.zero;

            // calculate direction to the player and moving towards it
            Vector2 direction = new Vector2(
                PlayerControl.PlayerTransform.position.x - transform.position.x,
                PlayerControl.PlayerTransform.position.y - transform.position.y);
            direction.Normalize(); // normalise it to make it a unity vector

            // because we set the speed to zero previously, adding the force with the original
            // impulse force with the normalised direction we have just calculated will
            // make the game object moving at the same speed as before
            _rb2D.AddForce(direction * _impulseForce, ForceMode2D.Impulse);
        }
    }
}