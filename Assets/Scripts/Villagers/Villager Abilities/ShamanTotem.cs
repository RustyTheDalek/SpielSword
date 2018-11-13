using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShamanTotem : SpawnableSpriteTimeObject
{
    //TODO: Have list of villagers interacted with this to check for multiple combos if desired
    public bool comboUsed = false;

    public event TimeObjectEvent OnUsedTotem;

    protected override void Start()
    {
        base.Start();

        OnStartPlayback += EnableCollider;
        OnFinishPlayback+= DisableCollider;
    }

    public void Setup(VillagerManager villagerManager)
    {
        OnUsedTotem += villagerManager.IncCombosUsed;
    }

    public void Unsubscribe(VillagerManager villagerManager)
    {
        OnUsedTotem -= villagerManager.IncCombosUsed;
    }

    protected void EnableCollider()
    {
        GetComponent<Collider2D>().enabled = true;
    }

    protected void DisableCollider()
    {
        GetComponent<Collider2D>().enabled = false;
    }

    private void OnDestroy()
    {
        OnStartPlayback -= EnableCollider;
        OnFinishPlayback-= DisableCollider;
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
