using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ConfigData {
    // ======================================================================
    // Field Variables
    // ======================================================================

    private const string ConfigDataFileName = "ConfigData.csv";

    private readonly Dictionary<ConfigDataValueName, float> _values =
        new Dictionary<ConfigDataValueName, float>();

    // ======================================================================
    // Public Properties
    // ======================================================================

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

    // ======================================================================
    // Constructor
    // ======================================================================

    // Reads configuration data from a file. If the file read fails,
    // the object contains default values for the configuration data
    public ConfigData() {
        // read and save configuration data from file
        StreamReader input = null;

        try {
            // create stream reader object
            input = File.OpenText(Path.Combine(
                // put the data file into a folder called StreamingAssets
                // use this value to get to that folder location without hard-coding
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
            // always close input file
            // check if input is null
            // if close a file that never even opened, will get NullReferenceException
            if (input != null) {
                input.Close();
            }
            // with null propagation:
            //input?.Close();
        }

        //// direct value setting without stream reading, for phone builds
        //SetDefaultValues();
    }

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
}