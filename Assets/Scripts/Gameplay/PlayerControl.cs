using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
    // ======================================================================
    // Field Variables
    // ======================================================================

    // --------------- Fields to be attached Component Instances ---------------

    public static Transform PlayerTransform;

    private Rigidbody2D _rb2D;

    // --------------- Config Params ---------------

    // define the moving moving up and down speed and forward speed
    private float _vertSpeed;
    private float _horiSpeed;
    private float _buffFactor;

    public static VertMvtState VertMvtState;
    public static HoriMvtState HoriMvtState;

    // define the upper and lower limits of player movement
    private float _playerMvtUpperLimit;
    private float _playerMvtLowerLimit;

    // ======================================================================
    // Main Loop & MonoBehaviour Methods
    // ======================================================================

    void Start() {
        PlayerTransform = GetComponent<Transform>();
        _rb2D           = GetComponent<Rigidbody2D>();

        // assign the speed values from configuration data
        _vertSpeed  = ConfigUtils.VertSpeed;
        _horiSpeed  = ConfigUtils.HoriSpeed;
        _buffFactor = ConfigUtils.BuffFactor;

        // Android platform has trouble reading config streaming assets, thus directly assign
        //_vertSpeed       = 10.0f;
        //_horiSpeed       = 0.2f;
        //_buffFactor      = 3.0f;

        // initialise the vertical movement state with still where the player keeps the altitude
        VertMvtState = new VertMvtState();
        VertMvtState = VertMvtState.Still;

        // initialise the horizontal movement state with normal where the player keeps steady speed
        HoriMvtState = new HoriMvtState();
        HoriMvtState = HoriMvtState.Normal;

        // assign the player movement upper lower limits
        _playerMvtUpperLimit = ScreenUtils.ScreenTop    - 1.5f;
        _playerMvtLowerLimit = ScreenUtils.ScreenBottom + 2.0f;
    }

    void Update() {
        Debug.Log(Application.platform);
        
        // `RuntimePlatform.WebGLPlayer`   -> WebGL 
        // `RuntimePlatform.WindowsPlayer` -> Windows executable
        // `RuntimePlatform.WindowsEditor` -> Windows unity editor Game interface
        // `RuntimePlatform.OSXPlayer`     -> MacOS executable
        // `RuntimePlatform.OSXEditor`     -> MacOS unity editor Game interface
        // `RuntimePlatform.LinuxPlayer`   -> Linux executable
        // `RuntimePlatform.LinuxEditor`   -> Linux unity editor Game interface

        // using phone gyroscope & accelerometer input when running as phone apps
        if (Application.platform == RuntimePlatform.Android ||
            Application.platform == RuntimePlatform.IPhonePlayer) {
            PhoneSensorControl();
        } else {
            // using keyboard vertical input axis when running on any other platform
            KeyboardControl();
        }

        VertMvtHandler();
        HoriMvtHandler();
        CalculateClampedY();
    }

    // ======================================================================
    // Customised Methods
    // ======================================================================

    // ----- Phone Sensor Control -----
    private void PhoneSensorControl() {
        if (Input.acceleration.y < -0.60) {
            VertMvtState = VertMvtState.MovingDown;
        } else if (Input.acceleration.y > -0.25) {
            VertMvtState = VertMvtState.MovingUp;
        } else {
            VertMvtState = VertMvtState.Still;
        }
    }

    // ----- Keyboard Control -----
    private void KeyboardControl() {
        if (Input.GetKey(KeyCode.S)) {
            VertMvtState = VertMvtState.MovingDown;
        } else if (Input.GetKey(KeyCode.W)) {
            VertMvtState = VertMvtState.MovingUp;
        } else {
            VertMvtState = VertMvtState.Still;
        }
    }

    // ----- Change Movements Speeds by Manipulating States -----

    // TODO: try using Rigidbody2D MovePosition Method to rewrite this function
    private void VertMvtHandler() {
        switch (VertMvtState) {
            case VertMvtState.MovingDown:
                transform.Translate(
                    -Vector3.up * _vertSpeed * Time.deltaTime,
                    Space.World);
                break;
            case VertMvtState.MovingUp:
                transform.Translate(
                    Vector3.up * _vertSpeed * Time.deltaTime,
                    Space.World);
                break;
            case VertMvtState.Still:
                // stop only the vertical speed by setting the vertical component to 0
                transform.Translate(
                    Vector3.up * 0,
                    Space.World);
                break;
            default:
                //transform.position = gameObject.transform.position;
                transform.Translate(
                    Vector3.up * 0,
                    Space.World);
                break;
        }
    }

    private void HoriMvtHandler() {
        switch (HoriMvtState) {
            case HoriMvtState.Normal:
                transform.Translate(Vector3.right * _horiSpeed);
                break;
            case HoriMvtState.Buffed:
                transform.Translate(Vector3.right * _horiSpeed * _buffFactor);
                break;
        }
    }

    // clamp the player movement to prevent going into the boundaries
    private void CalculateClampedY() {
        // remember to add z pos, if using Vector2, the z pos will go back to 0 
        // where the player will be behind the background
        Vector3 playerPos = transform.position;

        if (playerPos.y > _playerMvtUpperLimit || playerPos.y < _playerMvtLowerLimit) {
            playerPos.y = Mathf.Clamp(
                playerPos.y,
                _playerMvtLowerLimit,
                _playerMvtUpperLimit);

            transform.position = playerPos;
        }
    }
}