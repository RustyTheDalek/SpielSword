﻿using UnityEngine;

/// <summary>
/// Tracks when Player Leaves Village
/// Created by Ian Jones : 09/04/18
/// Updated by Ian Joens : 10/04/18
/// </summary>
public class VillageExit : MonoBehaviour {

    public delegate void PlayerLeftVillageEvent();
    public event PlayerLeftVillageEvent OnPlayerLeftVillage;

    bool playerEntered = false;

    public void Setup(VillageAndMapManager wMapManager)
    {
        wMapManager.OnPlayerEnterVillage += Enable;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Villager" &&
            !playerEntered)
        {
            Debug.Log("Player Left Village");

            if(OnPlayerLeftVillage != null)
                OnPlayerLeftVillage();

            //OneShot event
            playerEntered = true;
        }
        else
        {
            Debug.Log("Something entered me but wasn't a villager");
        }
    }

    void Enable()
    {
        playerEntered = false;
    }

    public void Unsubscribe(VillageAndMapManager wMapManager)
    {
        wMapManager.OnPlayerEnterVillage -= Enable;
    }
}
