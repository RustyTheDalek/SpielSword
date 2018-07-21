using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controllers for ground characters
/// </summary>
public class GroundCharacter2D : PlatformerCharacter2D {

    #region public Variables

    public bool m_Grounded;            // Whether or not the character is grounded.

    #endregion

    #region Protected Variables

    /// <summary>
    /// Which direction the walking force should be applied in
    /// </summary>
    protected Vector2 moveVector = Vector2.right;

    /// <summary>
    /// Previous move
    /// </summary>
    protected Vector2 deltaMoveDir = Vector2.zero;
    #endregion

    #region Private Variables

    /// <summary>
    /// Can the character control themselves when there in the air
    /// </summary>
    [SerializeField] private bool m_AirControl = true;
    /// <summary>
    /// Maximum speed of the character xSpeed
    /// </summary>
    [SerializeField] private float m_MaxSpeed = 5f;

    [SerializeField] private float m_JumpForce = 15;  // Amount of force added when the character jumps.

    [SerializeField] private LayerMask m_WhatIsGround;  // A mask determining what is ground to the character

    private Transform m_Ground;    // A position marking where to check if the character is grounded.
    private const float k_GroundedRadius = .1f; // Radius of the overlap circle to determine if grounded
    private Transform m_Ceiling;   // A position marking where to check for ceilings

    public Transform m_Back, m_Front;  //Transforms for the Front and back of character used when calculating what direction move force should be applied

    private Collider2D[] colliders;

    private bool jump;
    private bool m_FacingRight = true;  // For determining which way the character is currently facing.

    private Vector2 directionForce;
    #endregion

    protected override void Awake()
    {
        base.Awake();

        // Setting up references.
        m_Ground = transform.Find("GroundCheck");
        m_Ceiling = transform.Find("CeilingCheck");
        m_Back = transform.Find("Back");
        if (m_Back == null)
        {
            Debug.Log("No back transform creating one, pls add to prefab");
            m_Back = new GameObject("Back").transform;
            m_Back.transform.SetParent(transform);
            m_Back.localPosition = new Vector3(-.5f, .5f, 0);
        }
        m_Front = transform.Find("Front");
        if (m_Front == null)
        {
            Debug.Log("No Front transform creating one, pls add to prefab");
            m_Front = new GameObject("Front").transform;
            m_Front.transform.SetParent(transform);
            m_Front.localPosition = new Vector3(.5f, .5f, 0);
        }
    }

    protected void FixedUpdate()
    {
        GroundCheck();
        CalcMoveVector();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, directionForce.normalized);
    }

    public virtual void Move(Vector2 moveDir, bool jump)
    {
        // If the player should jump...
        if (m_Grounded && jump)
        {
            m_Grounded = false;
            //Since the direction force for movement can result in positive Y Velocity 
            //we want to reset Y Velocity before a jump to prevent them going flying
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce), ForceMode2D.Impulse);
        }

        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {
            //moveVector = Vector2.right;
            directionForce = moveVector * moveDir.x * m_MoveForce * Time.deltaTime;

            m_Rigidbody2D.AddForce(directionForce, ForceMode2D.Impulse);

            //If the player stops moving or changes direction then we reduce xVelocity to zero
            if ((deltaMoveDir.x != moveDir.x && m_Grounded))
                m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);

            m_Rigidbody2D.velocity = new Vector2(Mathf.Clamp(
                    m_Rigidbody2D.velocity.x, -m_MaxSpeed, m_MaxSpeed),
                    m_Rigidbody2D.velocity.y);
        }

        DirectionLogic((int)moveDir.x);

        deltaMoveDir = moveDir;
    }

    /// <summary>
    /// Decide on move vector based on angles of colliders character is on
    /// </summary>
    private void CalcMoveVector()
    {
        if (m_Grounded)
        {
            //TODO:See why altering the parent objects scale affects this
            //Fire ray behind player and in front
            RaycastHit2D backHit = Physics2D.Raycast(m_Back.position,
                -transform.up, 1, m_WhatIsGround);

            RaycastHit2D frontHit = Physics2D.Raycast(m_Front.position,
                -transform.up, 1, m_WhatIsGround);

            if (backHit)
                Debug.DrawRay(m_Back.position, backHit.normal, Color.cyan);

            if(frontHit)
                Debug.DrawRay(m_Front.position, frontHit.normal, Color.cyan);

            //Finding the average between the two
            Vector2 avgNormal = (backHit.normal + frontHit.normal) / 2;

            Debug.DrawRay(m_Ceiling.position, avgNormal, Color.blue);

            //Then we find the perpendicular Vector of the two to give us a nice 
            //direction along platform we're running across
            moveVector = new Vector2(avgNormal.y, -avgNormal.x);

            Debug.DrawRay(transform.position, moveVector, Color.magenta);

        }
        else
        {
            //If in air we don't want odd move Vectors
            moveVector = transform.right;
        }
    }

    /// <summary>
    /// See if Character is touching the Ground
    /// </summary>
    private void GroundCheck()
    {
        m_Grounded = false;

        //The character is grounded if a circlecast to the groundcheck position hits anything designated as ground
        //This can be done using layers instead but Sample Assets will not overwrite your project settings.
        colliders = Physics2D.OverlapCircleAll(m_Ground.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
            }
        }
    }

    protected void DirectionLogic(int desiredDirection)
    {
        // If the input is moving the player right and the player is facing left...
        if (desiredDirection > 0 && !m_FacingRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (desiredDirection < 0 && m_FacingRight)
        {
            // ... flip the player.
            Flip();
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    //This works with Triggers in the world to prevent players from flying off of Ramps
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "RampUp")
        {
            //Only fire if moving in the right direction and well actually moving
            if (!jump && Vector2.Angle(m_Rigidbody2D.velocity, collision.transform.right) < 90)
            {
                if (m_Rigidbody2D.velocity.y > 0)
                {
                    Debug.Log("Slowing");
                    m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
                }
            }
        }

        if (collision.tag == "RampDown")
        {
            //Only fire if moving in the right direction and well actually moving
            if (!jump && Vector2.Angle(m_Rigidbody2D.velocity, collision.transform.right) < 90)
            {
                m_Rigidbody2D.AddForce(new Vector2(0, -100));
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //When player collides with Ground for first time then reduce xVelocity
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
        }
    }
}
