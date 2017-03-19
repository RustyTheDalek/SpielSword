using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageAura : MonoBehaviour {

    public float health = 4;

    //TODO: Add timer that make sure the Aura only lasts a certain amount of time
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DecreaseStrength()
    {
        if (health > 0)
        {
            health--;

            Color col = GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color = new Color(col.r, col.g, col.b, health / 4);
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.GetComponent<Villager>())
        {
            Villager temp = coll.GetComponent<Villager>();

            if (temp.currentVillager)
            {
                Debug.Log("Entered Buff aura");
                coll.GetComponent<Villager>().SetDamageMult( ( (int)health + 1) / 2);
            }
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.GetComponent<Villager>())
        {
            Villager temp = coll.GetComponent<Villager>();

            if (temp.currentVillager)
            {
                Debug.Log("Exited Buff aura");
                coll.GetComponent<Villager>().SetDamageMult(1);
            }
        }
    }


}
