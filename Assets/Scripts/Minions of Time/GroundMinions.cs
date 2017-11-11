using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlatformerCharacter2D))]
public class GroundMinions : Character {

    private float distanceToFloor;
    private float distanceFromWall;
    private Vector2 findFloor;
    private Vector2 findWall;
    private Vector2 findPlayer;
    private Ray2D ray;
    private bool inAir;

    public LayerMask layerGround;
    public LayerMask layerGroundOnly;
    public LayerMask layerVillagerOnly;
    public GameObject actPlayer;
    public bool playerHere;

    public override void Awake()
    {
        base.Awake();

        m_Platformer = GetComponent<PlatformerCharacter2D>();

        distanceFromWall = 0.4f;
        distanceToFloor = 0.8f;
    }

    // Use this for initialization
    public void Start ()
    {

        //actPlayer = GameObject.FindGameObjectWithTag("Player");
        playerHere = false;

        //Set a random start direction
        xDir = Random.Range(0, 2);
        if (xDir == 0)
        {
            xDir = -1;
        }
        findFloor = new Vector2(transform.position.x + xDir, transform.position.y);
        findWall = new Vector2(transform.position.x + (xDir*0.6f), transform.position.y);
    }
	
	// Update is called once per frame
	public override void Update ()
    {
        base.Update();

        Movement();

        if (playerHere)
        {
            FindFoe();
        }
    }

    void Movement()
    {
        #region Find the floor
        findFloor = new Vector2(transform.position.x + xDir, transform.position.y);
        //ray cast below the player to ensure ground is still there
        if (inAir != !Physics2D.Raycast(findFloor, Vector2.down, distanceToFloor, layerGround))
        {
            if (!Physics2D.Raycast(findFloor, Vector2.down, distanceToFloor, layerGround))
            {

                //Debug.Log("There is no ground");
                //if not reverse the direction
                xDir *= -1;
            }
        }
        inAir = !Physics2D.Raycast(findFloor, Vector2.down, distanceToFloor, layerGround);
        #endregion

        #region Find the wall
        findWall = new Vector2(transform.position.x + (xDir * 0.6f), transform.position.y);
        Vector2 facedDirection;

        if (xDir == 1)
        {
            facedDirection = Vector2.right;
        }
        else
        {
            facedDirection = Vector2.left;
        }

        //ray cast infront the player to ensure there is no wall
        if (Physics2D.Raycast(findWall, facedDirection, distanceFromWall, layerGround))
        {

            //Debug.Log("There is a Wall");
            //if not reverse the direction
            xDir *= -1;
        }
        #endregion

        //regardless continue moving
        animData.move = xDir;
    }

    void FindFoe()
    {
        findPlayer = new Vector2(actPlayer.transform.position.x - transform.position.x,actPlayer.transform.position.y - transform.position.y).normalized;
        Debug.Log("I'll find him");
        Debug.DrawRay(transform.position, findPlayer, Color.green);
        if (Physics2D.Raycast(transform.position, findPlayer, 3.5f, layerVillagerOnly))
        {
            Debug.Log("i Dont see him");
            if (!Physics2D.Raycast(transform.position, findPlayer, 3.5f, layerGroundOnly))
            {
                Debug.Log("Get help he is here");
            }
        }
    }

}
