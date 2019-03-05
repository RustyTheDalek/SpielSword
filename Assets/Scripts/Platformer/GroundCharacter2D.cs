using UnityEngine;

/// <summary>
/// Controllers for ground characters
/// </summary>
public class GroundCharacter2D : PlatformerCharacter2D {

    #region public Variables

    public bool m_Grounded, m_DeltaGrounded;            // Whether or not the character is grounded.
    public bool m_Sliding;

    /// <summary>
    /// to enable/disable the faced direction based on movement
    /// </summary>
    public bool m_ManualFaceDirection = false;

    [Tooltip("If enabled force directions will be applied in an absolute direction, " +
        "useful for characters that don't flip there sprite")]
    public bool m_AbsoluteMoveDirection = false;

    public bool m_AlignToGround;

    public float alignSpeed = 5;

    [Tooltip("How much drag the controller applys to stopping when the there is no desired move")]
    [Range(0, 1)]
    public float m_dragFactor;

    [Tooltip("How much is the character Sprite offset from the motion wheel")]
    [Range(-10, 10)]
    public float sprite_Offset;

    /// <summary>
    /// The Transform of the Character object and visual representation of the player
    /// </summary>
    public Transform m_Character;

    #endregion

    #region Protected Variables

    /// <summary>
    /// Which direction the walking force should be applied in
    /// </summary>
    protected Vector2 moveVector = Vector2.right;

    public bool FacingRight
    {
        get
        {
            if (m_AbsoluteMoveDirection)
            {
                return m_Front.localPosition.x > 0;
            }
            else
            {
                return m_Character.localScale.x > 0;
            }
        }
    }

    public Direction FacingDirection
    {
        get
        {
            if(m_AbsoluteMoveDirection)
            {
                if (m_Front.localPosition.x > 0)
                    return Direction.Right;
                else
                    return Direction.Left;
            }
            else
            {
                if (m_Character.localScale.x > 0)
                    return Direction.Right;
                else
                    return Direction.Left;
            }
        }
    }

    protected Vector2 ForwardDir
    {
        get
        {
            if (m_AbsoluteMoveDirection)
            {
                //o--> Local position of front is greater than 1 * right vector = right vector
                //<--o Local position of front is less than 1 * right vector = left vector
                return (Vector2.right * m_Front.transform.localPosition.x).normalized;
            }
            else
            {
                return transform.right;
            }
        }
    }


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
    private const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private Transform m_Ceiling;   // A position marking where to check for ceilings

    public Transform m_Back, m_Front;  //Transforms for the Front and back of character used when calculating what direction move force should be applied

    private Collider2D[] colliders;

    private bool jump;

    private Vector2 directionForce;
    #endregion

    protected override void Awake()
    {
        base.Awake();

        // Setting up references.
        m_Character = transform.Find("Character");
        m_Ground = m_Character.Find("GroundCheck");
        m_Ceiling = m_Character.Find("CeilingCheck");
        m_Back = m_Character.Find("Back");
        if (m_Back == null)
        {
            Debug.Log("No back transform creating one, pls add to prefab");
            m_Back = new GameObject("Back").transform;
            m_Back.transform.SetParent(transform);
            m_Back.localPosition = new Vector3(-.5f, .5f, 0);
        }
        m_Front = transform.Find("Character/Front");
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
        CalcMoveVector();
        GroundCheck();
    }

    protected void LateUpdate()
    {
        m_Character.transform.position = new Vector3(m_Rigidbody2D.transform.position.x,
                        m_Rigidbody2D.GetComponent<CircleCollider2D>().bounds.min.y + sprite_Offset,
                        m_Rigidbody2D.transform.position.z);

    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawRay(transform.position, directionForce.normalized);
    //    Gizmos.color = Color.cyan;
    //    Gizmos.DrawRay(transform.position, moveVector);
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawRay(transform.position, -transform.up);
    //}

    public virtual void Move(Vector2 moveDir, float _MaxSpeed = 15f, bool jump = false, float manualDirection = 1)
    {
        m_MaxSpeed = _MaxSpeed;

        Move(moveDir, jump, manualDirection);
    }

    public virtual void Move(Vector2 moveDir, bool jump = false, float manualDirection = 1)
    {
        // If the player should jump...
        if (m_Grounded && jump)
        {
            m_Grounded = false;
            //Since the direction force for movement can result in positive Y Velocity 
            //we want to reset Y Velocity before a jump to prevent them going flying
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
            m_Rigidbody2D.AddForce(Vector2.up * new Vector2(0f, m_JumpForce), ForceMode2D.Impulse);
        }

        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {
            //If the player stops moving or changes direction then we reduce xVelocity to zero
            if (m_Grounded && (Mathf.Abs(deltaMoveDir.x - moveDir.x) > .75f) && !m_Sliding)
            {
                if (m_Grounded)
                {
                    m_Rigidbody2D.velocity *= (1 - m_dragFactor);
                    m_Rigidbody2D.angularVelocity *= (1 - m_dragFactor);
                }
                else
                {
                    Debug.Log("MoveDirection different in air");
                    m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x *
                                            (1 - m_dragFactor), m_Rigidbody2D.velocity.y);

                    m_Rigidbody2D.angularVelocity *= (1 - m_dragFactor);
                }
            }

            if(!m_Sliding && moveDir.x == 0)
            {
                m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            else if(!m_Sliding)
            {
                m_Rigidbody2D.constraints = RigidbodyConstraints2D.None;
            }

            //directionForce = moveVector * moveDir.x * m_MoveForce;

            m_Rigidbody2D.AddForce(moveDir * m_MoveForce, ForceMode2D.Force);

            m_Rigidbody2D.velocity = new Vector2(Mathf.Clamp(
                    m_Rigidbody2D.velocity.x, -m_MaxSpeed, m_MaxSpeed),
                    m_Rigidbody2D.velocity.y);
        }

        if (m_ManualFaceDirection)
        {
            DirectionLogic(manualDirection);
        }
        else
        {
            DirectionLogic(moveDir.x);
        }

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
                -transform.up, transform.localScale.y * 1.2f, m_WhatIsGround);

            RaycastHit2D midHit = Physics2D.Raycast(m_Ceiling.position,
                -transform.up, transform.localScale.y * 2f, m_WhatIsGround);

            RaycastHit2D frontHit = Physics2D.Raycast(m_Front.position,
                -transform.up, transform.localScale.y * 1.2f, m_WhatIsGround);

            //if (backHit)
            //    Debug.DrawRay(m_Back.position, backHit.normal, Color.cyan);

            //if (frontHit)
            //    Debug.DrawRay(m_Front.position, frontHit.normal, Color.cyan);

            //Finding the average between the two
            Vector2 avgNormal = (backHit.normal + midHit.normal + frontHit.normal) / 3;

            if (m_AlignToGround)
            {
                //transform.up = avgNormal;
                m_Character.transform.up = Vector3.Slerp(m_Character.transform.up, avgNormal, Time.deltaTime * alignSpeed);
            }

            //Debug.DrawRay(m_Ceiling.position, avgNormal, Color.blue);

            //Then we find the perpendicular Vector of the two to give us a nice 
            //direction along platform we're running across
            moveVector = new Vector2(avgNormal.y, -avgNormal.x);

            //Debug.DrawRay(transform.position, moveVector, Color.magenta);

        }
        else
        {
            //If in air we don't want odd move Vectors which can cause the character to fly off
            moveVector = ForwardDir;
            transform.up = Vector3.Slerp(transform.up, Vector2.up, Time.deltaTime * alignSpeed);

        }
    }

    /// <summary>
    /// See if Character is touching the Ground
    /// </summary>
    private void GroundCheck()
    {
        m_DeltaGrounded = m_Grounded;
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

        if(m_Grounded && !m_DeltaGrounded && m_Rigidbody2D.velocity.y < 0)
        {
            Debug.Log("Landing");
            //TODO:Make this slowly reduce velocity a short time
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x * 0, m_Rigidbody2D.velocity.y);
            m_Rigidbody2D.angularVelocity = 0;
        }
    }

    protected virtual void DirectionLogic(float desiredDirection)
    {
        // If the input is moving the character right and the character is facing left...
        if (desiredDirection > 0 && !FacingRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the character left and the character is facing right...
        else if (desiredDirection < 0 && FacingRight)
        {
            // ... flip the player.
            Flip();
        }
    }

    private void Flip()
    {
        if (m_AbsoluteMoveDirection)
        {
            Transform tFront = m_Front;
            m_Front = m_Back;
            m_Back = tFront;
        }
        else
        {
            // Multiply the player's x local scale by -1.
            Vector3 theScale = m_Character.transform.localScale;
            theScale.x *= -1;
            m_Character.localScale = theScale;
        }
    }

    internal void MoveTo(Vector3 spawnPos)
    {
        m_Rigidbody2D.transform.position = spawnPos;
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
        //if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        //{
        //    m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
        //}

        if(collision.collider.tag == "Wall")
        {
            m_Rigidbody2D.velocity = Vector2.zero;
            m_Rigidbody2D.angularVelocity = 0;
        }
    }

    public void SetCharacterCollisions(bool active)
    {
        //m_Rigidbody2D.simulated = active;
        m_Rigidbody2D.velocity = Vector2.zero;
        m_Rigidbody2D.angularVelocity = 0;
    }

    public void SetSliding(bool active, PhysicsMaterial2D physMat)
    {
        m_Sliding = active;

        if (active)
            m_Rigidbody2D.sharedMaterial = physMat;
        else
            m_Rigidbody2D.sharedMaterial = physMat;
    }
}
