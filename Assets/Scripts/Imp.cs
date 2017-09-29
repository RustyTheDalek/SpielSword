using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that allows imps to fly towards the boss
/// Currently just flies towards a given position but this can be easily changed
/// </summary>
public class Imp : MonoBehaviour {

    public Vector3 targetPos = Vector3.zero;

    public float impSpeed = 10;

    Rigidbody2D rbody;

	// Use this for initialization
	void Start ()
    {
        rbody = GetComponent<Rigidbody2D>();	
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Find direction to target
        Vector3 toTarget = targetPos - transform.position;

        //Apply relative force
        rbody.AddForce(toTarget.normalized * Time.deltaTime * impSpeed);

#if UNITY_EDITOR

        Debug.DrawRay(transform.position, toTarget.normalized, Color.yellow);

#endif
	}
}
