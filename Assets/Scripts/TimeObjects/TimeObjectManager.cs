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
    public static int t = 0;

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

    /// <summary>
    /// Scaling for fastforward of boss and past objects
    /// </summary>
    public static float pastTimeScale = 1;

    public static int GameScale
    {
        get
        {
            return (int)timeState * (int)Time.timeScale * (int)pastTimeScale;
        }
    }

    public AnimationCurve rewindCurve;

    public delegate void TimeManagerEvent();
    /// <summary>
    /// Restart of level after Time has rewind following players death
    /// </summary>
    public event TimeManagerEvent OnRestartLevel;

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
        //Increment Game time
        t += (int)Time.timeScale * (int)timeState * (int)pastTimeScale;
    }

    private void LateUpdate()
    {
        if (t < startT)
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
            newTimeScale = rewindCurve.Evaluate((float)t / (float)longestTime);
            Time.timeScale = Mathf.Clamp(newTimeScale, .25f, 100);   
        }
    }

    public void SetStartOfFight()
    {
        startT = t;
    }

    void ReverseTime()
    {
        timeState = TimeState.Backward;

        Keyframe keyframe = new Keyframe(.5f, Mathf.Clamp(longestTime, .1f, 100), 0, 0);

        rewindCurve.MoveKey(1, keyframe);
    }
}