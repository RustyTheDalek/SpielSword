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
    private int prepareSpawn;
    private int currentSpawn;
    private GameObject spawn;
    private GameObject spawnToUse;
    private List<SpawnList> spawnOrder = new List<SpawnList>();
    private List<GameObject> spawns = new List<GameObject>();
    private FlightMinions script;

    public float maxY;
    public float minY;
    public float despawnZoneX;
    /// <summary>
    /// Time in Seconds that must 
    /// pass befor next spawn
    /// </summary>
    public float maxSpawnInterval;
    public float minSpawnInterval;
    public int noToSpawn;
    public GameObject enemyToSpawn;
    
    // initialize all 
    public void Awake()
    {
        currentSpawn = 0;
        // offset the fact lists use 0 as the first number
        noToSpawn -= 1;
        // sets the amount to spawn based on the distance to cover
        prepareSpawn = Mathf.FloorToInt((((transform.position.x - despawnZoneX) / 10) / minSpawnInterval) + 1);
        // no point creating more then we intend to create
        if (prepareSpawn > noToSpawn)
        {
            prepareSpawn = noToSpawn;
        }
        // creates a bunch of clones of the enemy
        for (int i = 0; i <= prepareSpawn; i++)
        {
            spawn = Instantiate(enemyToSpawn,
                    transform.position, Quaternion.identity, transform);
            script = spawn.GetComponent<FlightMinions>();
            script.dumbFire = true;
            script.killZone = despawnZoneX;
            spawns.Add(spawn);
            spawn.SetActive(false);
        }
    }

    // link all
    void Start () {
        // sets the spawn time and what height it will be created at
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
        // messure game time
        timePassed += Time.deltaTime;
        // keeps count of how many have been spawned so far
        if (spawnOrder.Count > currentSpawn)
        {
            // uses the time set to decided when to spawn the enemys
            if (timePassed > spawnOrder[currentSpawn].spawnTime)
            {
                #region searches for a clone that isn't active
                for (int i = 0; i < spawns.Count; i++)
                {
                    if (!spawns[i].activeSelf)
                    {
                        spawnToUse = spawns[i];
                        break;
                    }
                }
                #endregion

                #region if no clones are avaliable to use incress the time on remaining spawns
                if (spawnToUse == null)
                {
                    for (int i = currentSpawn; i <= spawnOrder.Count; i++)
                    {
                        if (i == spawns.Count)
                        {
                            spawnOrder[i].spawnTime += Random.Range(minSpawnInterval, maxSpawnInterval);
                        }
                        else
                        {
                            spawnOrder[i].spawnTime += spawnOrder[i + 1].spawnTime -
                            spawnOrder[i].spawnTime;
                        }
                    }
                }
                #endregion

                #region move the clone to the spawner based on height and enable
                else
                {
                    spawnToUse.transform.position = new Vector3(transform.position.x,
                                        spawnOrder[currentSpawn].spawnHeight, transform.position.z);
                    spawnToUse.SetActive(true);
                    currentSpawn += 1;
                }
                #endregion
            }
        }
    }
}
