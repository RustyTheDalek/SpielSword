using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FlyingCharacter2D))]
public class FlightMinions : Character {

    private float cooldownAttack;
    private int speed;
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
    /// Distance Minion orbits from origin
    /// </summary>
    public int orbitDistance = 5;

    /// <summary>
    /// The strength of the force that maintains the distance
    /// </summary>
    public float distanceAmp = 2;

    public GameObject actPlayer;
    public GameObject actCollision;
    public bool playerHere;
    /// <summary>
    /// Sets the minion to move left insted of orbit
    /// </summary>
    public bool dumbFire;
    public float timer = 0;
    /// <summary>
    /// Kill object once it has passed over the threshold for the level
    /// </summary>
    public float killZone;

    private FlyingCharacter2D m_Flying;

    protected override void Awake()
    {
        base.Awake();

        m_Flying = GetComponent<FlyingCharacter2D>();
        rb = GetComponent<Rigidbody2D>();

    }
    
    public void Start()
    {
        playerHere = false;
        onCooldown = false;
        moveDir = Vector2.zero;

        orginalPosition = transform.position;
        //Move the Minion by the distance we want him to Orbit
        transform.position = transform.position + Vector3.right * orbitDistance;

        speed = 20;

        playerColliders = actCollision.GetComponents<Collider2D>();
        attacking = false;
        animData.Add("MaxSpeed", 7f);

    }

    // Update is called once per frame
    public override void Update()
    {
        if (Alive)
        {
            base.Update();

            // if off the side of the level disable
            if (transform.position.x < killZone && dumbFire)
            {
                gameObject.SetActive(false);
            }

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
                    //calculate return vector
                    moveDir = (orginalPosition - transform.position);
                    moveDir = moveDir.normalized;

                    returning = true;
                    // resets any cool down
                    cooldownAttack = 0f;
                    onCooldown = false;
                    //if the enemy has made it back home then it no longer needs to return
                    if ( (transform.position - orginalPosition).magnitude <= orbitDistance)
                    {
                        returning = false;
                    }
                }
                //if there is no need to return back then continue to orbit a set point
                else
                {
                    if (!dumbFire)
                    {
                        //This code finds vector towards the it's origin then finds the 
                        //perpendicular vector with a clockwise bias so that it moves
                        //that direction
                        Vector2 toOriginal = transform.position - orginalPosition;
                        Vector2 forwardPoint = new Vector2(-toOriginal.y, toOriginal.x) / 
                            Mathf.Sqrt(toOriginal.x.Sqd() + toOriginal.y.Sqd());
                        //We use the direction to the Origin and the desired distance to 
                        //adjust the move direction to keep them the righht distance away
                        moveDir = forwardPoint.normalized + (toOriginal.normalized * 
                            (orbitDistance - toOriginal.magnitude) * distanceAmp); 
                    }
                    else
                    {
                        // if the entity was spawned just move left
                        //rb.velocity = new Vector2(-10, rb.velocity.y);
                        moveDir = Vector2.left;
                    }
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
            animData["MeleeAttack"] = true;
            animData["MaxSpeed"] = 15f;
            // Sets the old location to return too
            //TODO:Find out why this is
            //orginalPosition = transform.position;
        }
        HomeIn();        
    }

    /// <summary>
    /// Move to players first know position
    /// </summary>
    void HomeIn()
    {
        #region Rotate to align with target
        //relativePos = transform.position - playerPosition;
        //rotation = Quaternion.LookRotation(relativePos, Vector3.right);
        //rotation.x = 0;
        //rotation.y = 0;
        
        //current = transform.rotation;

        //transform.localRotation = Quaternion.Slerp(current, rotation , Time.deltaTime * 16);
        #endregion

        #region Move to location of players first know location

        moveDir = (playerPosition - transform.position);
        moveDir = moveDir.normalized;
        #endregion

        #region move by physics test code
        // moveForce = Vector3.MoveTowards(transform.position, playerPosition, speed * Time.deltaTime);
        // rb.MovePosition(moveForce);
        #endregion

        // enable the collisions for damage to player
        if (transform.position != playerPosition)
        {
            attacking = true;
        }
        // once location is reached stop the attack, disable the collisions
        else
        {
            StopAttacking();
        }
    }

    void StopAttacking()
    {
        animData["MeleeAttack"] = false;
        animData["MaxSpeed"] = 7f;
        attacking = false;
        onCooldown = true;
        returning = true;
        actPlayer = null;
        playerHere = false;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log("Minion hit something");

        if (coll.gameObject.layer == (LayerMask.NameToLayer("Weapon")))
        {
            OnHit();
        }

        if(coll.gameObject.layer == LayerMask.NameToLayer("Villager") && attacking && coll.gameObject == actPlayer)
        {
            Debug.Log("I hit the big boy!");
            StopAttacking();
        }
    }

    public void OnHit()
    {
        Debug.Log("Minion took Damage!");
        health -= 1;
    }
}

