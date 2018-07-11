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

    public static GameManager gManager;

    #region SaveVariables

    /// <summary>
    /// Loaded Save
    /// </summary>
    public SaveData currentSave;

    /// <summary>
    /// List of available saves
    /// </summary>
    List<SaveData> saves = new List<SaveData>();

    /// <summary>
    /// Array of save names found in the right directory;
    /// </summary>
    string[] saveNames;

    #endregion

    public delegate void GameManagerEvent();
    public event GameManagerEvent OnNewGame;

    public delegate void GameManagerLoadSavesEvent(List<SaveData> savesLoaded);
    public event GameManagerLoadSavesEvent OnLoadAllSaves;

    public delegate void GameManagerLoadSaveEvent(SaveData saveLoaded);
    public event GameManagerLoadSaveEvent OnLoadSave;

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
        //First we need to see if there are save(s).
        Debug.Log("Looking for saves in " + Application.persistentDataPath);
        saveNames = Directory.GetFiles(Application.persistentDataPath);

        //If so Load 'em up
        if (saveNames.Length > 0)
        {
            Debug.Log("I've found " + saveNames.Length + " saves");

            foreach (string saveName in saveNames)
            {
                string formattedName = saveName.Remove(0, Application.persistentDataPath.Length + 1);
                formattedName = formattedName.Remove(5, 4);
                LoadIntoSaves(formattedName);
            }

            if (OnLoadAllSaves != null)
                OnLoadAllSaves(saves);
        }
        else
        {
            Debug.Log("No new saves");
        }
    }

    public void Save()
    {
        Save(currentSave.SaveName);
    }

    public void Save(string _SaveName, bool newSave = false)
    {
        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Create(
            Application.persistentDataPath + "/" + _SaveName + ".dat");

        SaveData saveData;

        if (newSave)
        {
            saveData = new SaveData
            {
                SaveName = _SaveName,
                Hats = new List<string>(),
                Levels = new List<LevelStats>(),
                StoryProgress = 0,
            };
        }
        else
        {
            saveData = new SaveData
            {
                SaveName = _SaveName,
                Hats = currentSave.Hats,
                Levels = currentSave.Levels,
                StoryProgress = currentSave.StoryProgress
            };
        }

        bf.Serialize(file, saveData);
        file.Close();

        currentSave = saveData;
    }

    void LoadIntoSaves(string filename)
    {
        saves.Add(Load(filename));
    }

    public void LoadChosenSave(string filename)
    {
        currentSave = Load(filename);

        if (currentSave != null)
        {
            if (OnLoadSave != null)
                OnLoadSave(currentSave);
        }
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
            Debug.LogWarning("Couldn't find save: " + saveName + " in " + Application.persistentDataPath);
            return null;
        }
    }

    public void StartNewGame(string saveName)
    {
        //Create a new save
        Save(saveName, true);

        if (OnNewGame != null)
            OnNewGame();
    }

#if UNITY_EDITOR
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 30), "Saves Found: " + 0);
    }
#endif
}
 