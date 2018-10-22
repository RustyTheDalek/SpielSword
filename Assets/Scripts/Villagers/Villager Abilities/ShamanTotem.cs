﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShamanTotem : SpawnableSpriteTimeObject
{
    //TODO: Have list of villagers interacted with this to check for multiple combos if desired
    public bool comboUsed = false;

    public event TimeObjectEvent OnUsedTotem;

    public void Setup(VillagerManager villagerManager)
    {
        OnUsedTotem += villagerManager.IncCombosUsed;
    }

    public void Unsubscribe(VillagerManager villagerManager)
    {
        OnUsedTotem -= villagerManager.IncCombosUsed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(creator.gameObject != collision.gameObject)
        {
            if (!comboUsed)
            {
                if(OnUsedTotem != null)
                    OnUsedTotem();
            }
        }
    }
}
