using UnityEngine;

/// <summary>
/// Manages Objects that are affected by Time
/// Created by : Ian Jones - 19/03/17
/// Updated by : Ian Joens - 06/04/18
/// </summary>
public class TimeObjectManager : MonoBehaviour
{

    public delegate void NewRoundReadyEvent();
    public static event NewRoundReadyEvent OnNewRoundReady;

    public AnimationCurve rewindCurve;

    // Use this for initialization
    void Start ()
    {
        GameManager.OnPlayerDeath += SetMaxReverseSpeed;
    }

    // Update is called once per frame
    void Update()
    {

        //Increment Game time
        Game.t += (int)Time.timeScale * (int)Game.timeState * (int)Game.PastTimeScale;
    }

    void SetMaxReverseSpeed()
    {
        Keyframe keyframe = new Keyframe(.5f, Mathf.Clamp(Game.longestTime / 60, .1f, 20));

        rewindCurve.MoveKey(1, keyframe);
    }

    private void LateUpdate()
    {
        if (Game.t < Game.FightStart)
        {
            //Skips time ahead to when fight starts
            Game.t = Game.FightStart;

            Game.timeState = TimeState.Forward;
            Time.timeScale = 1;

            if (OnNewRoundReady != null)
                OnNewRoundReady();
        }

        if (Game.timeState == TimeState.Forward)
        {
            if (Game.t > Game.longestTime)
                Game.longestTime = Game.t;
        }
        else
        {
            //Currently not using X while we're testing
            float x = rewindCurve.Evaluate((float)Game.t / (float)Game.longestTime);
            float newTimeScale = x;
            //Time.timeScale = newTimeScale;   
        }
    }
}