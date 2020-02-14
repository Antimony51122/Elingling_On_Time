using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : SpawnedObj {
    // ======================================================================
    // Field Variables
    // ======================================================================

    // --------------- Fields to be attached Component Instances ---------------

    private Rigidbody2D    _rb2D;
    private SpriteRenderer _spriteRenderer;

    // --------------- Config Params ---------------

    private float _topLaneTop;
    private float _topLaneBot;
    private float _botLaneTop;
    private float _botLaneBot;

    private VehicleLane _vehicleLane;

    private float _speed;

    // ======================================================================
    // MonoBehaviour Methods
    // ======================================================================

    protected override void Start() {
        _topLaneTop = ScreenUtils.ScreenTop - 2;
        _topLaneBot = 2;
        _botLaneTop = -1;
        _botLaneBot = ScreenUtils.ScreenBottom + 3;

        _speed = 5;

        _rb2D           = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        SetLaneAndDirection();

        UnityEvents.Add(EventName.HealthChangedEvent, new HealthChangedEvent());
        EventManager.AddFloatArgInvoker(EventName.HealthChangedEvent, this);

        UnityEvents.Add(EventName.GameOverEvent, new GameOverEvent());
        EventManager.AddFloatArgInvoker(EventName.GameOverEvent, this);
    }

    protected override void Update() {
        // vehicle on top lane moving towards left, bot towards right
        // this method is very computational consuming,
        // instead, set vehicle object to dynamic and set gravity to 0 to avoid falling
        //if (_vehicleLane == VehicleLane.Top) {
        //    transform.Translate(
        //        Vector2.left * _speed * Time.deltaTime, Space.World);
        //} else {
        //    transform.Translate(
        //        Vector2.right * _speed * Time.deltaTime, Space.World);
        //}

        base.Update();
    }

    protected override void OnTriggerEnter2D(Collider2D coll) {
        Debug.Log(coll.name);

        if (coll.gameObject.CompareTag("Player")) {
            UnityEvents[EventName.HealthChangedEvent].Invoke(1.0f);

            // check for game over
            if (PlayerStatus.Health == 0) {
                UnityEvents[EventName.GameOverEvent].Invoke(0);
                Debug.Log(PlayerStatus.Health);
            }
        }

        


    }

    // ======================================================================
    // Customised Methods
    // ======================================================================

    private void SetLaneAndDirection() {
        int enumLen = System.Enum.GetNames(typeof(VehicleLane)).Length;
        _vehicleLane = (VehicleLane) Random.Range(0, enumLen);

        //Debug.Log(_vehicleLane);

        // if VehicleLane is top, spawn on top lane range, otherwise on bot lane range
        // place the vehicle to corresponding initial position and add force to make it move
        if (_vehicleLane == VehicleLane.Top) {
            transform.position = new Vector3(
                transform.position.x,
                Random.Range(_topLaneBot, _topLaneTop),
                transform.position.z);

            // add force to initialise the vehicle movement
            _rb2D.AddForce(new Vector2(-200, 0)); // moving towards left

            // don't flip the sprite horizontally to so the vehicle faces left
            _spriteRenderer.flipX = false;
        } else {
            transform.position = new Vector3(
                transform.position.x,
                Random.Range(_botLaneBot, _botLaneTop),
                transform.position.z);

            _rb2D.AddForce(new Vector2(100, 0)); // moving towards right

            // flip the sprite horizontally to make the vehicle face right
            _spriteRenderer.flipX = true;
        }
    }
}