using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionsManager : MonoBehaviour {

    public int health;
    public float xDir;
    public bool alive
    {
        get
        {
            return health > 0;
        }
    }
    // Use this for initialization
    public void Start () {
        health = 0;
    }
	
	// Update is called once per frame
	public void Update () {
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
