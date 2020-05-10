.. figure:: ../_static/index/cover.gif
    :align: center
    :width: 100%

Configuration Utilities & Game Initialisation
=============================================

As a professional practise of game development, we tends to separate all the configuration parameters used in the game in a centralise data management file, usually in :guilabel:`.csv` file thus we can tune the game directly in the separate data file.

Once again, we start with declaring an ``enum`` of data value names:

.. code-block:: C#

    public enum ConfigDataValueName {
        VertSpeed,
        HoriSpeed,
        BuffFactor,
        BuffDuration,
        MinSpawnIntervalBuff,
        MaxSpawnIntervalBuff,
        MinSpawnIntervalObstacle,
        MaxSpawnIntervalObstacle,
        MinSpawnIntervalSoldier,
        MaxSpawnIntervalSoldier
    }

After declaring the enumerations comes the main part where we create the ``ConfigData`` class for all the data manipulations. Firstly, we declare the variable to store the string of data path and declare the value dictionary:

.. code-block:: C#

    private const string ConfigDataFileName = "ConfigData.csv";

    private readonly Dictionary<ConfigDataValueName, float> _values =
        new Dictionary<ConfigDataValueName, float>();

Then we declare all the public properties to be utilised elsewhere:

.. code-block:: C#

    // using expression-body style
    public float VertSpeed                => _values[ConfigDataValueName.VertSpeed];
    public float HoriSpeed                => _values[ConfigDataValueName.HoriSpeed];
    public float BuffFactor               => _values[ConfigDataValueName.BuffFactor];
    public float BuffDuration             => _values[ConfigDataValueName.BuffDuration];
    public float MinSpawnIntervalBuff     => _values[ConfigDataValueName.MinSpawnIntervalBuff];
    public float MaxSpawnIntervalBuff     => _values[ConfigDataValueName.MaxSpawnIntervalBuff];
    public float MinSpawnIntervalObstacle => _values[ConfigDataValueName.MinSpawnIntervalObstacle];
    public float MaxSpawnIntervalObstacle => _values[ConfigDataValueName.MaxSpawnIntervalObstacle];
    public float MinSpawnIntervalSoldier  => _values[ConfigDataValueName.MinSpawnIntervalSoldier];
    public float MaxSpawnIntervalSoldier  => _values[ConfigDataValueName.MaxSpawnIntervalSoldier];

After that, we define the main functionality of stream reading. The function should read configuration data from a file. If the file read fails, the object should contain default values for the configuration data. After reading the data, always remember to close the input file and check if input is ``null``. If we close a file that has never been opened, we will get a ``NullReferenceException``.

.. code-block:: C#

    public ConfigData() {
        StreamReader input = null;

        try {
            // create stream reader object
            input = File.OpenText(Path.Combine(
                Application.streamingAssetsPath, ConfigDataFileName));

            // populate in names and values
            string currentLine = input.ReadLine();
            while (currentLine != null) {
                string[] tokens = currentLine.Split(',');
                ConfigDataValueName valueName = (ConfigDataValueName)Enum.Parse(
                    typeof(ConfigDataValueName), tokens[0]);
                _values.Add(valueName, float.Parse(tokens[1]));
                currentLine = input.ReadLine();
            }
        } catch (Exception e) {
            Console.WriteLine(e);

            // set default values if something went wrong
            SetDefaultValues();
        } finally {
            // if close a file that never even opened, will get NullReferenceException
            if (input != null) {
                input.Close();
            }
        }
    }

.. warning:: Beware that the ``Application.streamingAssetsPath`` variable corresponds to a certain directory :guilabel:`StreamingAssets` for the convenience to deduct redundant hard-coding. However, the :guilabel:`.cvs` file has to be in this directory or otherwise will trigger the exception. 

.. figure:: ../_static/screenshots_unity/streaming_assets.png
    :align: center

    screenshots of streaming assets path in unity 

As a fallback plan if the stream reading fails, we should always declare default values:

.. code-block:: C#

    private void SetDefaultValues() {
        _values.Clear();
        _values.Add(ConfigDataValueName.VertSpeed,                10.0f);
        _values.Add(ConfigDataValueName.HoriSpeed,                0.2f);
        _values.Add(ConfigDataValueName.BuffFactor,               3.0f);
        _values.Add(ConfigDataValueName.BuffDuration,             4.0f);
        _values.Add(ConfigDataValueName.MinSpawnIntervalBuff,     8.0f);
        _values.Add(ConfigDataValueName.MaxSpawnIntervalBuff,     12.0f);
        _values.Add(ConfigDataValueName.MinSpawnIntervalObstacle, 1.25f);
        _values.Add(ConfigDataValueName.MaxSpawnIntervalObstacle, 1.75f);
        _values.Add(ConfigDataValueName.MinSpawnIntervalSoldier,  12.0f);
        _values.Add(ConfigDataValueName.MaxSpawnIntervalSoldier,  20.0f);
    }

After declaring the ``ConfigData`` class, we declare a ``ConfigUtils`` utility class to declare static variables of each of the parameters, Since these are utility classes we don't need to inherit from the ``MonoBehaviour`` unity class as we don't want to attach the class to game objects to instantiate it. We just want consumers to access the class directly.

.. code-block:: C#

    public class ConfigUtils {
        private static ConfigData _configData;

        // using expression-body style
        public static float VertSpeed                => _configData.VertSpeed;
        public static float HoriSpeed                => _configData.HoriSpeed;
        public static float BuffFactor               => _configData.BuffFactor;
        public static float BuffDuration             => _configData.BuffDuration;
        public static float MinSpawnIntervalBuff     => _configData.MinSpawnIntervalBuff;
        public static float MaxSpawnIntervalBuff     => _configData.MaxSpawnIntervalBuff;
        public static float MinSpawnIntervalObstacle => _configData.MinSpawnIntervalObstacle;
        public static float MaxSpawnIntervalObstacle => _configData.MaxSpawnIntervalObstacle;
        public static float MinSpawnIntervalSoldier  => _configData.MinSpawnIntervalSoldier;
        public static float MaxSpawnIntervalSoldier  => _configData.MaxSpawnIntervalSoldier;

        // Initialise the config utils, run the initialisation in GameInitializer.cs
        public static void Initialize() {
            _configData = new ConfigData();
        }
    }

Eventually, we call the ``Initialize()`` method in ``GameInitializer`` class where all the functionalities including ``EventManager`` functionalities from the previous section initialise for the current game session. The ``GameInitializer`` class should be the first script attached to the :guilabel:`Main Camera` in the Gamplay Scene:

.. code-block:: C#

    public class GameInitializer : MonoBehaviour {
        // Awake is called before Start
        void Awake() {
            // initialise the screen utils
            ScreenUtils.Initialize();

            // initialise the config utils
            // Beware: build on phone device has problem reading streaming assets
            ConfigUtils.Initialize();

            // initialise all event handling functionality
            EventManager.Initialize();
        }
    }

.. warning:: Note that the phone build have problems with streaming assets reading functionalities thus we just use the default values for phone builds.
