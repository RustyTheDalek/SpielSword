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

    public static bool paused;

    private void Awake()
    {
        VillagerManager.OnNextVillager += TrackNewVillager;
    }

    // Use this for initialization
    void Start ()
    {

        livesTxt = GameObject.Find("livesUsedTxt").GetComponent<Text>();

        bossHealth = GetComponentInChildren<BossHealthBar>(true).GetComponent<RectTransform>();

        currentBoss = GetComponentInChildren<BossManager>();

        if (trackCam == null)
            trackCam = GetComponentInChildren<Camera2DFollow>();

        endGamePnl = GetComponentInChildren<EndGamePnl>();

        arenaGate = GetComponentInChildren<ArenaGate>();
        arenaEntryPoint = GetComponentInChildren<ArenaEntry>();
        arenaEntryPoint.OnPlayerEnterArena += EnableBossUI;
        
        vilManager.Setup(currentBoss, arenaEntryPoint);
        arenaGate.Setup(arenaEntryPoint);
        currentBoss.Setup(arenaEntryPoint);
        timeManager.Setup(arenaEntryPoint, vilManager);

        currentBoss.OnBossDeath += CalculateScore;
        vilManager.OnOutOfLives += LoadVillage;

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
        //Starts at 1 as Boss has obviously died
        score = 1;

        //Check to see if combo was used
        if(ShamanTotem.comboUsed || Aura.comboUsed)
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

        endGamePnl.OpenSlate(currentBoss.highestStageReached, VillagerManager.combosUsed, vilManager.totalLives);
    }

    void TrackNewVillager(Villager newVillager)
    {
        trackCam.target = newVillager.transform;
    }

    private void OnDestroy()
    {
        arenaEntryPoint.OnPlayerEnterArena += EnableBossUI;

        vilManager.Unsubscribe(arenaEntryPoint);
        currentBoss.Unsubscribe(arenaEntryPoint);
        arenaGate.Unsubscribe(arenaEntryPoint);
        timeManager.Unsubscribe(arenaEntryPoint, vilManager);
    }
}
