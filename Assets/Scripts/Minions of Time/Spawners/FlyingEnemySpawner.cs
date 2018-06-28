using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Script for spawning flying enemys
/// Created     : Sean Taylor - 28/06/18
/// </summary>
public class FlyingEnemySpawner : MonoBehaviour {

    private float spawnTime;
    private float spawnHeight;
    private float timePassed;
    private int currentSpawn;
    private List<SpawnList> spawnOrder = new List<SpawnList>();
    private GameObject spawn;

    public float maxY;
    public float minY;
    public float despawnZoneX;
    public GameObject enemyToSpawn;
    /// <summary>
    /// Time in Seconds that must 
    /// pass befor next spawn
    /// </summary>
    public float maxSpawnInterval;
    public float minSpawnInterval;
    public int noToSpawn;

    // initialize all 
    public void Awake()
    {
        currentSpawn = 0;
        noToSpawn -= 1;
        // WIP create dumby objects for use
    }

    // link all
    void Start () {

        for (int i = 0; i <= noToSpawn; i++)
        {
            spawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            spawnHeight = Random.Range(minY, maxY);
            if (i == 0)
            {
                spawnOrder.Add(new SpawnList
                (spawnTime, spawnHeight));
            }
            else
            {
                spawnOrder.Add(new SpawnList
                (spawnOrder[i - 1].spawnTime + spawnTime, spawnHeight));
            }
        }
    }
	
	// Update is called once per frame
	void Update () {

        timePassed += Time.deltaTime;
        if (spawnOrder.Count > currentSpawn)
        {
            if (timePassed > spawnOrder[currentSpawn].spawnTime)
            {
                spawn = Instantiate(enemyToSpawn, 
                    new Vector3 (transform.position.x, 
                    spawnOrder[currentSpawn].spawnHeight, transform.position.z),
                    Quaternion.identity);
                spawn.GetComponent<FlightMinions>().dumbFire = true;
                spawn.GetComponent<FlightMinions>().killZone = despawnZoneX;
                currentSpawn += 1;
            }
        }
    }
}
