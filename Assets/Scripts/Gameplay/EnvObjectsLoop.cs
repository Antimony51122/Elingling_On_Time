using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvObjectsLoop : MonoBehaviour {
    // ==============================================================
    // Field Variables
    // ==============================================================

    // --------------- Serialized Cached References ---------------

    // contain all the background objects we want to loop
    [SerializeField] private GameObject[] _loopObjs;

    // for camera to track the player movement
    [SerializeField] private GameObject _player;

    // --------------- Fields to be attached Component Instances ---------------

    // reference for our main camera
    private Camera _mainCamera;

    // --------------- Config Params ---------------

    // store the boundaries of the above camera
    private Vector2 _screenBounds;

    // compensating for screen boundaries crevices
    public float Choke;

    // ==============================================================
    // MonoBehaviour Methods
    // ==============================================================

    void Start() {
        // cache the main camera
        _mainCamera = gameObject.GetComponent<Camera>();

        // TODO: separate all dimension data in modular ScreenUtils class
        // figure our the dimensions of this camera in world space
        // take the screen width and height and plots it on an xy axis
        _screenBounds = _mainCamera.ScreenToWorldPoint(new Vector3(
            Screen.width,
            Screen.height,
            _mainCamera.transform.position.z)
        );

        // cycle through the game objects in the list and execute the function for each of them
        // local reference obj contains the value of our current row in the list
        foreach (GameObject obj in _loopObjs) {
            LoadChildObjects(obj);
        }
    }

    void Update() {
        // log the player object's current x position
        //Debug.Log(_player.transform.position.x);

        // let the camera follow the player and locate the player at x = -5 of the screen
        transform.position = new Vector3(
            _player.transform.position.x + 5,
            transform.position.y,
            transform.position.z);
    }

    void LateUpdate() {
        // re-position their children so they are always filling the screen
        foreach (GameObject obj in _loopObjs) {
            RepositionChildObjects(obj);
        }
    }

    // ==============================================================
    // Customised Methods
    // ==============================================================

    // load all the objects we want to loop to fill the screen
    private void LoadChildObjects(GameObject obj) {
        // figure out the width of the current sprite by
        // fetching the horizontal value of the boundary box of the sprite
        float objectWidth = obj.GetComponent<SpriteRenderer>().bounds.size.x - Choke;

        // how many of the clones we need to make to fill the width of the screen
        // Mathf.Ceil makes sure we have enough objects to fill the width
        // "+ 2" are safety measure precautions for android devices
        int childrenNeeded = (int) Mathf.Ceil(_screenBounds.x * 2 / objectWidth) + 2;

        // clone the project objects so we have a mold as a reference
        GameObject clone = Instantiate(obj) as GameObject;

        // clone all child objects as reference (instead of just using obj) because
        // as we start adding children objects to obj those child objects will be cloned as well
        // instead, we need a copy of obj to use for each child
        for (int i = 0; i <= childrenNeeded; i++) {
            GameObject c = Instantiate(clone) as GameObject;

            // set the clone as the child object of the parent object
            c.transform.SetParent(obj.transform);

            // space out these one after each other
            c.transform.position = new Vector3(
                objectWidth * i,
                transform.position.y,
                obj.transform.position.z);

            c.name = obj.name + i;
        }

        Destroy(clone);
        Destroy(obj.GetComponent<SpriteRenderer>());
    }

    private void RepositionChildObjects(GameObject obj) {
        // be careful with `GetComponentsInChildren` rather than `GetComponentInChildren`
        Transform[] children = obj.GetComponentsInChildren<Transform>();

        // check if the camera extends past to the edge of either the first or the last child
        // and re-position the children accordingly
        // check there are more than one child in the list
        if (children.Length > 1) {
            //Debug.Log(children.Length);

            // what we really care about is the first and the last child
            GameObject firstChild = children[1].gameObject; // [1] because [0] is the parent object
            GameObject lastChild  = children[children.Length - 1].gameObject;

            // transform position is at the centre of the object, so add or subtract half the width
            float halfObjectWidth = lastChild.GetComponent<SpriteRenderer>().bounds.extents.x - Choke;

            // detect if camera is exposing the right edge of the background element
            // "4 *" are safety measure precautions for android devices
            if (transform.position.x + 4 * _screenBounds.x > lastChild.transform.position.x + halfObjectWidth) {
                // move our first child to the end of the list
                firstChild.transform.SetAsLastSibling();

                // set the position of the first child to be at right edge of the last child object
                firstChild.transform.position = new Vector3(
                    lastChild.transform.position.x + halfObjectWidth * 2,
                    lastChild.transform.position.y,
                    lastChild.transform.position.z);
            } else if (transform.position.x - _screenBounds.x < firstChild.transform.position.x - halfObjectWidth) {
                // reverse of the above circumstance
                // move last child to the first of the list
                lastChild.transform.SetAsFirstSibling();

                // set the position of the last child to be at left edge of the first child object
                lastChild.transform.position = new Vector3(
                    firstChild.transform.position.x - halfObjectWidth * 2,
                    firstChild.transform.position.y,
                    firstChild.transform.position.z);
            }
        }
    }
}