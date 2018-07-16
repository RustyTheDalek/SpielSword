using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to control the Ground based minions Boss
/// Created by : Sean Taylor      - ~06/17
/// Updated by : Sean Taylor      - 24/04/18
/// </summary>

[RequireComponent(typeof(GroundCharacter2D))]
public class GroundMinions : GroundCharacter {

    private float distanceToFloor;
    private float distanceFromWall;
    private Vector2 findFloor;
    private Vector2 findWall;
    private Vector2 findPlayer;
    private Ray2D ray;
    private bool inAir;
    private bool attacking;

    public Animator animi;
    public LayerMask layerGround;
    public LayerMask layerGroundOnly;
    public LayerMask layerVillagerOnly;
    public GameObject actPlayer;
    public bool lastAttacking;
    public bool playerHere;
    public int attackCount;

    public bool QueuedAttack
    {
        get
        {
            if (animi.GetBool("Attack1") ||
                animi.GetBool("Attack2") ||
                animi.GetBool("Attack3") ||
                animi.GetBool("Attack4"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public float timer = 0;
    
    protected override void Awake()
    {
        base.Awake();

        distanceFromWall = 0.4f;
        distanceToFloor = 0.8f;

        animData.Add("ManualFacedDirection", 0);
    }

    // Use this for initialization
    public void Start ()
    {
        attacking = false;
        playerHere = false;

        //Set a random start direction
        moveDir = new Vector2(Random.Range(0, 2), 0);
        if (moveDir.x == 0)
        {
            moveDir.x = -1;
        }
        findFloor = new Vector2(transform.position.x + moveDir.x, transform.position.y);
        findWall = new Vector2(transform.position.x + (moveDir.x * 0.6f), transform.position.y);

        //total number of attacks
        attackCount = 3;

    }
	
	// Update is called once per frame
	public override void Update ()
    {
        base.Update();
        if (Alive)
        {
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
    }

    void Movement()
    {
        #region Find the floor
        findFloor = new Vector2(transform.position.x + moveDir.x, transform.position.y);
        bool raycastResult = Physics2D.Raycast(findFloor, Vector2.down, distanceToFloor, layerGround);

        //ray cast below the player to ensure ground is still there
        if (inAir != !raycastResult)
        {
            if (!raycastResult)
            {
                //if not reverse the direction
                moveDir.x *= -1;
            }
        }
        inAir = !raycastResult;
        #endregion

        #region Find the wall
        findWall = new Vector2(transform.position.x + (moveDir.x * 0.6f), transform.position.y);
        Vector2 facedDirection;

        if (moveDir.x == 1)
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
            //if not reverse the direction
            moveDir.x *= -1;
        }
        #endregion

        if (!attacking && lastAttacking)
        {
            m_Ground.bManualFaceDirection = true;
            animData["ManualFacedDirection"] = (int)moveDir.x;
        }
        else if (!attacking && !lastAttacking)
        {
            //regardless continue moving
            m_Ground.bManualFaceDirection = false;
            animData["Move"] = moveDir;
        }
        lastAttacking = attacking;
    }

    void FindFoe()
    {
        findPlayer = new Vector2(actPlayer.transform.position.x - transform.position.x,actPlayer.transform.position.y - transform.position.y).normalized;
        bool rayResult = Physics2D.Raycast(transform.position, findPlayer, 3.5f, layerGroundOnly);
        if (rayResult){}
        else
        {
            int attack = Random.Range(0, attackCount - 1);
            Attack(attack);
        }
        
    }

    void Attack(int attack)
    {
        animData["Move"] = Vector2.zero;
        if (attacking || QueuedAttack)
        {
            // makes sure a attack isn't already playing befor continuing
            return;
        }
        else if (attack == 0)
        {
            animi.SetBool("Attack3", true);
        }
        else if (attack == 1)
        {
            // obsolete if not attacking behind self 
            // as facing direction will always end in a right attack
            if (moveDir.x == 1)
            {
                animi.SetBool("Attack2", true);
            }
            else if (moveDir.x == -1)
            {
                animi.SetBool("Attack2", true);
            }
        }
        else if (attack == 2)
        {
            animi.SetBool("Attack4", true);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == (LayerMask.NameToLayer("Weapon")))
        {
            OnHit();
        }
    }

    public void OnHit()
    {
        Debug.Log("Minion took Damage!");
        health -= 1;
    }

    public void OnAttacking()
    {
        attacking = true;
    }

    public void ExitAttacking()
    {
        attacking = false;
    }
}
