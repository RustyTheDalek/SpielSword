using UnityEngine;
using System.Collections;

/// <summary>
/// Controls the Animation and movement of the 2D Characters
/// Created : GGJ16
/// Updated by : Ian Jones  - 06/04/18
/// </summary>
public class PlatformerCharacter2D : MonoBehaviour
{
    [SerializeField] private bool m_AirControl = true;                 // Whether or not a player can steer while jumping;
    [SerializeField] private float m_MaxSpeed = 5f;                    // The fastest the character can travel in the x axis.
    [SerializeField] private float m_MoveForce = 10f;                  // Strength of force that moves Character
    [SerializeField] private float m_JumpForce = 400f;                 // Amount of force added when the character jumps.
    //[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

    private Transform m_GroundCheck;    // A position marking where to check if the character is grounded.
    const float k_GroundedRadius = .25f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the character is grounded.
    private Transform m_CeilingCheck;   // A position marking where to check for ceilings
    const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the character can stand up

    private Transform m_Back, m_Front;  //Transforms for the Front and back of character

    protected Animator m_Anim;            // Reference to the character's animator component.
    private Rigidbody2D m_Rigidbody2D;
    public bool m_FacingRight = true;  // For determining which way the character is currently facing.

    Collider2D[] colliders;

    bool dead;
    bool meleeAttack;
    bool rangedAttack;
    bool jump;
    /// <summary>
    /// to enable/disable the faced direction based on movemment
    /// </summary>
    public bool manualFaceDirection;
    public int xDir = 0, deltaXDir = 0;

    /// <summary>
    /// Which direction the force should be applied in
    /// </summary>
    Vector2 moveVector = Vector2.right;

    private void Awake()
    {
        // Setting up references.
        m_GroundCheck = transform.Find("GroundCheck");
        m_CeilingCheck = transform.Find("CeilingCheck");
        m_Back = transform.Find("Back");
        m_Front = transform.Find("Front");
        m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        manualFaceDirection = false;
    }

    private void FixedUpdate()
    {
        if (m_Anim.runtimeAnimatorController)
        {
            m_Grounded = false;

            //The character is grounded if a circlecast to the groundcheck position hits anything designated as ground
            //This can be done using layers instead but Sample Assets will not overwrite your project settings.
            colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    m_Grounded = true;
                }
            }

            m_Anim.SetBool("Ground", m_Grounded);

            // Set the vertical animation
            m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);

            if (m_Grounded)
            {
                //Fire ray behind player and in front
                RaycastHit2D backHit = Physics2D.Raycast(m_Back.position,
                    -transform.up, 1, m_WhatIsGround);

                RaycastHit2D frontHit = Physics2D.Raycast(m_Front.position,
                    -transform.up, 1, m_WhatIsGround);


                Debug.DrawRay(m_Back.position, backHit.normal, Color.cyan);
                Debug.DrawRay(m_Front.position, frontHit.normal, Color.cyan);

                //Finding the average between the two
                Vector2 avgNormal = (backHit.normal + frontHit.normal) / 2;

                Debug.DrawRay(m_CeilingCheck.position, avgNormal, Color.blue);

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
    }

    //This works with Triggers in the world to prevent players from flying off of Ramps
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "RampUp")
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

    public virtual void Move(Hashtable animData)
    {
        //rangedAttack = false;
        dead = (bool)animData["Dead"];
        meleeAttack = (bool)animData["MeleeAttack"];
        rangedAttack = (bool)animData["RangedAttack"];
        jump = (bool)animData["Jump"];
        xDir = (int)animData["Move"];

        //If dead and not in dead state we want to trigger the death animation
        if (TimeObjectManager.timeState == TimeState.Forward)
        {
            m_Anim.SetBool("Dead", dead);
        }

        //We want to make sure attacks are triggered only when time is moving forwards
        if (TimeObjectManager.timeState == TimeState.Forward)
        {
            if(meleeAttack)
                m_Anim.SetTrigger("MeleeAttack");

            if(rangedAttack)
                m_Anim.SetTrigger("RangedAttack");
        }
        
        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl && !dead)
        {

            // The Speed animator parameter is set to the absolute value of the horizontal input.
            m_Anim.SetFloat("Speed", Mathf.Abs(xDir));

            Vector2 directionForce = moveVector * xDir * m_MoveForce * Time.deltaTime;

            m_Rigidbody2D.AddForce(directionForce, ForceMode2D.Impulse);

            Debug.DrawRay(transform.position, directionForce.normalized, Color.red);

            //If the player stops moving or changes direction then we reduce Velocity to zero
            if ((deltaXDir != xDir && m_Grounded))
                m_Rigidbody2D.velocity = new Vector2(0, 0);

            m_Rigidbody2D.velocity = new Vector2(Mathf.Clamp(
                    m_Rigidbody2D.velocity.x, -m_MaxSpeed, m_MaxSpeed),
                    m_Rigidbody2D.velocity.y);

            if (!manualFaceDirection)
                DirectionLogic(xDir);
            else
                DirectionLogic((int)animData["ManualFacedDirection"]);
            
        }
        // If the player should jump...
        if (m_Grounded && jump && m_Anim.GetBool("Ground"))
        {
            m_Grounded = false;
            m_Anim.SetBool("Ground", false);
            //Since the direction force for movement can result in positive Y Velocity 
            //we want to reset Y Velocity before a jump to prevent them going flying
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0); 
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce), ForceMode2D.Impulse);
        }

        deltaXDir = xDir;
    }

    private void DirectionLogic(int desiredDirection)
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
}
