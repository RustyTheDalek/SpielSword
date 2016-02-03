using System;
using UnityEngine;
using UnityStandardAssets._2D;

/// <summary>
/// Class for controller Villager
/// </summary>
[RequireComponent(typeof(PlatformerCharacter2D))]
public class Villager : MonoBehaviour
{
    public PlatformerCharacter2D m_Character;
    private bool m_Jump;
    public float xDir;

    public float health = 1;

    public Vector3 startingPos;

    //Target X position for Villager to aim for when they're waiting in queue
    public float targetX;

    VillagerAnimData animData;

    /// <summary>
    /// Whether the Villager is advancing in the queue
    /// </summary>
    public bool advancing;

    /// <summary>
    /// If the Villager alive?
    /// </summary>
    public bool alive
    {
        get
        {
            return health > 0;
        }
    }

    //Whether it's currently in control by the player
    public bool activePlayer = false;

    private void Awake()
    {
        m_Character = GetComponent<PlatformerCharacter2D>();
        startingPos = transform.position;
    }

    private void Update()
    {
        if (alive)
        {
            if (activePlayer)
            {   

                xDir = 0;
                xDir = ((Input.GetKey(KeyCode.D)) ? 1 : xDir);
                xDir = ((Input.GetKey(KeyCode.A)) ? -1 : xDir);

                if (!m_Jump)
                {
                    // Read the jump input in Update so button presses aren't missed.
                    m_Jump = Input.GetKeyDown(KeyCode.Space);
                }
            }
            else
            {
                if (Mathf.Abs(transform.localPosition.x - targetX) > .5f)
                {
                    xDir = Mathf.Clamp01(targetX - transform.localPosition.x);
                }
                else
                {
                    advancing = false;
                }   
            }
        }
        else
        {
        }
    }

    private void FixedUpdate()
    {
        if (activePlayer)
        {
            xDir = ((Input.GetKey(KeyCode.D)) ? 1 : xDir);
            xDir = ((Input.GetKey(KeyCode.A)) ? -1 : xDir);

            animData.move = xDir;
            animData.jump = m_Jump;
            animData.attack = Input.GetKey(KeyCode.DownArrow);
            animData.dead = !alive;

            // Pass all parameters to the character control script.
            m_Character.Move(animData);
            m_Jump = false;
        }
        else
        {
            animData.move = xDir;
            animData.dead = false;
            animData.jump = false;
            animData.attack = false;
            m_Character.Move(animData);
        }
    }
    
    /// <summary>
    /// Kills player
    /// </summary>
    public void Kill()
    {
        health = 0;
    }

    /// <summary>
    /// Sets target for Villager to aim for in the x axis
    /// </summary>
    /// <param name="xPos">Target X Position</param>
    public void SetTarget(float xPos)
    {
        targetX = xPos;
        advancing = true;
    }
}

