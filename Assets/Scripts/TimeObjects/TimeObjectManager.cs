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

    // Use this for initialization
    void Start ()
    {                                   
        //Retreive all relevant Time Objects
        TimeObject[] tObjs = GetComponentsInChildren<TimeObject>();

        foreach (TimeObject tObj in tObjs)
        {
            tObjects.Add(tObj);
        }

        VillagerTimeObject[] vObjs = GetComponentsInChildren<VillagerTimeObject>();

        foreach (VillagerTimeObject vObj in vObjs)
        {
            vObjects.Add(vObj);
        }
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
                tObj.Reset();
            }

            foreach (VillagerTimeObject vObj in vObjects)
            {
                vObj.Reset();
            }
        }
         
#endif

        //Increment Game time
        Game.t += (int)Time.timeScale * (int)Game.timeState;

        if (Game.timeState == TimeState.Forward)
        {
            if (Game.t > Game.longestTime)
                Game.longestTime = Game.t;
        }
    }
}
