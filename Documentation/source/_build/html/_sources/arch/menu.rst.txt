.. figure:: ../_static/index/cover.gif
    :align: center
    :width: 100%

Menu & Scene Management
=======================

The game consists of 4 occasions in 3 scenes: a main menu, the gameplay scene, the score page after that and a pause scene which is contained in the gameplay scene. As usual, we start the implementation with an ``enum`` stating all the cases we are caring:

.. code-block:: C#

    public enum MenuName {
        MainMenu,
        Gameplay,
        ScorePage,
        Pause
    }

Then we create a central scene manager to manage the navigation through the menu system:



Main Menu
---------

.. code-block:: C#

    public static class MenuManager {
        // Goes to the menu with the given name
        public static void GoToMenu(MenuName name) {
            switch (name) {
                case MenuName.MainMenu:
                    // go to MainMenu scene
                    SceneManager.LoadScene("01_MainMenu");
                    break;
                case MenuName.Gameplay:
                    // go to gameplay scene
                    SceneManager.LoadScene("02_GamePlay");
                    break;
                case MenuName.ScorePage:
                    // go to score page
                    SceneManager.LoadScene("03_ScorePage");
                    break;
                case MenuName.Pause:
                    // instantiate prefab
                    Object.Instantiate(Resources.Load("PauseMenu"));
                    break;
            }
        }
    }

After that, we handle the detail functionalities in each scene. In the main menu scene, we have a button listening for the ``OnClick`` events for the main menu buttons:

.. code-block:: C#

    public void HandlePlayButtonOnClickEvent() {
        MenuManager.GoToMenu(MenuName.Gameplay);
    }

In the scene, create the button gameobject and attache the function in the :guilabel:`OnClick` event inspector:

.. figure:: ../_static/screenshots_unity/play_button_1.png
    :align: center
    :width: 100%

    :guilabel:`Playbutton` in Unity Hierarchy 

.. figure:: ../_static/screenshots_unity/play_button_2.png
    :align: center
    :width: 100%

    assigning the ``HandlePlayButtonOnClickEvent()`` functionality in the inspector



Score Page
----------

The score page inherit score data from previous gameplay session. We first create a ``GameSession`` class to declare the static variables we are going to show in the score page:

.. code-block:: C#

    public class GameSession : MonoBehaviour {
        public static int ScoreResult;
        public static int TimeResult;
        public static int BuffCollectedResult;
        public static int BuffMissedResult;

        ...
    }

In order to let the ``GameSession`` object get inherited towards the score page, we have to untilise the ``DontDestroyOnLoad`` method. In addition, We should keep only one single ``GameSession`` object throughout the game, thus we need to detect and destroy extra ``GameSession`` object, we find the length of list of ``GameSession`` objects and if it's bigger than 1, that means the current one is the second thus we detroy it. Otherwise, we maintain it towards the next session.

.. code-block:: C#

    void Awake() {
        // Find how many Game Status Objects are there
        // Beware this time using plural FindObjectsOfType<>() because there might be multiple
        int gameSessionsCount = FindObjectsOfType<GameSession>().Length;

        // gameSessionsCount more than one means this is the second game session
        if (gameSessionsCount > 1) {
            // prevent the issues destroying action come in bit later then activating the game object
            gameObject.SetActive(false);
            Destroy(gameObject); // Destroy "yourself" referring to the current Game Status
        } else {
            DontDestroyOnLoad(gameObject); // Maintain "yourself" if this is the first Game Status
        }
    }

Don't forget to destroy the current game status when restarting the game:

.. code-block:: C#

    public void ResetGame() {
        Destroy(gameObject);
    }

In the Gameplay Scene, when one game session has ended, the ``HandleGameOverEvent`` handler function will be called to update the score data that will be passed on to the next scene.

.. code-block:: C#

    private void HandleGameOverEvent(float unused) {
        GameSession.ScoreResult         = Score;
        GameSession.TimeResult          = (int) TotalPlayTime;
        GameSession.BuffCollectedResult = BuffCollectedCount;
        GameSession.BuffMissedResult    = BuffMissedCount;

        MenuManager.GoToMenu(MenuName.ScorePage);
    }

In the score page, we assign the :guilabel:`TextMeshProGUI` object will be assigned and updated:

.. code-block:: C#

    public class ScorePage : MonoBehaviour {
        // ======================================================================
        // Field Variables
        // ======================================================================

        [SerializeField] private TextMeshProUGUI _textMeshProScore2;
        [SerializeField] private TextMeshProUGUI _textMeshTime;
        [SerializeField] private TextMeshProUGUI _textMeshProBuffCollected;
        [SerializeField] private TextMeshProUGUI _textMeshProBuffMissed;

        // ==============================================================
        // Main Loop & MonoBehaviour Methods
        // ==============================================================
        void Start() {
            _textMeshProScore2.text        = GameSession.ScoreResult.ToString();
            _textMeshTime.text             = GameSession.TimeResult.ToString();
            _textMeshProBuffCollected.text = GameSession.BuffCollectedResult.ToString();
            _textMeshProBuffMissed.text    = GameSession.BuffMissedResult.ToString();
        }

        ...
    }

.. figure:: ../_static/screenshots_unity/scorepage_text.png
    :align: center
    :width: 100%

    :guilabel:`TMP` in Unity Hierarchy 

Then we assign handler functions to button listening for the ``OnClick`` events for the score page in the inspector:

.. code-block:: C#

    public void HandleReplayButtonOnClickEvent() {
        DestroyGameSession();
        MenuManager.GoToMenu(MenuName.Gameplay);
    }

    public void HandleBackToMainMenuButtonOnClickEvent() {
        DestroyGameSession();
        MenuManager.GoToMenu(MenuName.MainMenu);
    }

.. |score_btn1| image:: ../_static/screenshots_unity/scorepage_button_1.png
    :align: middle

.. |score_btn2| image:: ../_static/screenshots_unity/scorepage_button_2.png
    :align: middle

+-------------------+-------------------+
| Replay the Game   | Back to Main Menu |
+-------------------+-------------------+
| |score_btn1|      | |score_btn2|      |
+-------------------+-------------------+

Lastly, always remember to destory the current game session when restarting the game to avoid conflicts in next loop of game-flow.

.. code-block:: C#

    private void DestroyGameSession() {
        if (FindObjectOfType<GameSession>() != null) {
            // destroy the current Game Session when restarting the game
            FindObjectOfType<GameSession>().ResetGame();
        }
    }
