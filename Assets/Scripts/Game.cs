using UnityEngine;
using System.Collections;

public static class Game
{
    public static TimeState timeState = TimeState.Forward;
    public static SkipStageType skipStageType = SkipStageType.VillagerWipe;
    public static BossState bossState = BossState.Waking;

    /// <summary>
    /// Custom Time Variable for past Villagers
    /// </summary>
    public static int t = 0;

    /// <summary>
    /// Scaling for Time Helps speed up reversal
    /// </summary>
    public static float longestTime = 0;

    /// <summary>
    /// Invcinibilty mode for testing
    /// </summary>
    public static bool GodMode = false;

    /// <summary>
    /// Whether the stage was met early.
    /// </summary>
    public static bool StageMetEarly;

    public static bool bossReady = false;

    public static bool paused;
    
    /// <summary>
    /// If the Boss is attackable
    /// </summary>
    public static bool attackable = true;

    public static bool debugText = true;

    public const int MAXSCORE = 5;

    private static int _Score = 0;

    public static void IncScore()
    {
        _Score++;

        Debug.Log("Score increased");
    }

    public static bool  ReachedStage3,
                        ReachedStage5,
                        ComboAchieved,
                        LessThanTenLives;

    public static int TotalScore
    {
        get
        {
            return _Score;
        }
    }

#if UNITY_EDITOR

    public static GUIStyle debugStyle = new GUIStyle();

#endif

    /// <summary>
    /// Scaling for fastforward of boss and past objects
    /// </summary>
    public static float PastTimeScale = 1;

    public static int GameScale
    {
        get
        {
            return (int)Game.timeState * (int)Time.timeScale * (int)PastTimeScale;
        }
    }
}
