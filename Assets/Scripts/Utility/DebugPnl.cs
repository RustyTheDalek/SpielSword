using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script for aiding with Debugging, contains references to parts it needs to won't 
/// be present in final build
/// Created by      : Ian - 15/03/18
/// </summary>
public class DebugPnl : MonoBehaviour {

    VillagerManager vilManager;
    BossManager bManager;

    bool active = false;

    private void Start()
    {

        vilManager = GameObject.Find("VillagerManager").GetComponent<VillagerManager>();
        bManager = GameObject.Find("Boss").GetComponent<BossManager>();
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

    public void BossStage2()
    {
        bManager.Stage2();
    }

    public void BossStage3()
    {
        bManager.Stage3();
    }

    public void BossStage4()
    {
        bManager.Stage4();
    }

    public void BossStage5()
    {
        bManager.Stage5();
    }

}
