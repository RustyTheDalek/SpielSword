using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : VillagerAttack
{
    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (attackType == AttackType.Ranged)
        { 
            switch(LayerMask.LayerToName(collision.gameObject.layer))
            {
                case "Boss":

                    collision.GetComponentInParent<LivingObject>().OnHit(
                        transform.PointTo(collision.transform), damage * damageMult);
                    break;
            }

        }
    }
}
