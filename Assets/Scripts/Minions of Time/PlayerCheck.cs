using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheck : MonoBehaviour {

    public GameObject parentMinion;

    //public Component parentComponent;
    bool groundEnemy;
    bool flyingEnemy;
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
            //Debug.Log("Ground Enemy");
            groundEnemy = true;
            flyingEnemy = false;
            //parentComponent = parentMinion.GetComponent<FlightMinions>();
        }
        else if (parentMinion.GetComponent<FlightMinions>() != null)
        {
            groundEnemy = false;
            flyingEnemy = true;
            //parentComponent = parentMinion.GetComponent<GroundMinions>();
        }
        else
        {
            groundEnemy = false;
            flyingEnemy = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D player)
    {
        if (player.GetComponent<Villager>())
        {
            onExit = false;
            if (!onEnter)
            {
                //Debug.Log("I sense a player");
                if (groundEnemy)
                {
                    parentMinion.GetComponent<GroundMinions>().actPlayer = player.gameObject;
                    parentMinion.GetComponent<GroundMinions>().playerHere = true;
                }
                else if (flyingEnemy)
                {
                    parentMinion.GetComponent<FlightMinions>().actPlayer = player.gameObject;
                    parentMinion.GetComponent<FlightMinions>().playerHere = true;
                }
                else
                {
                    parentMinion.GetComponent<PlayerGrapple>().actPlayer = player.gameObject;
                    parentMinion.GetComponent<PlayerGrapple>().playerHere = true;
                }
            }
            onEnter = true;
        }
    }

    void OnTriggerStay2D(Collider2D player)
    {
        if (player.GetComponent<Villager>())
        {
            if (player.attachedRigidbody.position.x >=
                parentMinion.transform.position.x)
            {
                if (groundEnemy)
                {
                    parentMinion.GetComponent<GroundMinions>().xDir = 1;
                }
                else if (flyingEnemy)
                {
                    parentMinion.GetComponent<FlightMinions>().xDir = 1;
                }
            }
            else if (player.attachedRigidbody.position.x <=
                parentMinion.transform.position.x)
            {
                if (groundEnemy)
                {
                    parentMinion.GetComponent<GroundMinions>().xDir = -1;
                }
                else if (flyingEnemy)
                {
                    parentMinion.GetComponent<FlightMinions>().xDir = -1;
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D player)
    {
        if (player.GetComponent<Villager>())
        {
            onEnter = false;
            if (!onExit)
            {
                //Debug.Log("Dont you want to play?");
                if (groundEnemy)
                {
                    parentMinion.GetComponent<GroundMinions>().playerHere = false;
                }
                else if (flyingEnemy)
                {
                    parentMinion.GetComponent<FlightMinions>().playerHere = false;
                }
                else
                {
                    parentMinion.GetComponent<PlayerGrapple>().playerHere = false;
                }
            }
            onExit = true;
        }
    }
}
