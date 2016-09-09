using System;
using UnityEngine;
using UnityStandardAssets._2D;

/// <summary>
/// Class for controller Villager
/// </summary>
[RequireComponent(typeof(PlatformerCharacter2D))]
public abstract class Villager : MonoBehaviour
{
    public PlatformerCharacter2D m_Character;
    private bool m_Jump;
    public float xDir;

    public float health = 1;

    public Vector3 startingPos;

    //Target X position for Villager to aim for when they're waiting in queue
    public float targetX;

    public  VillagerAnimData animData;

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
    public bool activePlayer
    {
        get
        {
            return villagerState == VillagerState.CurrentVillager;
        }
    }

    public VillagerState villagerState;

    public ParticleSystem deathEffect;

    public CircleCollider2D melee;

    public CircleCollider2D[] PlayerCollisions;

    public virtual void Awake()
    {
        m_Character = GetComponent<PlatformerCharacter2D>();
        deathEffect = GetComponentInChildren<ParticleSystem>();
        startingPos = transform.position;

        //villagerState = VillagerState.Waiting;

        //TO-DO: FIX THIS TRASH
        melee = GetComponentInChildren<MeleeAttack>().GetComponentInChildren<CircleCollider2D>();

        PlayerCollisions = GetComponents<CircleCollider2D>();

        animData.canSpecial = true;
    }

    public virtual void Update()
    {
        switch(villagerState)
        {
            case VillagerState.CurrentVillager:   

                //Get PlayerMovement
                xDir = 0;
                xDir = ((Input.GetKey(KeyCode.D)) ? 1 : xDir);
                xDir = ((Input.GetKey(KeyCode.A)) ? -1 : xDir);

                animData.attack = Input.GetKey(KeyCode.DownArrow);

                OnSpecial(Input.GetKey(KeyCode.LeftArrow));

                if (!m_Jump)
                {
                    // Read the jump input in Update so button presses aren't missed.
                    m_Jump = Input.GetKeyDown(KeyCode.Space);
                }

                break;

            case VillagerState.Waiting:

                //Wander/AI Code
                if (Mathf.Abs(transform.localPosition.x - targetX) > .5f)
                {
                    xDir = Mathf.Clamp01(targetX - transform.localPosition.x);
                }
                else
                {
                    advancing = false;
                }

                break;
        }
    }

    private void FixedUpdate()
    {
        switch (villagerState)
        {
            case VillagerState.CurrentVillager:

                animData.move = xDir;
                animData.jump = m_Jump;

                //animData.dead = !alive;

                m_Character.Move(animData);
                m_Jump = false;
                break;

            case VillagerState.Waiting:

                animData.move = xDir;
                animData.dead = false;
                animData.jump = false;
                animData.attack = false;
                m_Character.Move(animData);
                break;
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

    public void SetTrigger(bool active)
    {
        PlayerCollisions[0].isTrigger = active;
        PlayerCollisions[1].isTrigger = active;
    }

    public abstract void OnSpecial(bool playerSpecial);

    public virtual void OnHit()
    {
        health--;
    }
}

