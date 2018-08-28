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

    private SaveData _CurrentSave;
    /// <summary>
    /// Loaded Save
    /// </summary>
    public SaveData CurrentSave
    {
        get
        {
            if (_CurrentSave == null)
            {
                //Catch in case no save
                Debug.LogWarning("No Save, returning Null?");
                return null;
            }
            else
                return _CurrentSave;
        }

        set
        {
            _CurrentSave = value;
        }
    }


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

    public delegate void HatEvent(Hat hat);
    public event HatEvent OnUnlockHat;

    protected string SaveLocation
    {
        get
        {
            return Application.persistentDataPath + "/Saves/";
        }
    }

    SaveIcon saveIcon;

    public AssetBundle hats;

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
        saveIcon = GetComponentInChildren<SaveIcon>();

        hats = AssetBundle.LoadFromFile(Path.Combine(
            Application.streamingAssetsPath, "AssetBundles/hats"));

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

    /// <summary>
    /// Unlocks Hat for save
    /// </summary>
    /// <param name="newHat">Name of new Hat that's unlocked</param>
    /// <param name="save"> Do we want to save as well? By default we will</param>
    public void UnlockHat(string newHat, bool save = true)
    {
        if (gManager.CurrentSave != null && !gManager.CurrentSave.Hats.Contains(newHat))
        {
            Debug.Log("Unlocked : " + newHat);
            gManager.CurrentSave.Hats.Add(newHat);

            OnUnlockHat(gManager.hats.LoadAsset<Hat>(newHat));

            if(save)
                gManager.Save();
        }
    }

    public void Save()
    {
        Save(CurrentSave.SaveName);
    }

    public void Save(string _SaveName, bool newSave = false)
    {
        StartCoroutine(saveIcon.ShowSave());

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
                Hats = CurrentSave.Hats,
                Hat = CurrentSave.Hat,
                Levels = CurrentSave.Levels,
                StoryProgress = CurrentSave.StoryProgress
            };
        }

        bf.Serialize(file, saveData);
        file.Close();

        CurrentSave = saveData;
    }

    void LoadIntoSaves(string filename)
    {
        saves.Add(Load(filename));
    }

    public void LoadChosenSave(string filename)
    {
        CurrentSave = Load(filename);

        if (CurrentSave != null)
        {
            if (OnLoadSave != null)
                OnLoadSave(CurrentSave);
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
}
 