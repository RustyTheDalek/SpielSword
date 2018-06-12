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
    /// Scaling for Time Helps speed up reversal
    /// </summary>
    public float longestTime = 0;

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

    public delegate void NewRoundReadyEvent();
    public static event NewRoundReadyEvent OnNewRoundReady;

    // Use this for initialization
    void Start ()
    {
        LevelManager.OnPlayerDeath += SetMaxReverseSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //Increment Game time
        t += (int)Time.timeScale * (int)timeState * (int)pastTimeScale;
    }

    void SetMaxReverseSpeed()
    {
        Keyframe keyframe = new Keyframe(.5f, Mathf.Clamp(longestTime / 60, .1f, 100));

        rewindCurve.MoveKey(1, keyframe);
    }

    private void LateUpdate()
    {
        if (t < LevelManager.fightStart)
        {
            //Skips time ahead to when fight starts
            t = LevelManager.fightStart;

            timeState = TimeState.Forward;
            Time.timeScale = 1;

            if (OnNewRoundReady != null)
                OnNewRoundReady();
        }

        if (timeState == TimeState.Forward)
        {
            if (t > longestTime)
                longestTime = t;
        }
        else
        {
            float newTimeScale = rewindCurve.Evaluate((float)t / (float)longestTime);
            Time.timeScale = Mathf.Clamp(newTimeScale, .25f, 100);   
        }
    }
}