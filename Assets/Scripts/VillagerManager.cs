using UnityEngine;
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

    
    public Villager activeVillager;

    [SerializeField] List<Villager> remainingVillagers;
    List<Villager> pastVillagers;

    Action currentAction;

    public BossManager   bossTemplate,
                        currentBoss;

    public List<Sprite> Hats;

    public static RuntimeAnimatorController[] villagerAnimators = new RuntimeAnimatorController[2];

    public static List<MageAura> auras = new List<MageAura>();

    public static int totalLives = 0;

#if UNITY_EDITOR
    /// <summary>
    /// Editor variables to view statics
    /// </summary>
    public float myT;
    public float myTimeScale;
#endif

    /// <summary>
    /// Render Layer for CurrentVillager
    /// </summary>
    int currentVillagerLayer = 6;

    VillagerClass ClassToSpawn = VillagerClass.Warrior;

    void Awake()
    {
        //Retrieve Animators
        villagerAnimators = Resources.LoadAll<RuntimeAnimatorController>("VAnimators");
    }

    // Use this for initialization
    void Start ()
    {
        //Setup lists
        remainingVillagers = new List<Villager>();
        pastVillagers = new List<Villager>();

        //Get all Villagers and add them to the Villager list
        //Villager[] villagers = remainingVillagersTrans.GetComponentsInChildren<Villager>();
        //remainingVillagers.AddRange(villagers
        Vector3 spawnOffset = Vector3.zero;
        for (int i = 0; i < 5; i++)
        {
            GameObject temp = AssetManager.villager.Spawn();

            spawnOffset += new Vector3(-1, 0, 0);

            SetupVillager(temp, spawnOffset);
        }

        //Spawn a new villager and teleport them to the Arena
        NextVillager();
        EnterArena();

        Debug.Log(AssetManager.villagerSprites.Count);
	}

    private void SetupVillager(GameObject villager, Vector3 spawnOffset)
    {
        villager.transform.SetParent(remainingVillagersTrans);
        villager.transform.localPosition = spawnOffset;
        villager.name = "Spiel " + -spawnOffset.x;

        switch (ClassToSpawn)
        {
            case VillagerClass.Warrior:

                villager.AddComponent<Warrior>();
                break;

            case VillagerClass.Mage:

                villager.AddComponent<Mage>();
                break;

            case VillagerClass.Warlock:

                villager.AddComponent<Warlock>();
                break;
        }

        remainingVillagers.Add(villager.GetComponent<Villager>());
    }
	
	// Update is called once per frame
	void Update ()
    {

#if UNITY_EDITOR //Debug code to allow killing of Player for testing purposes

        //myTimeScale = Time.timeScale;

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            activeVillager.Kill();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ClassToSpawn = VillagerClass.Warrior;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ClassToSpawn = VillagerClass.Mage;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ClassToSpawn = VillagerClass.Warlock;
        }
#endif

        switch (Game.timeState)
        {
            case TimeState.Forward:

                if(!activeVillager.alive)//Game world needs to be reset
                {
                    TimeObjectManager.SoftReset();

                    foreach (MageAura aura in auras)
                    {
                        aura.DecreaseStrength();
                    }

                    //Reverse time
                    Game.timeState = TimeState.Backward;

                    //Turn active Villager into Past Villager
                    activeVillager.deathEffect.Play();
                    activeVillager.villagerState = VillagerState.PastVillager;
                    activeVillager.transform.parent = pastVillagersTrans;
                    activeVillager.gameObject.layer = LayerMask.NameToLayer("PastVillager");
                    activeVillager.melee.gameObject.layer = LayerMask.NameToLayer("PastVillager");
                    //activeVillager.SetTrigger(true);

                    activeVillager.GetComponent<SpriteRenderer>().color = new Color(activeVillager.GetComponent<SpriteRenderer>().color.r,
                                                                                    activeVillager.GetComponent<SpriteRenderer>().color.g,
                                                                                    activeVillager.GetComponent<SpriteRenderer>().color.b,
                                                                                    .5f);

                    activeVillager.transform.Find("Hat").GetComponent<SpriteRenderer>().color = new Color(activeVillager.GetComponent<SpriteRenderer>().color.r,
                                                                    activeVillager.GetComponent<SpriteRenderer>().color.g,
                                                                    activeVillager.GetComponent<SpriteRenderer>().color.b,
                                                                    .5f);

                    pastVillagers.Add(activeVillager);

                    currentBoss.GetComponent<BossManager>().SetAnimators(false);
                }

                break;

            case TimeState.Backward:

                //float x = Mathf.InverseLerp(0, Game.longestTime, Game.t);
                //float newTimeScale = -Mathf.Pow(x, 2) + (4 * x) + 1;
                //Time.timeScale = newTimeScale;

                break;
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

        Game.t += (int)Time.timeScale * (int)Game.timeState;
	}

    public void OnNewRound()
    {
        currentBoss.Reset();
        NextVillager();
        EnterArena();
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
            activeVillager.villagerState = VillagerState.PresentVillager;
            activeVillager.vTO.tObjectState = TimeObjectState.Present;
            activeVillager.transform.parent = activeVillagerTrans;
            activeVillager.GetComponent<SpriteRenderer>().sortingOrder = currentVillagerLayer;
            TimeObjectManager.vObjects.Add(activeVillager.GetComponent<VillagerTimeObject>());
            remainingVillagers.RemoveAt(0);

            currentVillagerLayer++;

            //Add a random Hat to the active Villager
            activeVillager.hat.gameObject.AddComponent<SpriteRenderer>();
            activeVillager.hat.GetComponent<SpriteRenderer>().sprite = Hats[Random.Range(0, Hats.Count)];
            activeVillager.hat.GetComponent<SpriteRenderer>().sortingOrder = currentVillagerLayer;

            currentVillagerLayer++;

            //Reset all the past Villagers
            //foreach (PastVillager pVillager in pastVillagers)
            //{
                //pVillager.GetComponent<Animator>().SetTrigger("ExitDeath");
            //}
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
