using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheck : MonoBehaviour {

    public GameObject parentMinion;
    bool onEnter;
    bool onExit;

	// Use this for initialization
	void Start () {
        onEnter = false;
        onExit = true;
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
            parentMinion.GetComponent<GroundMinions>().actPlayer = player.gameObject;
            parentMinion.GetComponent<GroundMinions>().playerHere = true;
        }
        onEnter = true;
    }

    void OnTriggerExit2D(Collider2D player)
    {
        onEnter = false;
        if (!onExit)
        {
            Debug.Log("Dont you want to play?");
            parentMinion.GetComponent<GroundMinions>().playerHere = false;
        }
        onExit = true;
    }
}
