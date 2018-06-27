using System.Collections.Generic;
using UnityEngine;
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

    public delegate void WorldMapEvent();
    public event WorldMapEvent OnPlayerEnterVillage;

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

        mapVillager = GetComponentInChildren<MapVillager>();

        currentNode = worldNodes["VillageNode"];

        villageExit = GetComponentInChildren<VillageExit>();
        villageExit.OnPlayerLeftVillage += Enable;
        villageExit.Setup(this);

        bVillager = GetComponentInChildren<Basic>();
        bVillager.Setup(villageExit, this);

        cameraTransitions = GetComponentInChildren<CameraTransitions>();
        cameraTransitions.Setup(villageExit, this);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (active)
        {
            //Allos logic for selecting levels
            switch (currentNode.name)
            {
                case "World1":

                    if(Input.GetKeyDown(KeyCode.Space))
                    {
                        SceneManager.LoadScene("First Level");
                    }
                    break;

                case "World2":

                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        SceneManager.LoadScene("Second Level");
                    }
                    break;
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
	}

    private void Enable()
    {
        active = true;
    }

    private void OnDestroy()
    {
        villageExit.OnPlayerLeftVillage -= Enable;
        villageExit.Unsubscribe(this);
        cameraTransitions.Unsubscribe(villageExit, this);
        bVillager.Unsubscribe(villageExit, this);
    }

}
