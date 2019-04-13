﻿using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Collections;

/// <summary>
/// Manages all the Villages Alived, Past or Dead.
/// Created By      : Ian - GGJ16         
/// Last updated By : Ian - 07/04/18
/// </summary>
public class VillagerManager : MonoBehaviour {

    //Transforms to sort the Villagers
    public Transform    activeVillagerTrans,
                        remainingVillagersTrans,
                        pastVillagersTrans,
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

    public Animator portal;

    public AudioSource EN;

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

    public int totalLives = 1;

    /// <summary>
    /// Render Layer for CurrentVillager
    /// </summary>
    int currentVillagerLayer = 6;

    bool endSlateActive = false;

    VillagerClass classToSpawn;

    [Header("Spawning override")]
    public bool overrideSpawn;
    public VillagerClass overrideClass;

    public delegate void VillagerManagerEvent();
    public delegate void NewVillagerEvent(Villager newVillager);

    /// <summary>
    /// In future will deal with minutia of what happens when a player loses all lives
    /// </summary>
    public event VillagerManagerEvent OnOutOfLives;
    /// <summary>
    /// What happens when active villager dies
    /// </summary>
    public event VillagerManagerEvent OnActiveDeath;

    public event NewVillagerEvent OnNextVillager;

    public MusicManager musicManager;

    public AudioClip bossMusic;

    #region Assets
    public List<Villager> villagerPrefabs;

    //AssetBundle villagers;

    #endregion

    void Awake()
    {
        Transform[] objs = GetComponentsInChildren<Transform>();

        activeVillagerTrans = objs[1];
        remainingVillagersTrans = objs[2];
        pastVillagersTrans = objs[3];
        levelStart = objs[5];
        arenaStart = objs[6];

        musicManager = FindObjectOfType<MusicManager>();


        portal = GetComponentInChildren<Animator>();
    }

    // Use this for initialization
    public void Setup(TimeObjectManager timeManager, ArenaEntry arenaEntry)
    {
        //Setup lists
        remainingVillagers = new List<Villager>();
        pastVillagers = new List<Villager>();

        //Get all Villagers and add them to the Villager list
        Vector3 spawnOffset = Vector3.zero;
        for (int i = 0; i < villagersToSpawn; i++)
        {
            spawnOffset += new Vector3(-1.5f, 0, 0);

            if (overrideSpawn)
                classToSpawn = overrideClass;
            else
                classToSpawn = (VillagerClass)Random.Range(0, (int)VillagerClass.Last);

            //TODO:Change creatpool size to be whatever the maximum number of villagers you can have in that level is
            villagerPrefabs[(int)classToSpawn].CreatePool(50);
            Villager temp = villagerPrefabs[(int)classToSpawn].Spawn();
            temp.name = classToSpawn.ToString() + i;
            SetupVillager(temp.gameObject, spawnOffset);
        }

        //Spawn a new villager and teleport them to the Arena
        spawnPos = levelStart.transform.position;
        NextVillager();
        EnterLevel();

        OnActiveDeath += OnVillagerDeath;

        arenaEntry.OnPlayerEnterArena += ArenaCheckpoint;
        timeManager.OnRestartLevel += StartFightWNewVillager;
    }

    private void SetupVillager(GameObject villager, Vector3 spawnOffset)
    {
        villager.transform.SetParent(remainingVillagersTrans);
        villager.transform.localPosition = spawnOffset;

        remainingVillagers.Add(villager.GetComponent<Villager>());

        villager.SetActive(false);

        if (classToSpawn == VillagerClass.Paladin)
            villager.GetComponent<Paladin>().Setup(this);

        if (classToSpawn == VillagerClass.Shaman)
            villager.GetComponent<Shaman>().currentWard.GetComponent<ShamanTotem>().Setup(this);
    }

    public void IncCombosUsed()
    {
        totalCombos++;
        Debug.Log("Combo has been used");
    }

    void Update()
    {

#if UNITY_EDITOR //Debug code to allow killing of Player for testing purposes

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            overrideClass = VillagerClass.Berserker;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            overrideClass = VillagerClass.Mage;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            overrideClass = VillagerClass.Paladin;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            overrideClass = VillagerClass.Rogue;
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            overrideClass = VillagerClass.Shaman;
        }
#endif
    }

    private void LateUpdate()
    {
        switch (TimeObjectManager.timeState)
        {
            case TimeState.Forward:

                //When the active Villager has died and the animation is finished
                if (!activeVillager.Alive && activeVillager.GetComponent<TimeObject>().finished)
                {
                    //Game over if no lives
                    if (RemainingVillagers <= 0 && !endSlateActive)
                    {
                        Debug.Log("Game Over");

                        //TODO:On game over probably have Golem keep going and character 
                        //just dies
                        if (OnOutOfLives != null)
                            OnOutOfLives();

                        endSlateActive = true;
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
        portal.transform.position = activeVillager.portal.transform.position;
        portal.SetTrigger("Grow");
        PlayEffect();

        StartCoroutine(DelayedJump(1.5f));

        //Turn active Villager into Past Villager
        activeVillager.villagerState = VillagerState.PastVillager;
        activeVillager.transform.parent = pastVillagersTrans;
        activeVillager.gameObject.layer = LayerMask.NameToLayer("PastVillager");
        //if (activeVillager.melee)
        //{
        //    activeVillager.melee.gameObject.layer = 
        //        LayerMask.NameToLayer("PastVillager");
        //}

        pastVillagers.Add(activeVillager);
    }

    /// <summary>
    /// Waits a given time then jumps time back
    /// </summary>
    /// <param name="secondsToWait"></param>
    /// <returns></returns>
    public IEnumerator DelayedJump(float secondsToWait)
    {
        yield return new WaitForSecondsRealtime(secondsToWait);

        TimeObjectManager.t = TimeObjectManager.startT + 5;

        portal.transform.position = levelStart.position;
        portal.SetTrigger("Shrink");

    }

    public void StartFightWNewVillager()
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
            if (activeVillager)
            {
                activeVillager.GetComponent<TimeObject>().tObjectState = TimeObjectState.PastStart;
                activeVillager.DisableAnimator();
                activeVillager.EnableOutsideMask();
            }


            //Get the next Villager
            activeVillager = remainingVillagers[0];
            activeVillager.gameObject.SetActive(true);
            activeVillager.villagerState = VillagerState.PresentVillager;
            activeVillager.transform.parent = activeVillagerTrans;
            activeVillager.Sprite.sortingOrder = currentVillagerLayer;
            remainingVillagers.RemoveAt(0);

            currentVillagerLayer++;

            //Add a random Hat to the active Villager
            if (GameManager.gManager.CurrentSave != null && GameManager.gManager.CurrentSave.Hat != null)
                activeVillager.hat.sprite = GameManager.gManager.hats.LoadAsset<Hat>(GameManager.gManager.CurrentSave.Hat).hatDesign;

            activeVillager.hat.sortingOrder = currentVillagerLayer;

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
        activeVillager.Rigidbody.position = spawnPos;
    }

    /// <summary>
    /// Changes the start point to the Arena once it's been reached
    /// </summary>
    void ArenaCheckpoint()
    {
        Debug.Log("Reached Checkpoint, now spawning in Arena");
        spawnPos = arenaStart.transform.position;
        musicManager.ChangeLM(bossMusic);
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

    public void Unsubscribe(TimeObjectManager timeManager,ArenaEntry arenaEntry)
    {
        if(timeManager)
            timeManager.OnRestartLevel -= StartFightWNewVillager;

        if(arenaEntry)
            arenaEntry.OnPlayerEnterArena -= ArenaCheckpoint;

        if (remainingVillagers != null)
        {
            foreach (Villager villager in remainingVillagers)
            {
                if (villager.GetComponent<Paladin>())
                    villager.GetComponent<Paladin>().Unsubscribe(this);

                if (villager.GetComponent<Shaman>())
                    villager.GetComponent<Shaman>().currentWard.GetComponent<ShamanTotem>().Unsubscribe(this);
            }
        }


        if (pastVillagers != null)
        {
            foreach (Villager villager in pastVillagers)
            {
                if (villager.GetComponent<Paladin>())
                    villager.GetComponent<Paladin>().Unsubscribe(this);

                if (villager.GetComponent<Shaman>())
                    villager.GetComponent<Shaman>().currentWard.GetComponent<ShamanTotem>().Unsubscribe(this);
            }
        }
    }

    private void PlayEffect()
    {
        EN.Stop();
        EN.Play();
    }

    private void OnDestroy()
    {
        OnActiveDeath -= OnVillagerDeath;
    }
}
