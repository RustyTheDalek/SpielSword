using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlatformerCharacter2D))]
public class FlightMinions : Character {

    private float distanceToFloor;
    private float distanceFromWall;
    private float cooldownAttack;
    private int speed;
    private Vector2 findFloor;
    private Vector2 findWall;
    private Vector2 findPlayer;
    private Vector3 orbitPoint;
    private Vector3 orginalPosition;
    private Vector3 playerPosition;
    private Vector3 relativePos;
    private Vector3 moveForce;
    private Ray2D ray;
    private bool inAir;
    private bool attacking;
    private bool lastAttacking;
    /// <summary>
    /// Is returning following a succesful 
    /// attack to it's original position
    /// </summary>
    private bool returning;
    /// <summary>
    /// Attack is currently unusable for the duration
    /// </summary>
    private bool onCooldown;
    private Quaternion rotation;
    private Quaternion current;
    private Collider2D[] playerColliders;
    private Rigidbody2D rb;

    public LayerMask layerGround;
    public LayerMask layerGroundOnly;
    public LayerMask layerVillagerOnly;
    /// <summary>
    /// Sets along the X axis away from the enemy 
    /// as to where the point of orbit should be
    /// </summary>
    public int orbitPointX;
    /// <summary>
    /// Sets along the Y axis away from the enemy 
    /// as to where the point of orbit should be
    /// </summary>
    public int orbitPointY;
    public GameObject actPlayer;
    public GameObject actCollision;
    public bool playerHere;

    public float timer = 0;

    public override void Awake()
    {
        base.Awake();

        m_Platformer = GetComponent<PlatformerCharacter2D>();
        rb = GetComponent<Rigidbody2D>();

        distanceFromWall = 0.4f;
        distanceToFloor = 0.8f;
    }
    
    public void Start()
    {
        playerHere = false;
        onCooldown = false;

        //Set a random start direction, obsolite atm
        xDir = Random.Range(0, 2);
        if (xDir == 0)
        {
            xDir = -1;
        }
        findFloor = new Vector2(transform.position.x + xDir, transform.position.y);
        findWall = new Vector2(transform.position.x + (xDir * 0.6f), transform.position.y);
        orbitPoint = new Vector3(transform.position.x + orbitPointX,
            transform.position.y + orbitPointY, 0);
        orginalPosition = transform.position;

        speed = 8;

        playerColliders = actCollision.GetComponents<Collider2D>();
        attacking = false;

    }

    // Update is called once per frame
    public override void Update()
    {
        if (Alive)
        {
            base.Update();

            timer += Time.deltaTime;

            // checks to see if the attack is on a cooldown
            if (onCooldown)
            {
                cooldownAttack += Time.deltaTime;
            }

            // checks to see if enoght time has passed for the cooldown
            if (cooldownAttack >= 2.5f)
            {
                onCooldown = false;
            }

            //possibly obsolite
            if (timer > .046875f)
            {
                Movement();
                timer = 0f;
            }

            // if there is a player and not doing anything else
            if (playerHere && !returning && !onCooldown)
            {
                // find the player a proceed to attack them
                FindFoe();
            }
            else if (attacking)
            {
                // continue to move to the players location
                HomeIn();
            }

            // makes sure the enemy isn't doing something
            if (!attacking && !onCooldown)
            {
                // checks that the enemy has finished attacking and is not where they began
                // or that they are already returning
                if (lastAttacking && transform.position != orginalPosition || returning)
                {
                    //move them back to where they started
                    transform.position = Vector3.MoveTowards(transform.position, orginalPosition, speed * Time.deltaTime);
                    returning = true;
                    // resets any cool down
                    cooldownAttack = 0f;
                    onCooldown = false;
                    //if the enemy has made it back home then it no longer needs to return
                    if (transform.position == orginalPosition)
                    {
                        returning = false;
                    }
                }
                //if there is no need to return back then continue to orbit a set point
                else
                {
                    transform.RotateAround(orbitPoint, Vector3.forward, 90 * Time.deltaTime);
                    transform.rotation = Quaternion.identity;

                }
            }
            // is used to check that the last frame was an attack
            lastAttacking = attacking;
        }
        
    }

    void Movement()
    {
        //regardless continue moving
        //animData["Move"] = xDir;
    }

    /// <summary>
    /// Ensures it can see the player
    /// </summary>
    void FindFoe()
    {
        // Takes players location
        findPlayer = new Vector2(actPlayer.transform.position.x - 
            transform.position.x, actPlayer.transform.position.y - transform.position.y).normalized;
        // See if it can draw to the player
        bool rayResult = Physics2D.Raycast(transform.position, findPlayer, 3.5f, layerGroundOnly);
        if (!rayResult)
        {
            Attack();
        }
    }

    /// <summary>
    /// Set the inital position of the player 
    /// and the location it is when it begins the attack
    /// </summary>
    void Attack()
    {
        if (!attacking)
        {
            // sets the players location
            playerPosition = actPlayer.transform.position;
            
            // Sets the old location to return too
            orginalPosition = transform.position;
        }
        HomeIn();        
    }

    /// <summary>
    /// Move to players first know position
    /// </summary>
    void HomeIn()
    {
        #region Rotate to align with target
        relativePos = transform.position - playerPosition;
        rotation = Quaternion.LookRotation(relativePos, Vector3.right);
        rotation.x = 0;
        rotation.y = 0;
        
        current = transform.rotation;

        transform.localRotation = Quaternion.Slerp(current, rotation , Time.deltaTime * 16);
        #endregion

        #region Move to location of players first know location
        moveForce = Vector3.MoveTowards(transform.position, playerPosition, speed * Time.deltaTime);
        //Vector3 direction = rb.transform.position - moveForce;
        //rb.AddForceAtPosition(direction.normalized, transform.position);
        //rb.MovePosition(moveForce);
        //Vector3 dir = (playerPosition - transform.position).normalized * speed;
        //rb.velocity = dir;
        //rb.AddRelativeForce(Vector3.forward * speed);
        rb.AddRelativeForce(moveForce, ForceMode2D.Force);
        #endregion

        // enable the collisions for damage to player
        if (transform.position != playerPosition)
        {
            attacking = true;
            foreach (Collider2D collider in playerColliders)
            {
                collider.enabled = true;
            }
        }
        // once location is reached stop the attack, disable the collisions
        else
        {
            attacking = false;
            onCooldown = true;
            returning = true;
            foreach (Collider2D collider in playerColliders)
            {
                collider.enabled = false;
            }
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

}

