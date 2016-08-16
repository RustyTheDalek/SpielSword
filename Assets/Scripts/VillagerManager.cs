﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Manages all the Villages Alived, Past or Dead.
/// Created By      : Ian - GGJ16         
/// Last updated By : Ian - 12/08/16
/// </summary>
public class VillagerManager : MonoBehaviour {

    //Transforms to sort the Villagers
    public Transform    activeVillagerTrans,
                        remainingVillagersTrans,
                        pastVillagersTrans,
                        deadVillagersTrans,
                        warpGateEnt,
                        warpGateExit;

    
    Villager activeVillager;

    [SerializeField] List<Villager> remainingVillagers;
    List<PastVillager> pastVillagers;

    List<Action> playerActions;

    Action currentAction;

    public GameObject   bossTemplate,
                        currentBoss;

    public List<Sprite> Hats;

    public static int totalLives = 0;

    /// <summary>
    /// Custom Time Variable for past Villagers
    /// </summary>
    public static float t;

    /// <summary>
    /// Time Scale for Past Villagers
    ///  1 = Normal Time
    /// -1 = Reversed Time;
    /// </summary>
    public static float villagerTimeScale = 1;

    /// <summary>
    /// Scaling for Time Helps speed up reversal
    /// </summary>
    public float longestTime = 0;

    /// <summary>
    /// Whether the World is waiting to be reset
    /// </summary>
    bool resetWorld = false;

#if UNITY_EDITOR
    /// <summary>
    /// Editor variables to view statics
    /// </summary>
    public float myT;
    public float myTimeScale;
#endif

    // Use this for initialization
    void Start ()
    {
        //Setup lists
        remainingVillagers = new List<Villager>();
        pastVillagers = new List<PastVillager>();
        playerActions = new List<Action>();

        //Get all Villagers and add them to the Villager list
        Villager[] villagers = remainingVillagersTrans.GetComponentsInChildren<Villager>();
        remainingVillagers.AddRange(villagers);

        //Spawn a new villager and teleport them to the Arena
        NextVillager();
        EnterArena();
	}
	
	// Update is called once per frame
	void Update ()
    {

#if UNITY_EDITOR //Debug code to allow killing of Player for testing purposes

        myT = t;
        myTimeScale = villagerTimeScale;

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            activeVillager.Kill();
        }
#endif

        //Adds a new action every frame to record the players input that frame
        if (activeVillager.alive)
        {
            currentAction = new Action();
            currentAction.timeStamp = Time.timeSinceLevelLoad;
            currentAction.pos = activeVillager.transform.position;
            currentAction.move = activeVillager.xDir;
            currentAction.attack = Input.GetKey(KeyCode.DownArrow);
            currentAction.health = activeVillager.health;

            playerActions.Add(currentAction);
        }
        else if(!resetWorld) //Game world needs to be reset
        {
            resetWorld = true;
            villagerTimeScale = -1;
            longestTime = t;

            activeVillager.deathEffect.Play();
            activeVillager.villagerState = VillagerState.PastVillager;
            activeVillager.gameObject.AddComponent<PastVillager>();
            activeVillager.GetComponent<PastVillager>().Setup(playerActions);
            activeVillager.transform.parent = pastVillagersTrans;
            activeVillager.gameObject.layer = LayerMask.NameToLayer("PastVillager");
            activeVillager.GetComponent<SpriteRenderer>().color = new Color(activeVillager.GetComponent<SpriteRenderer>().color.r,
                                                                            activeVillager.GetComponent<SpriteRenderer>().color.g,
                                                                            activeVillager.GetComponent<SpriteRenderer>().color.b,
                                                                            .5f);

            activeVillager.transform.Find("Hat").GetComponent<SpriteRenderer>().color = new Color(activeVillager.GetComponent<SpriteRenderer>().color.r,
                                                            activeVillager.GetComponent<SpriteRenderer>().color.g,
                                                            activeVillager.GetComponent<SpriteRenderer>().color.b,
                                                            .5f);

            pastVillagers.Add(activeVillager.GetComponent<PastVillager>());

            if (playerActions != null)
            {
                playerActions.Clear();
            }
        }

        //Set remaining Villagers to Queue appropriately
        for(int i = 0; i < remainingVillagers.Count; i++)
        {
            //If Villager is not moving forward and not in his correct place
            if(!remainingVillagers[i].advancing &&
                remainingVillagers[i].transform.localPosition.x < i * -2)
            {
                //Debug.Log("Villager " + i + " is not in his correct place");
                remainingVillagers[i].SetTarget(i * - 2);
            }
        }

        t += villagerTimeScale;

        if (t % 100 == 0 && villagerTimeScale < 1)
        {
            villagerTimeScale *= 2;
        }
	}

    void FixedUpdate()
    {
        if (resetWorld && t <= 0)
        {
            t = 0;
            villagerTimeScale = 1;
            longestTime = 0;
            Destroy(currentBoss);
            currentBoss = Instantiate(bossTemplate);
            NextVillager();
            EnterArena();

            resetWorld = false;
        }
    }

    /// <summary>
    /// Gives player control of next villager
    /// </summary>
    void NextVillager()
    {
        totalLives++;

        if (remainingVillagers.Count > 0)
        {
            //Prevent previous Villager from being controlled
            if (activeVillager)
            {
                Golem.health = 100;
            }

            //Get the next Villager
            activeVillager = remainingVillagers[0];
            activeVillager.villagerState = VillagerState.CurrentVillager;
            activeVillager.transform.parent = activeVillagerTrans;
            remainingVillagers.RemoveAt(0);

            //Add a random Hat to the active Villager
            activeVillager.transform.Find("Hat").gameObject.AddComponent<SpriteRenderer>();
            activeVillager.transform.Find("Hat").GetComponent<SpriteRenderer>().sprite = Hats[Random.Range(0, Hats.Count - 1)];
            activeVillager.transform.Find("Hat").GetComponent<SpriteRenderer>().sortingOrder = 5;

            //Reset all the past Villagers
            foreach (PastVillager pVillager in pastVillagers)
            {
                pVillager.GetComponent<Animator>().SetTrigger("ExitDeath");
            }
        }
    }

    //Move active Villager to the Arena
    void EnterArena()
    {
        activeVillager.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        activeVillager.GetComponent<Rigidbody2D>().angularVelocity = 0;
        activeVillager.GetComponent<Rigidbody2D>().isKinematic = true;
        activeVillager.transform.position = warpGateExit.transform.position;
        activeVillager.GetComponent<Rigidbody2D>().isKinematic = false;
    }

}
