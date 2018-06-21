using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages camera positions between points in scene.
/// Created by : Ian Jones - 09/04/18
/// </summary>
public class CameraTransitions : MonoBehaviour {

    public Dictionary<string,Transform> positions = new Dictionary<string, Transform>();

	// Use this for initialization
	void Start ()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("CameraPoints");

        foreach(GameObject obj in objs)
        {
            Debug.Log(obj.name + " added.");
            positions.Add(obj.name, obj.transform);
        }

        WorldMapManager.OnPlayerEnterVillage += EnterVillage;
	}

    public void Setup(VillageExit villageExit)
    {
        villageExit.OnPlayerLeftVillage += EnterWorldMap;
    }

    protected void EnterVillage()
    {
        transform.position = new Vector3(   positions["Village"].position.x, 
                                            positions["Village"].position.y, 
                                            transform.position.z);
    }
	
	protected void EnterWorldMap()
    {
        Debug.Log("Moved to WorldMap");
        transform.position = new Vector3(   positions["WorldMap"].position.x, 
                                            positions["WorldMap"].position.y, 
                                            transform.position.z);
    }

    public void Unsubscribe(VillageExit villageExit)
    {
        villageExit.OnPlayerLeftVillage -= EnterWorldMap;
    }
}
