using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages Objects that are affected by Time
/// </summary>
public class TimeObjectManager : MonoBehaviour
{

    public static List<TimeObject> tObjects = new List<TimeObject>();
    public static List<VillagerTimeObject> vObjects = new List<VillagerTimeObject>();
    public static List<SpriteTimeObject> spriteObjects = new List<SpriteTimeObject>();
    public static List<BossTimeObject> bossObjs = new List<BossTimeObject>();
    public static List<SpawnableSpriteTimeObject> vSpawnable = new List<SpawnableSpriteTimeObject>();
    public static List<PlatformerTimeObject> platformers = new List<PlatformerTimeObject>();

    public bool newRoundReady;

    public AnimationCurve rewindCurve;

    // Use this for initialization
    void Start ()
    {
        //Retreive all relevant Time Objects
        //TimeObject[] tObjs = GetComponentsInChildren<TimeObject>();

        //foreach (TimeObject tObj in tObjs)
        //{
        //    tObjects.Add(tObj);
        //}

        //VillagerTimeObject[] vObjs = GetComponentsInChildren<VillagerTimeObject>();

        //foreach (VillagerTimeObject vObj in vObjs)
        //{
        //    vObjects.Add(vObj);
        //}

        AssetManager.Projectile.name = "Range";
    }

    // Update is called once per frame
    void Update()
    {

        //Increment Game time
        Game.t += (int)Time.timeScale * (int)Game.timeState * (int)Game.PastTimeScale;

#if UNITY_EDITOR

        if (Input.GetKeyDown(KeyCode.R))
        {
            //For the purposes of testing this will also set any Present time objects to past
            Game.t = 0;

            //foreach (TimeObject tObj in tObjects)
            //{
            //    tObj.HardReset();
            //}

            foreach (VillagerTimeObject vObj in vObjects)
            {
                vObj.HardReset();
            }

            foreach (SpawnableSpriteTimeObject vAtt in vSpawnable)
            {
                vAtt.HardReset();
            }

            foreach (SpriteTimeObject sObj in spriteObjects)
            {
                sObj.HardReset();
            }

            foreach (BossTimeObject bObj in bossObjs)
            {
                bObj.HardReset();
            }

            foreach (PlatformerTimeObject pObj in platformers)
            {
                pObj.HardReset();
            }

        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Time Reversed");
            Game.timeState = TimeState.Backward;
            SoftReset();
        }

#endif
    }

    public static void SoftReset()
    {
        //Game.timeState = TimeState.Backward;

        //Time.timeScale = .25f;

        foreach (TimeObject tObj in tObjects)
        {
            tObj.SoftReset();
        }

        foreach (VillagerTimeObject vObj in vObjects)
        {
            vObj.SoftReset();
        }

        foreach (SpawnableSpriteTimeObject vAtt in vSpawnable)
        {
            vAtt.SoftReset();
        }

        foreach (SpriteTimeObject sObj in spriteObjects)
        {
            sObj.SoftReset();
        }

        foreach (BossTimeObject bObj in bossObjs)
        {
            bObj.SoftReset();
        }

        foreach (PlatformerTimeObject pObj in platformers)
        {
            pObj.SoftReset();
        }
    }

    private void LateUpdate()
    {
        if (Game.t < 0)
        {
            Game.t = 0;

            Game.timeState = TimeState.Forward;
            Time.timeScale = 1;

            try
            {
                newRoundReady = true;
            }
            catch
            {
                Debug.LogWarning("No Villager manager found, are you testing Time stuff without it?");
            }
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
            Time.timeScale = newTimeScale;

            
        }
    }
}
