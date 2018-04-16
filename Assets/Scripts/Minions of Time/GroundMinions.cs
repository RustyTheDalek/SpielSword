using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to control the Ground based minions Boss
/// Created by : Sean Taylor      - ~06/17
/// Updated by : Sean Taylor      - 10/04/18
/// </summary>

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
    public bool attacking;
    public int attackCount;

    public float timer = 0;

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

        timer += Time.deltaTime;

        if (timer > .046875f)
        {
            Movement();
            timer = 0f;
        }

        // Get a player check from PlayerCheck to see if player is present
        if (playerHere)
        {
            FindFoe();
        }
    }

    void Movement()
    {
        #region Find the floor
        findFloor = new Vector2(transform.position.x + xDir, transform.position.y);
        bool raycastResult = Physics2D.Raycast(findFloor, Vector2.down, distanceToFloor, layerGround);

        //ray cast below the player to ensure ground is still there
        if (inAir != !raycastResult)
        {
            if (!raycastResult)
            {

                //Debug.Log("There is no ground");
                //if not reverse the direction
                xDir *= -1;
            }
        }
        inAir = !raycastResult;
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
        animData["Move"] = xDir;
    }

    void FindFoe()
    {
        findPlayer = new Vector2(actPlayer.transform.position.x - transform.position.x,actPlayer.transform.position.y - transform.position.y).normalized;
        //Debug.Log("I'll find him");
        Debug.DrawRay(transform.position, findPlayer, Color.green);
        bool rayResult = Physics2D.Raycast(transform.position, findPlayer, 3.5f, layerGroundOnly);
        if (rayResult)
        {
            Debug.Log("i Dont see him");
        }
        else
        {
            Debug.Log("Get help he is here");
            int attack = Random.Range(0, attackCount - 1);
            Attack(attack);
        }
        
    }

    void Attack(int attack)
    {
        animData["Move"] = 0;
        if (attack == 0)
        {
            //SetBool("Attack1", true);
            attacking = true;
        }
        if (attack == 1)
        {
            //SetBool("Attack2", true);
            attacking = true;
        }
        if (attack == 2)
        {
            //SetBool("Attack3", true);
            attacking = true;
        }
        if (attack == 3)
        {
            //SetBool("Attack4", true);
            attacking = true;
        }
        animData["Move"] = xDir;
    }

}
