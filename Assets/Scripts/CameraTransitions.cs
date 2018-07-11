using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages camera positions between points in scene.
/// Created by : Ian Jones - 09/04/18
/// </summary>
public class CameraTransitions : MonoBehaviour {

    public Dictionary<string,CameraPoint> positions = new Dictionary<string, CameraPoint>();

	// Use this for initialization
	void Start ()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("CameraPoints");

        foreach(GameObject obj in objs)
        {
            Debug.Log(obj.name + " added.");
            positions.Add(obj.name, obj.GetComponent<CameraPoint>());
        }
	}

    public void Setup(VillageExit villageExit, VillageAndMapManager wMapManager)
    {
        villageExit.OnPlayerLeftVillage += EnterWorldMap;
        wMapManager.OnPlayerEnterVillage += EnterVillage;
    }

    protected void EnterVillage()
    {
        TransitionTo("Village");
    }
	
	protected void EnterWorldMap()
    {
        TransitionTo("WorldMap");
    }

    public void TransitionTo(string position)
    {
        transform.position = new Vector3(   positions[position].transform.position.x,
                                            positions[position].transform.position.y,
                                            transform.position.z);

        Camera.main.orthographicSize = positions[position].size;
    }

    public void Unsubscribe(VillageExit villageExit, VillageAndMapManager wMapManager)
    {
        villageExit.OnPlayerLeftVillage -= EnterWorldMap;
        wMapManager.OnPlayerEnterVillage -= EnterVillage;
    }
}
