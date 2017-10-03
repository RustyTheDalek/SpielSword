using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMinions : MinionsManager {

    public float speed = 1.0f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<Rigidbody2D>().AddForce(transform.right * speed);
    }
}
