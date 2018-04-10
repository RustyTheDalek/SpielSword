using System.Collections;
using UnityEngine;

/// <summary>
/// Script for aiding with Debugging, contains references to parts it needs to won't 
/// be present in final build
/// Created by : Ian Jones - 15/03/18
/// Updated by : Ian Jones - 10/04/18
/// </summary>
public class DebugPnl : MonoBehaviour {

#if UNITY_EDITOR
    VillagerManager vilManager;
    BossManager bManager;

    bool active = false;

    private void Start()
    {
        try
        {
            vilManager = GameObject.Find("VillagerManager").GetComponent<VillagerManager>();
        }
        catch
        {
            Debug.Log("No Villager manager, cannot debug");
        }

        try
        {
            bManager = GameObject.Find("Boss").GetComponent<BossManager>();
        }
        catch
        {
            Debug.Log("No Boss, not able to debug it");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            active = !active;

            if (active)
            {
                transform.localScale = Vector3.one;
            }
            else
            { 
                transform.localScale = Vector3.zero;
            }
        }
    }

    public void ToggleGodMode()
    {
        Game.GodMode = !Game.GodMode;
    }

    public void KillVillager()
    {
        vilManager.KillVillager();
    }

    public void ToStage(int stage)
    {
        stage--;
        bManager.ToStage(stage);
    }
#endif
}
