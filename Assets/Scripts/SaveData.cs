using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    string _SaveName;
    public string SaveName
    {
        get
        {
            return _SaveName;
        }
        set
        {
            _SaveName = value;
        }
    }

    /// <summary>
    /// Stats for levels played
    /// </summary>
    List<LevelStats> _Levels;
    public List<LevelStats> Levels
    {
        get
        {
            return _Levels;
        }
        set
        {
            _Levels = value;
        }
    }

    /// <summary>
    /// Hats player has unlocked 
    /// </summary>
    List<string> _Hats;
    public List<string> Hats
    {
        get
        {
            return _Hats;
        }
        set
        {
            _Hats = value;
        }
    }

    /// <summary>
    /// Progression in story
    /// </summary>
    int _StoryProgress;
    public int StoryProgress
    {
        get
        {
            return _StoryProgress;
        }
        set
        {
            _StoryProgress = value;
        }
    }

    /// <summary>
    /// Favourite Hat
    /// </summary>
    string _Hat;
    public string Hat
    {
        get
        {
            return _Hat;
        }
        set
        {
            _Hat = value;
        }
    }
}