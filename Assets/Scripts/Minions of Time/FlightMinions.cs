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
    private Ray2D ray;
    private bool inAir;
    private bool attacking;
    private bool lastAttacking;
    private bool returning;
    private bool onCooldown;
    private Quaternion rotation;
    private Quaternion current;
    private Collider2D[] playerColliders;


    public LayerMask layerGround;
    public LayerMask layerGroundOnly;
    public LayerMask layerVillagerOnly;
    public int orbitPointX;
    public int orbitPointY;
    public GameObject actPlayer;
    public GameObject actCollision;
    public bool playerHere;

    public float timer = 0;

    public override void Awake()
    {
        base.Awake();

        m_Platformer = GetComponent<PlatformerCharacter2D>();

        distanceFromWall = 0.4f;
        distanceToFloor = 0.8f;
    }

    // Use this for initialization
    public void Start()
    {

        //actPlayer = GameObject.FindGameObjectWithTag("Player");
        playerHere = false;
        onCooldown = false;

        //Set a random start direction
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

            if (onCooldown)
            {
                cooldownAttack += Time.deltaTime;
            }

            if (cooldownAttack >= 2.5f)
            {
                onCooldown = false;
            }

            if (timer > .046875f)
            {
                Movement();
                timer = 0f;
            }

            if (playerHere && !returning && !onCooldown)
            {
                FindFoe();
            }
            else if (attacking)
            {
                HomeIn();
            }

            if (!attacking && !onCooldown)
            {
                if (lastAttacking && transform.position != orginalPosition || returning)
                {
                    transform.position = Vector3.MoveTowards(transform.position, orginalPosition, speed * Time.deltaTime);
                    returning = true;
                    cooldownAttack = 0f;
                    onCooldown = false;
                    if (transform.position == orginalPosition)
                    {
                        returning = false;
                    }
                }
                else
                {
                    transform.RotateAround(orbitPoint, Vector3.forward, 90 * Time.deltaTime);
                    transform.rotation = Quaternion.identity;

                }
            }
            lastAttacking = attacking;
        }
        
    }

    void Movement()
    {
        //regardless continue moving
        //animData["Move"] = xDir;
    }

    void FindFoe()
    {
        findPlayer = new Vector2(actPlayer.transform.position.x - 
            transform.position.x, actPlayer.transform.position.y - transform.position.y).normalized;
        bool rayResult = Physics2D.Raycast(transform.position, findPlayer, 3.5f, layerGroundOnly);
        if (!rayResult)
        {
            Attack();
        }
    }

    void Attack()
    {
        if (!attacking)
        {
            // sets the players location
            playerPosition = actPlayer.transform.position;
            //current = new Quaternion (0, 0, transform.localRotation.z, 0);

            //if (m_Platformer.m_FacingRight)
            //{
            //    rotation.z -= 90;
            //}
            //else
            //{
            //    rotation.z += 90;
            //}

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
        //transform.LookAt(playerPosition, Vector3.right);
        #endregion

        // Move Enemy to Player
        transform.position = Vector3.MoveTowards(transform.position, playerPosition, speed * Time.deltaTime);

        // once location is reached stop the attack
        if (transform.position != playerPosition)
        {
            attacking = true;
            foreach (Collider2D collider in playerColliders)
            {
                collider.enabled = true;
            }
            //ColliderTransform.GetChild(0).GetComponent<Collider>();

        }
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

