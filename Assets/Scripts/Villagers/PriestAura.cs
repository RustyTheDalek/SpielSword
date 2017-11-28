using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestAura : Aura
{
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (auraActive && coll.GetComponent<Villager>())
        {
            Villager temp = coll.GetComponent<Villager>();

            if (temp.CurrentVillager)
            {
                Debug.Log("Entered Protection aura");
                coll.GetComponent<Villager>().shielded = true;
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
                Debug.Log("Entered Protection aura");
                coll.GetComponent<Villager>().shielded = true;
            }
            else
            {
                Debug.Log("No Protection aura");
                coll.GetComponent<Villager>().shielded = false;

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
                Debug.Log("Exited Protection aura");
                coll.GetComponent<Villager>().shielded = false;
            }
        }
    }
}
