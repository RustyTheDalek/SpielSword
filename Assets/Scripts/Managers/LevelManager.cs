using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Manages (No shit) the Level itself, handles logic relating to playing the level 
/// handling the skipping of Boss stages when player causes a paradox
/// Created by      : Ian - 24/07/17
/// Last updated    : Ian - 13/04/18
/// </summary>
public class LevelManager : MonoBehaviour {

    public SkipStageType skipStageType = SkipStageType.VillagerWipe;

    #region LevelObjects

    public BossManager currentBoss;

    public VillagerManager vilManager;

    public TimeObjectManager timeManager;

    public FlyingEnemySpawner flyingEnemySpawner;

    public Camera2DFollow trackCam;

    ArenaEntry arenaEntryPoint;
    ArenaGate arenaGate;

    Text livesTxt;

    #endregion

    EndGamePnl endGamePnl;

    /// <summary>
    /// Boss Health UI
    /// </summary>
    RectTransform bossHealth;

    #region Score Variables

    public const int MAXSCORE = 5;

    public int score = 0;

    #endregion

    /// <summary>
    /// Invcinibilty mode for testing
    /// </summary>
    public static bool GodMode = false;

    private void Awake()
    {
        if(vilManager)
            vilManager.OnNextVillager += TrackNewVillager;
    }

    // Use this for initialization
    void Start ()
    {

        livesTxt = GameObject.Find("livesUsedTxt").GetComponent<Text>();

        bossHealth = GetComponentInChildren<BossHealthBar>(true).
            GetComponent<RectTransform>();

        currentBoss = GetComponentInChildren<BossManager>();

        if (trackCam == null)
            trackCam = GetComponentInChildren<Camera2DFollow>();

        endGamePnl = GetComponentInChildren<EndGamePnl>();

        arenaGate = GetComponentInChildren<ArenaGate>();
        arenaEntryPoint = GetComponentInChildren<ArenaEntry>();
        arenaEntryPoint.OnPlayerEnterArena += EnableBossUI;
        
        vilManager.Setup(timeManager, arenaEntryPoint);
        arenaGate.Setup(arenaEntryPoint, timeManager);
        currentBoss.Setup(arenaEntryPoint, vilManager, timeManager);
        timeManager.Setup(arenaEntryPoint, vilManager);

        currentBoss.OnBossDeath += CalculateScore;
        vilManager.OnOutOfLives += CalculateScore;

        flyingEnemySpawner = GetComponentInChildren<FlyingEnemySpawner>(true);
        flyingEnemySpawner.Setup(timeManager);

    }

    void Update()
    {
        if (vilManager)
            livesTxt.text = "x" + vilManager.totalLives;
    }

    void EnableBossUI()
    {
        bossHealth.gameObject.SetActive(true);
    }

    public void LoadVillage()
    {
        SceneManager.LoadScene("Village");
    }

    public void CalculateScore()
    { 
        if(!currentBoss.Alive)
        {
            score++;
        }

        //Check to see if combo was used
        if(vilManager.totalCombos > 0)
        {
            score++;
        }

        //Did you finish the fight in a short number of lives
        if(vilManager.totalLives < 10 )
        {
            score++;
        }

        if(currentBoss.highestStageReached >= 3)
        {
            score++;

            if(currentBoss.highestStageReached >= 4)
            {
                score++;
            }
        }

        endGamePnl.OpenSlate(!currentBoss.Alive, currentBoss.highestStageReached, 
            vilManager.totalCombos, vilManager.totalLives);
    }

    void TrackNewVillager(Villager newVillager)
    {
        trackCam.target = newVillager.GroundController2D.m_Character;
    }

    private void OnApplicationQuit()
    {
        if (arenaEntryPoint)
        {
            arenaEntryPoint.OnPlayerEnterArena -= EnableBossUI;
        }

        if (vilManager && timeManager & arenaEntryPoint)
        {
            vilManager.OnNextVillager -= TrackNewVillager;
            vilManager.OnOutOfLives -= CalculateScore;
            vilManager.Unsubscribe(timeManager, arenaEntryPoint);
        }

        if (currentBoss && arenaEntryPoint && vilManager && timeManager)
        {
            currentBoss.Unsubscribe(arenaEntryPoint, vilManager, timeManager);
            currentBoss.OnBossDeath -= CalculateScore;
        }

        if (arenaGate && arenaEntryPoint && timeManager)
        {
            arenaGate.Unsubscribe(arenaEntryPoint, timeManager);
        }

        if (timeManager && arenaEntryPoint && vilManager)
        {
            timeManager.Unsubscribe(arenaEntryPoint, vilManager);
        }

        if (flyingEnemySpawner && timeManager)
        {
            flyingEnemySpawner.Unsubscribe(timeManager);
        }
    }
}
