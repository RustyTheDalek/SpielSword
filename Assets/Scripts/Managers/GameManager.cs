using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

/// <summary>
/// Looks after broader strokes of game, like Saves
/// </summary>
public class GameManager : MonoBehaviour {

    static GameManager gManager;

    #region SaveVariables

    /// <summary>
    /// Loaded Save
    /// </summary>
    SaveData currentSave;
    /// <summary>
    /// List of available saves
    /// </summary>
    List<SaveData> saves;

    /// <summary>
    /// Array of save names found in the right directory;
    /// </summary>
    string[] saveNames;

    List<LevelStats> levels;

    /// <summary>
    /// Hats player has unlocked 
    /// </summary>
    List<string> hats;

    int storyProgress;

    #endregion

    private void Awake()
    {
        if (gManager == null)
        {
            DontDestroyOnLoad(gameObject);
            gManager = this;
        }
        else if (gManager != this)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        //First we need to see if there is save(s).
        Debug.Log("Looking for saves in " + Application.persistentDataPath);
        saveNames = Directory.GetFiles(Application.persistentDataPath);

        //If so Load 'em up
        if (saveNames.Length > 0)
        {
            Debug.Log("I've found " + saveNames.Length + "saves");

            foreach (string saveName in saveNames)
            {
                LoadIntoSaves(saveName);
            }
        }
        else
        {
            Debug.Log("No new saves");
            //Load single new Game button
        }
    }

    void Save(string saveName, bool newSave = false)
    {
        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Create(
            Application.persistentDataPath + "/" + saveName + ".dat");

        SaveData saveData;

        if (newSave)
        {
            saveData = new SaveData
            {
                Hats = new List<string>(),
                Levels = new List<LevelStats>(),
                StoryProgress = 0,
            };
        }
        else
        {
            saveData = new SaveData
            {
                Hats = hats,
                Levels = levels,
                StoryProgress = storyProgress
            };
        }

        bf.Serialize(file, saveData);
        file.Close();
    }

    void LoadIntoSaves(string filename)
    {
        saves.Add(Load(filename));
    }

    void LoadChosenSave(string filename)
    {
        currentSave = Load(filename);
    }

    SaveData Load(string saveName)
    {
        if (File.Exists(Application.persistentDataPath + "/" + saveName + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(
                Application.persistentDataPath + "/" + saveName + ".dat", FileMode.Open);
            SaveData saveData = (SaveData)bf.Deserialize(file);
            file.Close();
            return saveData;
        }
        else
        {
            Debug.LogWarning("Couldn't find save: " + saveName);
            return null;
        }
    }

    public void StartNewGame(string saveName)
    {
        //Create a new save
        //Save(saveName, true);
        //Fade Save game Canvas

        //Animate to start game window
    }

#if UNITY_EDITOR
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 30), "Saves Found: " + 0);
    }
#endif
}

[Serializable]
class SaveData
{
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
}
