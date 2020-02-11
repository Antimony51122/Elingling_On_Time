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