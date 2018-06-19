using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlatformerCharacter2D))]
public class FlightMinions : Character {

    private float distanceToFloor;
    private float distanceFromWall;
    private int speed;
    private Vector2 findFloor;
    private Vector2 findWall;
    private Vector2 findPlayer;
    private Vector3 orbitPoint;
    private Vector3 orginalPosition;
    private Vector3 playerPosition;
    private Ray2D ray;
    private bool inAir;
    private bool attacking;
    private bool lastAttacking;
    private bool returning;
    private Quaternion rotation;
    private Quaternion current;


    public LayerMask layerGround;
    public LayerMask layerGroundOnly;
    public LayerMask layerVillagerOnly;
    public int orbitPointX;
    public int orbitPointY;
    public GameObject actPlayer;
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

        attacking = false;

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        timer += Time.deltaTime;

        if (timer > .046875f)
        {
            Movement();
            timer = 0f;
        }

        if (playerHere && !returning)
        {
            FindFoe();
        }
        else
        {
            attacking = false;
        }

        if (!attacking)
        {
            if(lastAttacking && transform.position != orginalPosition || returning)
            {
                transform.position = Vector3.MoveTowards(transform.position, orginalPosition, speed * Time.deltaTime);
                returning = true;
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
            rotation = Quaternion.LookRotation(new Vector3(playerPosition.x, playerPosition.y, 0));

            //Vector3 relativePos = actPlayer.transform.position - transform.position;
            //rotation = Quaternion.LookRotation(new Vector3(relativePos.x, relativePos.y, 0));


            current = new Quaternion (transform.localRotation.x, transform.localRotation.y, 0, 0);

            orginalPosition = transform.position;
        }

        // move to players first know position
        transform.localRotation = Quaternion.Lerp(current, rotation, Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, playerPosition, speed * Time.deltaTime);
        //transform.Translate(Vector3.forward * speed * Time.deltaTime);


        //  Not accurate plz fix
        if (transform.position != actPlayer.transform.position)
            // ^^ this
        {
            attacking = true;
        }
        else
        {
            attacking = false;
        }
    }

}

