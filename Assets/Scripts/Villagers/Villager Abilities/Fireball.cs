using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : VillagerAttack
{
    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (attackType == AttackType.Ranged)
        {
            switch (LayerMask.LayerToName(collision.gameObject.layer))
            {
                case "Minion":
                    Debug.Log("Burning Minion");
                    collision.GetComponentInParent<Character>().OnHit(transform.PointTo(collision.transform));
                    break;

                case "Boss":

                    Debug.Log("Burning Minion");
                    collision.GetComponent<Head>().OnHit(damageMult);
                    break;
            }
        }
    }
}
