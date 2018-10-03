using System.Collections;
using UnityEngine;

/// <summary>
/// Manages Objects that are affected by Time
/// Created by : Ian Jones - 19/03/17
/// Updated by : Ian Joens - 13/04/18
/// </summary>
public class TimeObjectManager : MonoBehaviour
{
    /// <summary>
    /// current game time, 0 = start of level.
    /// </summary>
    public static float t = 0;

    /// <summary>
    /// Time on previous frame
    /// </summary>
    public static float prevT;

    /// <summary>
    /// Difference in time between frames
    /// </summary>
    public static float DeltaT
    {
        get
        {
            return t - prevT;
        }
    }

    /// <summary>
    /// When does 'Time' start. changes when player reaches boss fight
    /// </summary>
    public static int startT;

    /// <summary>
    /// Scaling for Time Helps speed up reversal
    /// </summary>
    public float longestTime = 0;

    public float newTimeScale;

    public static TimeState timeState = TimeState.Forward;

    public static float GameScale
    {
        get
        {
            return (int)timeState * Time.timeScale;
        }
    }

    public AnimationCurve rewindCurve;

    public delegate void TimeManagerEvent();
    /// <summary>
    /// Restart of level after Time has rewind following players death
    /// </summary>
    public event TimeManagerEvent OnRestartLevel;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    public void Setup(ArenaEntry arenaEntry, VillagerManager villagerManager)
    {
        arenaEntry.OnPlayerEnterArena += SetStartOfFight;
        villagerManager.OnActiveDeath += ReverseTime;
    }

    public void Unsubscribe(ArenaEntry arenaEntry, VillagerManager villagerManager)
    {
        arenaEntry.OnPlayerEnterArena -= SetStartOfFight;
        villagerManager.OnActiveDeath -= ReverseTime;
    }

    // Update is called once per frame
    void Update()
    {
        prevT = t;
        //Increment Game time
        t += Time.timeScale * (int)timeState;
    }

    private void LateUpdate()
    {
        if (t < startT && TimeObjectManager.timeState == TimeState.Backward)
        {
            //Skips time ahead to when fight starts
            t = startT;

            timeState = TimeState.Forward;
            Time.timeScale = 1;

            if (OnRestartLevel != null)
                OnRestartLevel();
        }

        if (timeState == TimeState.Forward)
        {
            if (t > longestTime)
                longestTime = t;
        }
        else
        {
            //newTimeScale = rewindCurve.Evaluate((float)t - startT / (float)longestTime);
            //Time.timeScale = Mathf.Clamp(newTimeScale, .25f, 100);   
        }
    }

    public void SetStartOfFight()
    {
        startT = (int)t;
    }

    IEnumerator JumpTime()
    {
        yield return new WaitForSecondsRealtime(3);

        t = startT + Mathf.Clamp(longestTime, longestTime, 30);
    }

    public static void ReverseTime()
    {
        timeState = TimeState.Backward;
    }
}