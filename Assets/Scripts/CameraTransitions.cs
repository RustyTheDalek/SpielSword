using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages camera positions between points in scene.
/// Created by : Ian Jones - 09/04/18
/// </summary>
public class CameraTransitions : MonoBehaviour {

    Dictionary<string,Transform> positions = new Dictionary<string, Transform>();

	// Use this for initialization
	void Start ()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("CameraPoints");

        foreach(GameObject obj in objs)
        {
            Debug.Log(obj.name + " added.");
            positions.Add(obj.name, obj.transform);
        }

        VillageExit.OnPlayerLeftVillage += EnterWorldMap;
	}
	
	protected void EnterWorldMap()
    {
        Debug.Log("Moved to WorldMap");
        transform.position = positions["WorldMap"].position;
    }
}
