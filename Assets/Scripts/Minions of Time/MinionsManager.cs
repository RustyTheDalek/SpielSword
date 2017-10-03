using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionsManager : MonoBehaviour {

    byte health;
    private bool alive
    {
        get
        {
            return health > 0;
        }
    }
    // Use this for initialization
    void Start () {
        health = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (!alive)
        {
            Death();
        }
	}

    protected virtual void Death()
    {
        gameObject.SetActive(false);
    }
}
