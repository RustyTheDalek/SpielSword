using UnityEngine;
using System.Collections;

/// <summary>
/// Script for registering Boss attacks
/// </summary>
public class BossAttack : MonoBehaviour
{
    public bool attacking = false;

    void OnCollisionEnter2D(Collision2D coll)
    {
        //Debug.Log("Collided with " + coll.gameObject.name);

        //BossAttack can only damage Villagers, has to be enabled (In an attack 
        //animation and God mode off for obvious reasons
        if (coll.gameObject.layer == (LayerMask.NameToLayer("Villager")) && this.enabled
            && !Game.GodMode && !Game.StageMetEarly)
        {
            coll.gameObject.GetComponent<Villager>().OnHit();
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        //TODO: Potentially redudant code, have to come back and check this
        //if (this.name.Contains("Range"))
        //{
        //    if (coll.gameObject.GetComponent<Head>())
        //    {
        //        Debug.Log("hit");
        //        coll.gameObject.GetComponent<Head>().OnHit(1);
        //    }

        //    Destroy(this.gameObject);
        //}
    }
}
