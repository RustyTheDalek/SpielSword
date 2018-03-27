using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShamanTotem : SpawnableSpriteTimeObject
{
    public GameObject creator;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(creator != collision.gameObject)
        {
            if (!Game.ComboAchieved)
            {
                Debug.Log("Used Totem");
                Game.ComboAchieved = true;
                Game.IncScore();
            }
        }
    }
}
