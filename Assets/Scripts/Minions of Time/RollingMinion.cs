using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingMinion : GroundMinion
{
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        switch (LayerMask.LayerToName(collision.gameObject.layer))
        {
            case "Villager":
            case "PastVillager":

                Character villager = collision.gameObject.GetComponentInParent<Character>();

                if (collision.gameObject.transform.root.gameObject == closestVillager.gameObject &&
                    collision.otherCollider.gameObject.layer != LayerMask.NameToLayer("Minion") &&
                    !villager.shielded)
                {
                    Debug.Log(collision.otherCollider.gameObject.name);
                    this.NamedLog("Hit my target Villager");
                    OnDeath(Vector2.zero);
                    villager.OnHit(transform.PointTo(collision.otherCollider.transform));
                }
                break;
        }
    }

}
