using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMinions : MinionsManager {

    private float speed;
    private float distanceToFloor;
    private int layerGround;
    private Vector2 findFloor;
    private Ray2D findWallRay;
    private Ray2D ray;
    private bool inAir;

    // Use this for initialization
    new void Start () {
        base.Start();
        base.health = 1;
        speed = 0.60f;
        distanceToFloor = 0.8f;
        layerGround = LayerMask.NameToLayer("Ground");
        findFloor = new Vector2(transform.position.x + xDir, transform.position.y);
        //Do i want thier walking speed to be different? is that what this will do?
        xDir = Random.Range(0, 1);
        if (xDir == 0)
        {
            xDir = -1;
        }
    }
	
	// Update is called once per frame
	new void Update () {
        base.Update();
        Movement();
    }

    void Movement()
    {
        findFloor = new Vector2(transform.position.x + xDir * 3, transform.position.y);
        //ray cast below the player to ensure ground is still there
        if (inAir != !Physics2D.Raycast(findFloor, Vector2.down, distanceToFloor, layerGround))
        {
            if (!Physics2D.Raycast(findFloor, Vector2.down, distanceToFloor, layerGround))
            {

                Debug.Log("There is no ground");
                //if not reverse the direction
                if (xDir > 0)
                {
                    xDir = -1;
                }
                else
                {
                    xDir = 1;
                }
            }
        }
        inAir = !Physics2D.Raycast(findFloor, Vector2.down, distanceToFloor, layerGround);
        ray = new Ray2D(findFloor, Vector2.down);
       
        Debug.DrawRay(ray.origin, ray.direction);
        Debug.Log("There is ground");

        // Get something that moves teh character here plz
        //if so continue moving
        //GetComponent<Rigidbody2D>().AddForce(transform.right * speed);
        Vector3 move = new Vector3(transform.position.x + xDir, transform.position.y, transform.position.z);
        //move innitialy in a random path *may need to record this
        transform.position += move * speed * Time.deltaTime;
        //animData.move = xDir;

    }

}
