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

    // config data
    private static float _vertSpeed                      = 6.0f;
    private static float _horiSpeed                      = 0.2f;
    private static float _horiSpeedBuffed                = 0.6f;
    private static float _minSpawnIntervalBuff           = 4.0f;
    private static float _maxSpawnIntervalBuff           = 8.0f;
    private static float _minSpawnIntervalObstacleNormal = 1.0f;
    private static float _maxSpawnIntervalObstacleNormal = 1.5f;
    private static float _minSpawnIntervalObstacleBuffed = 0.33f;
    private static float _maxSpawnIntervalObstacleBuffed = 0.5f;

    // ======================================================================
    // Public Properties
    // ======================================================================

    // using expression-body style
    public float VertSpeed                      => _vertSpeed;
    public float HoriSpeed                      => _horiSpeed;
    public float HoriSpeedBuffed                => _horiSpeedBuffed;
    public float MinSpawnIntervalBuff           => _minSpawnIntervalBuff;
    public float MaxSpawnIntervalBuff           => _maxSpawnIntervalBuff;
    public float MinSpawnIntervalObstacleNormal => _minSpawnIntervalObstacleNormal;
    public float MaxSpawnIntervalObstacleNormal => _maxSpawnIntervalObstacleNormal;
    public float MinSpawnIntervalObstacleBuffed => _minSpawnIntervalObstacleBuffed;
    public float MaxSpawnIntervalObstacleBuffed => _maxSpawnIntervalObstacleBuffed;

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

            // read in names and values
            string names  = input.ReadLine();
            string values = input.ReadLine();

            if (names != null) {
                // print out config data title names
                //PrintConfigNames(names);
            }

            if (values != null) {
                // set configuration data fields
                SetConfigDataFields(values);
            }
        } catch (Exception e) {
            Console.WriteLine(e);
            throw;
        } finally {
            // always close input file
            // check if input is null
            // if close a file that never even opened, will get NullReferenceException
            if (input != null) {
                input.Close();
            }
        }
    }

    private void PrintConfigNames(string csvValues) {
        string[] nameSplitArr = csvValues.Split(',');

        foreach (string name in nameSplitArr) {
            Debug.Log(name);
        }
    }

    private void SetConfigDataFields(string csvValues) {
        string[] valueSplitArr = csvValues.Split(',');

        _vertSpeed                      = float.Parse(valueSplitArr[0]);
        _horiSpeed                      = float.Parse(valueSplitArr[1]);
        _horiSpeedBuffed                = float.Parse(valueSplitArr[2]);
        _minSpawnIntervalBuff           = float.Parse(valueSplitArr[3]);
        _maxSpawnIntervalBuff           = float.Parse(valueSplitArr[4]);
        _minSpawnIntervalObstacleNormal = float.Parse(valueSplitArr[5]);
        _maxSpawnIntervalObstacleNormal = float.Parse(valueSplitArr[6]);
        _minSpawnIntervalObstacleBuffed = float.Parse(valueSplitArr[7]);
        _maxSpawnIntervalObstacleBuffed = float.Parse(valueSplitArr[8]);
    }
}