.. figure:: ../_static/index/cover.gif
    :align: center
    :width: 100%

Utility Classes
===============

As a professional practice of software engineering, we tends to extract all utility classes which are not inheriting from the Unity :any:`MonoBehaviour` and contain functionalities that could be repeatly used in separate files. Then the scripts handling gameply implementations could just import and use these files like external packages. In this game, apart from the :file:`CustomTimer` class, we have another two utility classes serving these purposes in a similar pattern. Since these are just utility classes with static methods that we can directly utilise, I won't go into details how these functionalities have been implementated.

The first one is the :file:`ScreenUtils` class which contains static properties of the coordinates of the 4 edges of the screen:

.. code-block:: C#

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    // Provides screen utilities
    public static class ScreenUtils {
        #region Fields

        // cached for efficient boundary checking
        private static float _screenLeft;
        private static float _screenRight;
        private static float _screenTop;
        private static float _screenBottom;

        #endregion

        #region Properties

        // Gets the left edge of the screen in world coordinates
        public static float ScreenLeft {
            get { return _screenLeft; }
        }

        // Gets the right edge of the screen in world coordinates
        public static float ScreenRight {
            get { return _screenRight; }
        }

        // Gets the top edge of the screen in world coordinates
        public static float ScreenTop {
            get { return _screenTop; }
        }

        // Gets the bottom edge of the screen in world coordinates
        public static float ScreenBottom {
            get { return _screenBottom; }
        }

        #endregion

        #region Methods

        // Initialises the screen utilities
        public static void Initialize() {
            // save screen edges in world coordinates
            float screenZ = -Camera.main.transform.position.z;

            Vector3 lowerLeftCornerScreen  = new Vector3(0,            0,             screenZ);
            Vector3 upperRightCornerScreen = new Vector3(Screen.width, Screen.height, screenZ);
            Vector3 lowerLeftCornerWorld   = Camera.main.ScreenToWorldPoint(lowerLeftCornerScreen);
            Vector3 upperRightCornerWorld  = Camera.main.ScreenToWorldPoint(upperRightCornerScreen);

            _screenLeft   = lowerLeftCornerWorld.x;
            _screenRight  = upperRightCornerWorld.x;
            _screenTop    = upperRightCornerWorld.y;
            _screenBottom = lowerLeftCornerWorld.y;
        }

        #endregion
    }

Another one is the :any:`Probability` class which helps handling a set of events and assign a set of probabilities to each of them and let them randomly happen:

.. code-block:: C#

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Probability {
        public static T RandomEventsWithProb<T>(
            List<KeyValuePair<T, float>> items, float totalProb) {
            // pick random value with in range the sum of all occurence probabilities
            float randomValue = Random.Range(0, totalProb);
            float cumulative  = 0;

            foreach (KeyValuePair<T, float> item in items) {
                cumulative += item.Value;
                if (randomValue < cumulative) {
                    return item.Key;
                }
            }

            return default;
        }
    }