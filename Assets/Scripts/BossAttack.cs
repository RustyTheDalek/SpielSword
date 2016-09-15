using UnityEngine;
using System.Collections;

/// <summary>
/// Script for registering Boss attacks
/// </summary>
public class BossAttack : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D coll)
    {
        //Debug.Log("Collided with " + coll.gameObject.name);

        //BossAttack can only damage Villagers, has to be enabled (In an attack 
        //animation and God mode off for obvious reasons
        if (coll.gameObject.layer == (LayerMask.NameToLayer("Villager")) && this.enabled
            && !Game.GodMode)
        {
            coll.gameObject.GetComponent<Villager>().OnHit();
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (this.name.Contains("Range"))
        {
            if (coll.gameObject.GetComponent<Head>())
            {
                coll.gameObject.GetComponent<Head>().OnHit();
            }

            Destroy(this.gameObject);
        }
    }
}
