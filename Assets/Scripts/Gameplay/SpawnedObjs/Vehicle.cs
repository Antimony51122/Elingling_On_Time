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

    private float _speed; // used when rigidbody 2D in dynamic mode

    // ======================================================================
    // Main Loop & MonoBehaviour Methods
    // ======================================================================

    protected override void Start() {
        _topLaneTop = ScreenUtils.ScreenTop - 2;
        _topLaneBot = 2;
        _botLaneTop = -1;
        _botLaneBot = ScreenUtils.ScreenBottom + 3;

        // use in implementation with kinematic cases
        _speed = 5;

        _rb2D           = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        // determine which lane the car is starting at
        SetLaneAndDirection();

        // register for HealthChangeEvent and GameOverEvent and invoke when colliding with the player
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
        //Debug.Log(coll.name);

        if (coll.gameObject.CompareTag("Player")) {
            // deduct the health by 1
            UnityEvents[EventName.HealthChangedEvent].Invoke(1.0f);
            //Debug.Log(PlayerStatus.Health);

            // check for game over
            if (PlayerStatus.Health == 0) {
                UnityEvents[EventName.GameOverEvent].Invoke(0);
            }
        }

        base.OnTriggerEnter2D(coll);
    }

    protected override void OnDestroy() {
        // remove the invoker so we don't have the Vehicle script hanging around in that dictionary
        // in the EventManager after the Vehicle game object it was attached to gets destroyed
        EventManager.RemoveFloatArgInvoker(EventName.HealthChangedEvent, this);
        EventManager.RemoveFloatArgInvoker(EventName.GameOverEvent, this);
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
        // top lane's towards right, bot lanes towards left since in UK, vehicles keep left
        if (_vehicleLane == VehicleLane.Top) {
            transform.position = new Vector3(
                transform.position.x,
                Random.Range(_topLaneBot, _topLaneTop),
                transform.position.z);

            _rb2D.AddForce(new Vector2(100, 0)); // moving towards right

            // flip the sprite horizontally to make the vehicle face right
            _spriteRenderer.flipX = true;
        } else {
            transform.position = new Vector3(
                transform.position.x,
                Random.Range(_botLaneBot, _botLaneTop),
                transform.position.z);

            // add force to initialise the vehicle movement
            _rb2D.AddForce(new Vector2(-200, 0)); // moving towards left

            // don't flip the sprite horizontally to so the vehicle faces left
            _spriteRenderer.flipX = false;
        }
    }
}