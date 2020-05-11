.. figure:: ../_static/index/cover.gif
    :align: center
    :width: 100%

Background Environment
======================

Since the game is potentially a endless running game, it's important to provide a endless running pattern. It's not possible to pre-create the entire map which is long enough and let the player running on since this essentially is not endless and will also occupy huge memory spaces. In order to create smooth endless transitioning, we need to have a set of the same background element that the left side of it can connect with the right side and reuse this background element repeatly when the player is running towards the right.

.. figure:: ../_static/screenshots_unity/background_repetition.png
    :align: center
    :width: 100%

    Background Repetition

As you can see in the hierarchy tab in the above screenshot, when the game is running, we have 4 background elements in a row. Essentially, when the player is running towards the right, we take the last element which just left the screen the player just run over, and we move it to the right as you can see in the below screenshot, :any:`background0` has now moved from the left which is behind the player to the right which is in front of the player.

.. figure:: ../_static/screenshots_unity/background_repetition2.png
    :align: center
    :width: 100%

    Background Repetition 2

Then we just keep looping the same pattern and create a smoothly transitioning endless running pattern.

All of the above logic has been singly implemented in one file :file:`EnvObjLoop`. We start with declaring all the background objects we want to loop through and store the screen boundary configuration parameter:

.. code-block:: C#

    [SerializeField] private GameObject[] _loopObjs;

    ...

    private Vector2 _screenBounds;

Then we create a function to load all the objects we want to loop to fill the screen. We firstly figure out the width of the current spirte, ten we calculate how many of the clones wee need to fill the width of the screen, after that we start instantiating the clones and add it as the child:

.. code-block:: C#

    private void LoadChildObjects(GameObject obj) {
        // figure out the width of the current sprite by
        // fetching the horizontal value of the boundary box of the sprite
        float objectWidth = obj.GetComponent<SpriteRenderer>().bounds.size.x - Choke;

        // how many of the clones we need to make to fill the width of the screen
        // Mathf.Ceil makes sure we have enough objects to fill the width
        // "+ 2" are safety measure precautions for android devices
        int childrenNeeded = (int)Mathf.Ceil(_screenBounds.x * 2 / objectWidth) + 2;

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

After the step of creating and fulfilling, we need tackle the re-positioning. We first check if the camera has passed the edge of either the left-most child or the right-most child and re-position the children object accordingly. 

.. important:: Beware that since the position of each child has been specified using the center of the object, when performing the calculations, we need to deduct or add half object with to reach the left-most or right-most boundary.

.. code-block:: C#

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
            GameObject lastChild = children[children.Length - 1].gameObject;

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