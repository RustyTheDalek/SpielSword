using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShamanTotem : SpawnableSpriteTimeObject
{
    bool comboUsed = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(creator != collision.gameObject)
        {
            if (!comboUsed)
            {
                Debug.Log("Used Totem");
                LevelManager.combosUsed++;
                comboUsed = true;
                LevelManager.IncreaseScore();
            }
        }
    }
}
