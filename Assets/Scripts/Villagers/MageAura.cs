using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageAura : Aura
{
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (auraActive && coll.GetComponent<Villager>())
        {
            Villager temp = coll.GetComponent<Villager>();

            if (temp.CurrentVillager)
            {
                Debug.Log("Entered Buff aura");
                coll.GetComponent<Villager>().SetDamageMult(((int)health + 1) / 2);
            }
        }
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.GetComponent<Villager>())
        {
            Villager temp = coll.GetComponent<Villager>();

            if (temp.CurrentVillager && auraActive)
            {
                //Debug.Log("Entered Buff aura");
                coll.GetComponent<Villager>().SetDamageMult(((int)health + 1) / 2);
            }
            else
            {
                Debug.Log("No Buff aura");
                coll.GetComponent<Villager>().SetDamageMult(1);
            }
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.GetComponent<Villager>())
        {
            Villager temp = coll.GetComponent<Villager>();

            if (temp.CurrentVillager)
            {
                Debug.Log("Exited Buff aura");
                coll.GetComponent<Villager>().SetDamageMult(1);
            }
        }
    }
}
