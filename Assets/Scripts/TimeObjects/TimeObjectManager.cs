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
    public static List<SpawnableSpriteTimeObject> vSpawnable = new List<SpawnableSpriteTimeObject>();

    VillagerManager vilManager;

    // Use this for initialization
    void Start ()
    {
        //Retreive all relevant Time Objects
        TimeObject[] tObjs = GetComponentsInChildren<TimeObject>();

        foreach (TimeObject tObj in tObjs)
        {
            tObjects.Add(tObj);
        }

        //VillagerTimeObject[] vObjs = GetComponentsInChildren<VillagerTimeObject>();

        //foreach (VillagerTimeObject vObj in vObjs)
        //{
        //    vObjects.Add(vObj);
        //}

        AssetManager.Projectile.name = "Range";

        vilManager = GetComponentInChildren<VillagerManager>();
    }

    // Update is called once per frame
    void Update()
    {

#if UNITY_EDITOR

        if (Input.GetKeyDown(KeyCode.R))
        {
            //For the purposes of testing this will also set any Present time objects to past
            Game.t = 0;

            foreach (TimeObject tObj in tObjects)
            {
                tObj.HardReset();
            }

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
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
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
    }

    private void LateUpdate()
    {
        //Increment Game time
        Game.t += (int)Time.timeScale * (int)Game.timeState * (int)Game.PastTimeScale;

        if (Game.timeState == TimeState.Forward)
        {
            if (Game.t > Game.longestTime)
                Game.longestTime = Game.t;
        }
        else
        {
            float x = Mathf.InverseLerp(0, Game.longestTime, Game.t);
            float newTimeScale = -Mathf.Pow(x, 2) + (4 * x) + 1;
            Time.timeScale = newTimeScale;

            if (Game.t < 0)
            {
                Game.t = 0;
                Game.timeState = TimeState.Forward;

                try
                {
                    vilManager.OnNewRound();
                }
                catch
                {
                    Debug.LogWarning("No Villager manager found, are you testing Time stuff without it?");
                }
            }
        }
    }
}
