using UnityEngine;
using System.Collections;

/// <summary>
/// Controls the Animation of the WorldMap Spiel character
/// Created : 10/04/18
/// </summary>
public class MapCharacter2D : MonoBehaviour
{
    [SerializeField] private float m_MaxSpeed = 10f;    // The fastest the character can travel in the x axis.

    private Rigidbody2D m_Rigidbody2D;
    protected Animator m_Anim;                          // Reference to the character's animator component.
    public bool m_FacingRight = true;                   // For determining which way the character is currently facing.

    Vector2 direction;

    private void Awake()
    {
        // Setting up references.
        m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public virtual void Move(Hashtable animData)
    {
        direction = (Vector2)animData["Direction"];
        
        m_Anim.SetFloat("vSpeed", direction.x);
        m_Anim.SetFloat("hSpeed", direction.y);

        //m_Rigidbody2D.velocity = new Vector2(xDir * m_MaxSpeed, m_Rigidbody2D.velocity.y);

        // If the input is moving the player right and the player is facing left...
        if (direction.x > 0 && !m_FacingRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (direction.x < 0 && m_FacingRight)
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
