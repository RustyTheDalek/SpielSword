using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Script for storing the hight and spawn times for
/// FlyingEnemySpawner
/// Created     : Sean Taylor - 28/06/18
/// </summary>
public class SpawnList : MonoBehaviour {

    public float spawnTime;
    public float spawnHeight;

    public SpawnList(float time, float height)
    {
        spawnTime = time;
        spawnHeight = height;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
