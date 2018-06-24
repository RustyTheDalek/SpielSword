using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShamanTotem : SpawnableSpriteTimeObject
{
    public static bool comboUsed = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(creator != collision.gameObject)
        {
            if (!comboUsed)
            {
                comboUsed = true;
            }
        }
    }
}
