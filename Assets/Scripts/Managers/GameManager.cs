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

    protected string SaveLocation
    {
        get
        {
            return Application.persistentDataPath + "/Saves/";
        }
    }

    #region Assets
    Dictionary<string, Hat> _Hats;

    public Dictionary<string, Hat> Hats
    {
        get
        {
            if (_Hats == null)
            {
                _Hats = new Dictionary<string, Hat>();

                Object[] objs = Resources.LoadAll<Hat>("Hats");

                Hat hat;

                foreach (object obj in objs)
                {
                    if (obj as Hat != null)
                    {
                        hat = (Hat)obj;
                        Debug.Log("Adding: " + hat.name);
                        _Hats.Add(hat.name, hat);
                    }
                }
            }

            return _Hats;
        }
    }
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
        //First we need to see if there are save(s).
        Debug.Log("Looking for saves in " + SaveLocation);

        if (!Directory.Exists(SaveLocation))
            Directory.CreateDirectory(SaveLocation);

        saveNames = Directory.GetFiles(SaveLocation);

        //If so Load 'em up
        if (saveNames.Length > 0)
        {
            Debug.Log("I've found " + saveNames.Length + " saves");

            for(int i = 1; i < 4; i++)
            {
                string saveName = "Save" + i;
                LoadIntoSaves(saveName);
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
            SaveLocation + _SaveName + ".dat");

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
                Hat = currentSave.Hat,
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
        if (File.Exists(SaveLocation + saveName + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(
                SaveLocation + saveName + ".dat", FileMode.Open);
            SaveData saveData = (SaveData)bf.Deserialize(file);
            file.Close();
            return saveData;
        }
        else
        {
            Debug.LogWarning("Couldn't find save: " + saveName + " in " + SaveLocation);
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
 