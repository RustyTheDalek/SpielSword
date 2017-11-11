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
    [HideInInspector]
    public Transform    activeVillagerTrans,
                        remainingVillagersTrans,
                        pastVillagersTrans,
                        deadVillagersTrans,
                        warpGateEnt,
                        warpGateExit;

    public int villagersToSpawn;
    
    public Villager activeVillager;

    [SerializeField] List<Villager> remainingVillagers;
    List<Villager> pastVillagers;

    Action currentAction;

    public List<Sprite> Hats;

    public static List<MageAura> auras = new List<MageAura>();
    public static List<SpawnableSpriteTimeObject> attacks = new List<SpawnableSpriteTimeObject>();

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

    public VillagerClass classToSpawn = VillagerClass.Warlock;

    int magesSpawned = 0,
        warriorsSpawned = 0,
        warlocksSpawned = 0,
        priestSpawned = 0,
        roguesSpawned = 0;

    void Awake()
    {
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
        for (int i = 0; i < villagersToSpawn; i++)
        {
            GameObject temp = AssetManager.Villager.Spawn();

            spawnOffset += new Vector3(-1, 0, 0);

            //classToSpawn = (VillagerClass)Random.Range(0, (int)VillagerClass.Last -1);
            classToSpawn = VillagerClass.Warlock;

            SetupVillager(temp, spawnOffset);
        }

        //Spawn a new villager and teleport them to the Arena
        NextVillager();
        EnterArena();

        //Debug.Log(AssetManager.VillagerSprites.Count);
	}

    private void SetupVillager(GameObject villager, Vector3 spawnOffset)
    {
        villager.transform.SetParent(remainingVillagersTrans);
        villager.transform.localPosition = spawnOffset;

        switch (classToSpawn)
        {
            case VillagerClass.Warrior:

                villager.AddComponent<Warrior>();
                villager.GetComponent<SpriteRenderer>().color = Color.yellow;
                villager.name = "Warrior " + warriorsSpawned + 1;
                warriorsSpawned++;
                break;

            case VillagerClass.Mage:

                villager.AddComponent<Mage>();
                villager.GetComponent<SpriteRenderer>().color = Color.blue;
                villager.name = "Mage " + magesSpawned + 1;
                magesSpawned++;
                break;

            case VillagerClass.Warlock:

                villager.AddComponent<Warlock>();
                villager.GetComponent<SpriteRenderer>().color = new Color(75f/255f, 0, 130f/255f);
                villager.name = "Warlock " + warlocksSpawned + 1;
                warlocksSpawned++;
                break;

            case VillagerClass.Priest:

                villager.AddComponent<Priest>();
                villager.GetComponent<SpriteRenderer>().color = new Color(1, 1, 224f / 255f);
                villager.name = "Priest " + warlocksSpawned + 1;
                priestSpawned++;
                break;

            case VillagerClass.Rogue:

                villager.AddComponent<Rogue>();
                villager.GetComponent<SpriteRenderer>().color = Color.red;
                villager.name = "Rogue " + warlocksSpawned + 1;
                roguesSpawned++;
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
            classToSpawn = VillagerClass.Warrior;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            classToSpawn = VillagerClass.Mage;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            classToSpawn = VillagerClass.Warlock;
        }
#endif
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
	}

    public void OnVillagerDeath()
    {

        //Turn active Villager into Past Villager
        activeVillager.deathEffect.Play();
        activeVillager.villagerState = VillagerState.PastVillager;
        activeVillager.transform.parent = pastVillagersTrans;
        activeVillager.gameObject.layer = LayerMask.NameToLayer("PastVillager");
        activeVillager.melee.gameObject.layer = LayerMask.NameToLayer("PastVillager");
        //activeVillager.SetTrigger(true);

        //activeVillager.GetComponent<SpriteRenderer>().color = new Color(activeVillager.GetComponent<SpriteRenderer>().color.r,
        //                                                                activeVillager.GetComponent<SpriteRenderer>().color.g,
        //                                                                activeVillager.GetComponent<SpriteRenderer>().color.b,
        //                                                                .5f);

        //activeVillager.transform.Find("Hat").GetComponent<SpriteRenderer>().color = new Color(activeVillager.GetComponent<SpriteRenderer>().color.r,
        //                                                activeVillager.GetComponent<SpriteRenderer>().color.g,
        //                                                activeVillager.GetComponent<SpriteRenderer>().color.b,
        //                                                .5f);

        pastVillagers.Add(activeVillager);

    }

    public void OnNewRound()
    {
        NextVillager();
        EnterArena();
        WeakenAuras();
    }

    private void WeakenAuras()
    {
        foreach (MageAura aura in auras)
        {
            if(aura)
                aura.DecreaseStrength();
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
                Golem.health = BossManager.MAXHEALTH;
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

    /// <summary>
    /// Removes all Villagers that are still alive at time of boss skipping
    /// </summary>
    public void TrimVillagers()
    {
        Debug.Log("Martying Villagers");
        for (int i = 0; i < pastVillagers.Count; i++)
        {
            if (pastVillagers[i].Alive)
            {
                pastVillagers[i].vAnimData.martyed = true;
                pastVillagers[i].GetComponent<VillagerTimeObject>().SetMartyPoint();
            }
        }
    }

    internal void TrimSpawnables()
    {
        Debug.Log("Martying Spawnables");

        for (int i = 0; i < attacks.Count; i++)
        {
            if (attacks[i].finishFrame > Game.t)
            {
                attacks[i].SetMartyPoint();
            }
        }
    }

}
