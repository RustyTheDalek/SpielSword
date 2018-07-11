using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages Village and World map
/// Created by : Ian Jones - 10/04/18
/// </summary>
public class VillageAndMapManager : MonoBehaviour {

    #region WorldMapObjects

    Dictionary<string,Node> worldNodes = new Dictionary<string,Node>();

    MapVillager mapVillager;
    Basic bVillager;

    Node currentNode, newNode;

    VillageExit villageExit;

    CameraTransitions cameraTransitions;

    bool moving = false, active = false;

    Vector2 direction;

    /// <summary>
    /// How quickly map node travels
    /// </summary>
    public float nodeTravelSpeed = 5;

    #endregion

    Dictionary<string, PlayableAsset> startupSequences = new Dictionary<string, PlayableAsset>();
    PlayableDirector startupDirector;

    bool playingStartup = false;

    public delegate void WorldMapEvent();
    public event WorldMapEvent OnPlayerEnterVillage;

    List<SaveGamePnl> savePnls = new List<SaveGamePnl>();

    public string startPosition;

    void Awake()
    {
        GameManager.gManager.OnNewGame += StartIntro;
        GameManager.gManager.OnLoadAllSaves += SetupSaves;
        GameManager.gManager.OnLoadSave += StartGame;

        savePnls.AddRange(GetComponentsInChildren<SaveGamePnl>(true));
    }

    // Use this for initialization
    void Start ()
    {
        //Find nodes for World Map
        Node[] temp = GetComponentsInChildren<Node>();

        foreach (Node node in temp)
        {
            Debug.Log(node.name + " added.");
            worldNodes.Add(node.name, node);
        }

        currentNode = worldNodes["VillageNode"];

        mapVillager = GetComponentInChildren<MapVillager>();

        villageExit = GetComponentInChildren<VillageExit>();
        villageExit.OnPlayerLeftVillage += Enable;
        villageExit.Setup(this);

        bVillager = GetComponentInChildren<Basic>(true);
        bVillager.Setup(villageExit, this);

        cameraTransitions = GetComponentInChildren<CameraTransitions>();
        cameraTransitions.Setup(villageExit, this);

        startupDirector = GetComponent<PlayableDirector>();

        PlayableAsset[] Timelines = Resources.LoadAll<PlayableAsset>("Timelines");
        
        foreach(PlayableAsset timeline in Timelines)
        {
            startupSequences.Add(timeline.name, timeline);
        }

        cameraTransitions.TransitionTo(startPosition);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (active)
        {
            if (Input.GetButtonDown("Jump") && currentNode.name.Contains("Level"))
            {
                SceneManager.LoadScene(currentNode.name);
            }

            //Directional moving
            if (!moving)
            {
                if (mapVillager.direction == Vector2.right && currentNode.Right)
                {
                    moving = true;
                    newNode = currentNode.Right;
                    direction = Vector2.right;
                }

                if (mapVillager.direction == Vector2.left && currentNode.Left)
                {
                    moving = true;
                    newNode = currentNode.Left;
                    direction = Vector2.left;
                }

                if (mapVillager.direction == Vector2.up && currentNode.Up)
                {
                    moving = true;
                    newNode = currentNode.Up;
                    direction = Vector2.up;
                }

                if (mapVillager.direction == Vector2.down && currentNode.Down)
                {
                    moving = true;
                    newNode = currentNode.Down;
                    direction = Vector2.down;
                }
            }
            else //What to do when reaching a node
            {   
                mapVillager.transform.Translate(direction * Time.deltaTime * nodeTravelSpeed);

                if (Vector3.Distance(mapVillager.transform.position, newNode.transform.position) < .5f)
                {
                    moving = false;

                    direction = Vector2.zero;
                    currentNode = newNode;
                    newNode = null;

                    switch (currentNode.name)
                    {
                        case "VillageNode":

                            active = false;
                            OnPlayerEnterVillage();
                            break;
                    }
                }
            }
        }

        if (playingStartup && startupDirector.state != PlayState.Playing)
        {
            switch (startupDirector.playableAsset.name)
            {
                case "Intro0":

                    Debug.Log("Intro cinematic complete");
                    GameManager.gManager.currentSave.StoryProgress++;
                    GameManager.gManager.Save();
                    playingStartup = false;
                    break;
            }
        }
	}

    void StartIntro()
    {
        startupDirector.playableAsset = startupSequences["Intro0"];
        startupDirector.Play();
        playingStartup = true;
    }

    /// <summary>
    /// In future this will load the right sequence based on 
    /// </summary>
    void StartGame(SaveData save)
    { 
        //We would use save here to set things up as needed
        startupDirector.playableAsset = startupSequences["Intro" + save.StoryProgress];
        startupDirector.Play();
    }

    void SetupSaves(List<SaveData> saves)
    {
        for (int i = 0; i < saves.Count; i++)
        {
            Debug.Log("Saves: " + saves.Count + " : " + "Panels: " + savePnls.Count);
            savePnls[i].LoadInfo(saves[i]);
        }
    }

    private void Enable()
    {
        active = true;
    }

    private void OnDestroy()
    {
        GameManager.gManager.OnNewGame -= StartIntro;
        GameManager.gManager.OnLoadAllSaves -= SetupSaves;

        villageExit.OnPlayerLeftVillage -= Enable;
        villageExit.Unsubscribe(this);
        cameraTransitions.Unsubscribe(villageExit, this);
        bVillager.Unsubscribe(villageExit, this);
    }

}
