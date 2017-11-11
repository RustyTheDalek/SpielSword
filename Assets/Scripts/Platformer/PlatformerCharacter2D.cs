using System;
using UnityEngine;

/// <summary>
/// Controls the Animation and movement of the 2D Characters
/// </summary>
public class PlatformerCharacter2D : MonoBehaviour
{
    [SerializeField] private bool m_AirControl = true;                 // Whether or not a player can steer while jumping;
    [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the character can travel in the x axis.
    [SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the character jumps.
    //[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

    private Transform m_GroundCheck;    // A position marking where to check if the character is grounded.
    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the character is grounded.
    private Transform m_CeilingCheck;   // A position marking where to check for ceilings
    const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the character can stand up

    protected Animator m_Anim;            // Reference to the character's animator component.
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the character is currently facing.

    private void Awake()
    {
        // Setting up references.
        m_GroundCheck = transform.Find("GroundCheck");
        m_CeilingCheck = transform.Find("CeilingCheck");
        m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        if (m_Anim.runtimeAnimatorController)
        {
            m_Grounded = false;

            // The character is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
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

            //m_Anim.SetFloat("TimeDirection", (float)Game.timeState);
        }
    }


    public virtual void Move(PlatformerAnimData animData)
    {
        //If dead and not in dead state we want to trigger the death animation
        if (animData.dead && !m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Death") && 
            Game.timeState == TimeState.Forward)
        {
            m_Anim.SetTrigger("Dead");
        }

        //Want to make sure the Character reverses from death if time is moving backwards
        if (Game.timeState == TimeState.Backward)
        {
            //Debug.Log("Exiting Death");
            m_Anim.SetTrigger("ExitDeath");
        }

        //We want to make sure attacks are triggered only when time is moving forwards
        if (Game.timeState == TimeState.Forward)
        {
            m_Anim.SetBool("MeleeAttack", animData.meleeAttack);
            m_Anim.SetBool("RangedAttack", animData.rangedAttack);
        }
        
        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl && !animData.dead)
        {
            // Reduce the speed if crouching by the crouchSpeed multiplier
            animData.move = (!m_Grounded ? animData.move * .8f : animData.move);

            // The Speed animator parameter is set to the absolute value of the horizontal input.
            m_Anim.SetFloat("Speed", Mathf.Abs(animData.move));

            // Move the character
            //if (PresentVillager)
            //{
                m_Rigidbody2D.velocity = new Vector2(animData.move * m_MaxSpeed, m_Rigidbody2D.velocity.y);
            //}

            // If the input is moving the player right and the player is facing left...
            if (animData.move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
                // Otherwise if the input is moving the player left and the player is facing right...
            else if (animData.move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
        // If the player should jump...
        if (m_Grounded && animData.jump && m_Anim.GetBool("Ground"))
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            m_Anim.SetBool("Ground", false);
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }
    }

    public void CanAttack(bool attack)
    {
        if (!attack)
        {
            m_Anim.SetBool("CanAttack", true);
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
