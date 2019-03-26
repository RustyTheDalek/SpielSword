using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Script for spawning flying enemys
/// Created     : Sean Taylor - 28/06/18
/// </summary>
public class FlyingEnemySpawner : TimeObjectLite
{

    private float spawnTime;
    private float spawnHeight;
    private float timePassed;
    private int prepareSpawn;
    private int currentSpawn;
    private GameObject spawn;
    private FlightMinion spawnToUse;
    private List<SpawnList> spawnOrder = new List<SpawnList>();
    private List<FlightMinion> spawns = new List<FlightMinion>();

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
    public FlightMinion enemyToSpawn;

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
        enemyToSpawn.CreatePool(prepareSpawn);
    }

    // link all
    void Start()
    {
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

    public void Setup(TimeObjectManager tManager)
    {
        tManager.OnRestartLevel += ResetTime;
    }

    // Update is called once per frame
    protected override void TOLUpdate()
    {
        // messure game time
        timePassed += Time.deltaTime;
        // keeps count of how many have been spawned so far
        if (spawnOrder.Count > currentSpawn)
        {
            // uses the time set to decided when to spawn the enemys
            if (timePassed > spawnOrder[currentSpawn].spawnTime)
            {
                //If a minion has already been spawned at this time then renable
                if (spawns.Count > currentSpawn)
                {
                    spawns[currentSpawn].enabled = true;
                    spawns[currentSpawn].gameObject.SetActive(true);
                }
                else //Otherwise spawn them 
                {
                    spawnToUse = enemyToSpawn.Spawn(transform, new Vector3(
                                                        0,
                                                        spawnOrder[currentSpawn].spawnHeight,
                                                        transform.position.z));

                    spawnToUse.SetState(MinionState.Migrating, true);
                    spawnToUse.killZone = despawnZoneX;
                    spawns.Add(spawnToUse);
                }
                currentSpawn += 1;
            }
        }
    }

    public void Unsubscribe(TimeObjectManager tManager)
    {
        tManager.OnRestartLevel -= ResetTime;
    }

    private void ResetTime()
    {
        foreach(FlightMinion spawn in spawns)
        {
            spawn.gameObject.SetActive(false);
        }

        timePassed = 0;
        currentSpawn = 0;
    }
}
