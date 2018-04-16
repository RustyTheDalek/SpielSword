using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlatformerCharacter2D))]
public class FlightMinions : Character {

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
    public GameObject orbitPoint;
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

        if (playerHere)
        {
            FindFoe();
        }

        //Vector3 relativePos = orbitPoint.transform.position - transform.position;
        //Quaternion rotation = Quaternion.LookRotation(relativePos);

        //Quaternion current = transform.localRotation;

        //transform.localRotation = Quaternion.Lerp(current, rotation, Time.deltaTime);
        //transform.Translate(0, 0, 3 * Time.deltaTime);
        //transform.rotation = Quaternion.identity;
        transform.RotateAround(orbitPoint.transform.position, Vector3.forward, 90 * Time.deltaTime);
        transform.rotation = Quaternion.identity;
    }

    void Movement()
    {
        //regardless continue moving
        //animData["Move"] = xDir;
    }

    void FindFoe()
    {
        findPlayer = new Vector2(actPlayer.transform.position.x - transform.position.x, actPlayer.transform.position.y - transform.position.y).normalized;
        Debug.Log("I'll find him");
        Debug.DrawRay(transform.position, findPlayer, Color.green);
        bool rayResult = Physics2D.Raycast(transform.position, findPlayer, 3.5f, layerGroundOnly);
        if (rayResult)
        {
            Debug.Log("i Dont see him");
        }
        else
        {
            Debug.Log("Get help he is here");
            Attack();
        }
    }

    void Attack()
    {
        Vector3 relativePos = actPlayer.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);

        Quaternion current = transform.localRotation;

        transform.localRotation = Quaternion.Lerp(current, rotation, Time.deltaTime);
        transform.Translate(Vector3.forward * Time.deltaTime);
    }

}

