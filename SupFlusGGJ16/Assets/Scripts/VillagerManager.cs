using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VillagerManager : MonoBehaviour {

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

	// Use this for initialization
	void Start ()
    {
        remainingVillagers = new List<Villager>();
        pastVillagers = new List<PastVillager>();

        Villager[] villagers = remainingVillagersTrans.GetComponentsInChildren<Villager>();

        remainingVillagers.AddRange(villagers);

	    if(activeVillager == null)
        {
            Villager player = activeVillagerTrans.GetComponentInChildren<Villager>();

            if (player == null)
            {
                NextVillager();
                EnterArena();
            }
            else
            {
                activeVillager = player;
            }

            activeVillager.activePlayer = true;

            playerActions = new List<Action>();
        }
	}
	
	// Update is called once per frame
	void Update ()
    {

#if UNITY_EDITOR

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            activeVillager.Kill();
        }

#endif

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
        else
        {
            //What needs to happen when active Villager dies
            //1. Level resets to start
            Destroy(currentBoss);
            currentBoss = Instantiate(bossTemplate);
            //2. previous Villagers begin their tasks
            //3. Take control of next Villager in list
            NextVillager();
            //4. Teleport Villager to middle
            EnterArena();
        }

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
                Boss.health = 100;

                activeVillager.activePlayer = false;
                activeVillager.transform.position = activeVillager.startingPos;
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
                activeVillager.m_Character.PresentVillager = false;
            }

            if (playerActions != null)
            {
                playerActions.Clear();
            }

            activeVillager = remainingVillagers[0];
            remainingVillagers.RemoveAt(0);

            activeVillager.activePlayer = true;
            activeVillager.transform.parent = activeVillagerTrans;


            activeVillager.transform.Find("Hat").gameObject.AddComponent<SpriteRenderer>();
            activeVillager.transform.Find("Hat").GetComponent<SpriteRenderer>().sprite = Hats[Random.Range(0, Hats.Count - 1)];
            activeVillager.transform.Find("Hat").GetComponent<SpriteRenderer>().sortingOrder = 5;
            foreach (PastVillager pVillager in pastVillagers)
            {
                pVillager.t = 0;
                pVillager.GetComponent<Animator>().SetTrigger("ExitDeath");
            }
        }
    }

    void EnterArena()
    {
        activeVillager.transform.position = warpGateExit.transform.position; 
    }

}
