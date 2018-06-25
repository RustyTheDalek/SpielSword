using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages all the Villages Alived, Past or Dead.
/// Created By      : Ian - GGJ16         
/// Last updated By : Ian - 07/04/18
/// </summary>
public class VillagerManager : MonoBehaviour {

    //Transforms to sort the Villagers
    Transform   activeVillagerTrans,
                remainingVillagersTrans,
                pastVillagersTrans,
                deadVillagersTrans,
                levelStart,
                arenaStart;

    /// <summary>
    /// Where new Villagers Spawn
    /// </summary>
    Vector3 spawnPos;

    /// <summary>
    /// How many combos have been used by the player
    /// </summary>
    public int totalCombos;

    public int villagersToSpawn;

    public Villager activeVillager;

    [SerializeField] List<Villager> remainingVillagers;
    List<Villager> pastVillagers;

    public int RemainingVillagers
    {
        get
        {
            if (remainingVillagers != null)
            {
                return remainingVillagers.Count;
            }
            else
            {
                Debug.LogError("Remaining villagers null");

                return -1;
            }
        }
    }

    public delegate void NewVillagerEvent(Villager newVillager);
    public static NewVillagerEvent OnNextVillager;

    public int totalLives = 1;

    /// <summary>
    /// Render Layer for CurrentVillager
    /// </summary>
    int currentVillagerLayer = 6;

    public VillagerClass classToSpawn = VillagerClass.Warlock;

    public delegate void VillagerManagerEvent();

    /// <summary>
    /// In future will deal with minutia of what happens when a player loses all lives
    /// </summary>
    public event VillagerManagerEvent OnOutOfLives;
    /// <summary>
    /// What happens when active villager dies
    /// </summary>
    public event VillagerManagerEvent OnActiveDeath;

    #region Assets
    Dictionary<string, GameObject> _Villagers;

    Dictionary<string, GameObject> Villagers
    {
        get
        {
            if (_Villagers == null)
            {
                _Villagers = new Dictionary<string, GameObject>();

                Object[] objs = Resources.LoadAll("Villagers");

                GameObject gObj;

                foreach (object obj in objs)
                {
                    if (obj as GameObject != null)
                    {
                        gObj = (GameObject)obj;

                        _Villagers.Add(gObj.name, gObj);
                        _Villagers[gObj.name].CreatePool(50);
                    }
                }
            }

            return _Villagers;
        }
    }

    List<Sprite> _Hats;

    List<Sprite> Hats
    {
        get
        {
            if (_Hats == null)
            {
                _Hats = new List<Sprite>();

                Object[] objs = Resources.LoadAll("Sprites/Hats");

                Sprite sprite;

                foreach (object obj in objs)
                {
                    if (obj as Sprite != null)
                    {
                        sprite = (Sprite)obj;

                        _Hats.Add(sprite);
                    }
                }
            }

            return _Hats;
        }
    }
    #endregion

    void Awake()
    {
        TimeObjectManager.OnNewRoundReady += OnNewRound;

        Transform[] objs = GetComponentsInChildren<Transform>();

        activeVillagerTrans = objs[1];
        remainingVillagersTrans = objs[2];
        pastVillagersTrans = objs[3];
        deadVillagersTrans = objs[4];
        levelStart = objs[5];
        arenaStart = objs[6];

    }

    // Use this for initialization
    public void Setup (BossManager bManager, ArenaEntry arenaEntry)
    {
        //Setup lists
        remainingVillagers = new List<Villager>();
        pastVillagers = new List<Villager>();

        //Get all Villagers and add them to the Villager list
        Vector3 spawnOffset = Vector3.zero;
        for (int i = 0; i < villagersToSpawn; i++)
        {
            spawnOffset += new Vector3(-1.5f, 0, 0);

            classToSpawn = (VillagerClass)Random.Range(0, (int)VillagerClass.Last -1);
            //classToSpawn = VillagerClass.Priest;

            GameObject temp = Villagers[classToSpawn.ToString()].Spawn();
            temp.name = classToSpawn.ToString() + i;
            SetupVillager(temp, spawnOffset);
        }

        //Spawn a new villager and teleport them to the Arena
        spawnPos = levelStart.transform.position;
        NextVillager();
        EnterLevel();

        OnActiveDeath += OnVillagerDeath;

        arenaEntry.OnPlayerEnterArena += ArenaCheckpoint;
    }

    private void SetupVillager(GameObject villager, Vector3 spawnOffset)
    {
        villager.transform.SetParent(remainingVillagersTrans);
        villager.transform.localPosition = spawnOffset;

        remainingVillagers.Add(villager.GetComponent<Villager>());

        villager.SetActive(false);

        if (villager.GetComponent<AuraVillager>())
            villager.GetComponent<AuraVillager>().Setup(this);

        if (villager.GetComponent<Shaman>())
            villager.GetComponent<Shaman>().Setup(this);

    }

    public void IncCombosUsed()
    {
        totalCombos++;
        Debug.Log("Combo has been used");
    }
	
	// Update is called once per frame
	void Update ()
    {

#if UNITY_EDITOR //Debug code to allow killing of Player for testing purposes

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
                remainingVillagers[i].transform.localPosition.x <= i * -2)
            {
                remainingVillagers[i].SetTarget(i * - 2);
            }
        }
	}

    private void LateUpdate()
    {
        switch (TimeObjectManager.timeState)
        {
            case TimeState.Forward:

                if (!activeVillager.Alive)
                {
                    //Game over if no lives
                    if (RemainingVillagers <= 0)
                    {
                        Debug.Log("Game Over");

                        //TODO:On game over probably have Golem keep going and character just dies
                        if (OnOutOfLives != null)
                            OnOutOfLives();
                    }
                    else
                    {
                        if (OnActiveDeath != null)
                            OnActiveDeath();
                    }
                }

                break;
        }
    }

    public void OnVillagerDeath()
    {
        //Turn active Villager into Past Villager
        activeVillager.deathEffect.Play();
        activeVillager.villagerState = VillagerState.PastVillager;
        activeVillager.transform.parent = pastVillagersTrans;
        activeVillager.gameObject.layer = LayerMask.NameToLayer("PastVillager");
        if (activeVillager.melee)
        {
            activeVillager.melee.gameObject.layer = LayerMask.NameToLayer("PastVillager");
        }

        pastVillagers.Add(activeVillager);
    }

    public void OnNewRound()
    {
        NextVillager();
        EnterLevel();
    }

    /// <summary>
    /// Gives player control of next villager
    /// </summary>
    void NextVillager()
    {
        totalLives++;

        if (remainingVillagers.Count > 0)
        {

            //Get the next Villager
            activeVillager = remainingVillagers[0];
            activeVillager.gameObject.SetActive(true);
            activeVillager.villagerState = VillagerState.PresentVillager;
            activeVillager.vTO.tObjectState = TimeObjectState.Present;
            activeVillager.transform.parent = activeVillagerTrans;
            activeVillager.GetComponent<SpriteRenderer>().sortingOrder = currentVillagerLayer;
            remainingVillagers.RemoveAt(0);

            currentVillagerLayer++;

            //Add a random Hat to the active Villager
            activeVillager.hat.GetComponent<SpriteRenderer>().sprite = Hats[Random.Range(0, Hats.Count)];
            activeVillager.hat.GetComponent<SpriteRenderer>().sortingOrder = currentVillagerLayer;

            currentVillagerLayer++;

            if(OnNextVillager != null)
                OnNextVillager(activeVillager);

        }
    }

    /// <summary>
    /// Enters the next Villager into theLevel
    /// </summary>
    void EnterLevel()
    {
        activeVillager.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        activeVillager.GetComponent<Rigidbody2D>().angularVelocity = 0;
        activeVillager.GetComponent<Rigidbody2D>().isKinematic = true;
        activeVillager.transform.position = spawnPos;
        activeVillager.GetComponent<Rigidbody2D>().isKinematic = false;
    }

    /// <summary>
    /// Changes the start point to the Arena once it's been reached
    /// </summary>
    void ArenaCheckpoint()
    {
        Debug.Log("Reached Checkpoint, now spawning in Arena");
        spawnPos = arenaStart.transform.position;
    }

    #region DebugFunctions

    public void KillVillager()
    {
        if (activeVillager)
        {
            activeVillager.Kill();
        }
    }

    #endregion

    public void Unsubscribe(ArenaEntry arenaEntry)
    {
        arenaEntry.OnPlayerEnterArena -= ArenaCheckpoint;

        foreach (Villager villager in remainingVillagers)
        {
            if(villager.GetComponent<AuraVillager>())
                villager.GetComponent<AuraVillager>().Unsubscribe(this);

            if (villager.GetComponent<Shaman>())
                villager.GetComponent<Shaman>().Unsubscribe(this);
        }

        foreach (Villager villager in pastVillagers)
        {
            if (villager.GetComponent<AuraVillager>())
                villager.GetComponent<AuraVillager>().Unsubscribe(this);

            if (villager.GetComponent<Shaman>())
                villager.GetComponent<Shaman>().Unsubscribe(this);
        }
    }

    private void OnDestroy()
    {
        OnActiveDeath -= OnVillagerDeath;
    }
}
