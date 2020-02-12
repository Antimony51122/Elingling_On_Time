using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
    // ==============================================================
    // Field Variables
    // ==============================================================

    // --------------- Fields to be attached Component Instances ---------------

    public static Transform PlayerTransform;

    private Rigidbody2D _rb2dPlayer;

    // --------------- Config Params ---------------

    // define the moving moving up and down speed and forward speed
    private float _vertSpeed;
    private float _horiSpeed;
    private float _horiSpeedBuffed;

    public static VertMvtState VertMvtState;
    public static HoriMvtState HoriMvtState;

    // define the upper and lower limits of player movement
    private float _playerMvtUpperLimit;
    private float _playerMvtLowerLimit;

    // ==============================================================
    // MonoBehaviour Methods
    // ==============================================================

    void Start() {
        PlayerTransform = GetComponent<Transform>();
        _rb2dPlayer     = GetComponent<Rigidbody2D>();

        // assign the speed values from configuration data
        //_vertSpeed       = ConfigUtils.VertSpeed;
        //_horiSpeed       = ConfigUtils.HoriSpeed;
        //_horiSpeedBuffed = ConfigUtils.HoriSpeedBuffed;

        // Android platform has trouble reading config streaming assets, thus directly assign
        _vertSpeed       = 10.0f;
        _horiSpeed       = 0.2f;
        _horiSpeedBuffed = 0.6f;

        // initialise the vertical movement state with still where the player keeps the altitude
        VertMvtState = new VertMvtState();
        VertMvtState = VertMvtState.Still;

        // initialise the horizontal movement state with normal where the player keeps steady speed
        HoriMvtState = new HoriMvtState();
        HoriMvtState = HoriMvtState.Normal;
    }

    void Update() {
        HoriMvtHandler();
    }

    // ==============================================================
    // Customised Methods
    // ==============================================================

    private void HoriMvtHandler() {
        if (HoriMvtState == HoriMvtState.Normal) {
            transform.Translate(Vector3.right * _horiSpeed);
        } else if (HoriMvtState == HoriMvtState.Buffed) {
            transform.Translate(Vector3.right * _horiSpeedBuffed);
        }
    }

    // clamp the player movement to prevent going into the boundaries
    private void CalculateClampedY() {
        // remember to add z pos, if using Vector2, the z pos will go back to 0 
        // where the player will be behind the background
        Vector3 playerPos = new Vector3(
            transform.position.x,
            transform.position.y,
            transform.position.z);

        if (playerPos.y > _playerMvtUpperLimit || playerPos.y < _playerMvtLowerLimit) {
            playerPos.y = Mathf.Clamp(
                playerPos.y,
                _playerMvtLowerLimit,
                _playerMvtUpperLimit);

            transform.position = playerPos;
        }
    }
}