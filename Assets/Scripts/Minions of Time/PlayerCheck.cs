using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheck : MonoBehaviour {

    public GameObject parentMinion;

    //public Component parentComponent;
    bool groundEnemy;
    bool onEnter;
    bool onExit;

	// Use this for initialization
	void Start ()
    {
        onEnter = false;
        onExit = true;

        parentMinion = GetComponentInParent<Character>().gameObject;

        if (parentMinion.GetComponent<GroundMinions>() != null)
        {
            Debug.Log("Ground Enemy");
            groundEnemy = true;
            //parentComponent = parentMinion.GetComponent<FlightMinions>();
        }
        else
        {
            groundEnemy = false;
            //parentComponent = parentMinion.GetComponent<GroundMinions>();
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D player)
    {
        onExit = false;
        if (!onEnter)
        {
            Debug.Log("I sense a player");
            if (groundEnemy)
            {
                parentMinion.GetComponent<GroundMinions>().actPlayer = player.gameObject;
                parentMinion.GetComponent<GroundMinions>().playerHere = true;
            }
            else
            {
                parentMinion.GetComponent<FlightMinions>().actPlayer = player.gameObject;
                parentMinion.GetComponent<FlightMinions>().playerHere = true;
            }
        }
        onEnter = true;
    }

    void OnTriggerExit2D(Collider2D player)
    {
        onEnter = false;
        if (!onExit)
        {
            Debug.Log("Dont you want to play?");
            if (groundEnemy)
            {
                parentMinion.GetComponent<GroundMinions>().playerHere = false;
            }
            else
            {
                parentMinion.GetComponent<FlightMinions>().playerHere = false;
            }
        }
        onExit = true;
    }
}
