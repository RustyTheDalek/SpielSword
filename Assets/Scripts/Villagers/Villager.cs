using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Class for controller Villager
/// </summary>
[RequireComponent(typeof(PlatformerCharacter2D))]
public abstract class Villager : MonoBehaviour
{
    #region Public Variables

    public VillagerState villagerState = VillagerState.Waiting;
    public AttackType villagerAttackType = AttackType.Melee;

    public float xDir;

    public float health = 1;

    public Vector3 startingPos;

    //Target X position for Villager to aim for when they're waiting in queue
    public float targetX;

    public VillagerAnimData animData;

    //Whether the Villager is advancing in the queue
    public bool advancing;

    public bool Alive
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
            return villagerState == VillagerState.PresentVillager;
        }
    }

    public ParticleSystem deathEffect;

    public CircleCollider2D melee;

    public CircleCollider2D[] PlayerCollisions;

    public Transform hat;

    public VillagerTimeObject vTO;

    /// <summary>
    /// If this is the current Villager (Villager being controlled by the player)
    /// </summary>
    public bool CurrentVillager
    {
        get
        {
            return villagerState == VillagerState.PresentVillager;
        }
    }

    #region pastVillager variables

    /// <summary>
    /// Special time in which Villagers "Finish" Dying.
    /// </summary>
    public int reverseDeathTimeStamp = 0;

    #endregion

    #endregion

    #region Protected Variables

    protected PlatformerCharacter2D m_Character;
    protected Animator m_Animator;

    protected SpecialType specialType;

    #endregion

    #region Private Variables

    float rangedProjectileStrength = 25;

    bool m_Jump;

    Transform rangedTrans;

    #endregion

    public virtual void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_Character = GetComponent<PlatformerCharacter2D>();
        deathEffect = GetComponentInChildren<ParticleSystem>();
        vTO = GetComponent<VillagerTimeObject>();
        startingPos = transform.position;

        rangedTrans = GameObject.Find(this.name + "/RangedTransform").transform;

        //villagerState = VillagerState.Waiting;

        //TO-DO: FIX THIS TRASH
        melee = GetComponentInChildren<MeleeAttack>().GetComponentInChildren<CircleCollider2D>();

        PlayerCollisions = GetComponents<CircleCollider2D>();

        animData.canSpecial = true;
        animData.playerSpecialIsTrigger = false;
        //villagerState = VillagerState.Waiting;

        hat = transform.Find("Hat");

    }

    public virtual void Start()
    {
        if (!m_Animator.runtimeAnimatorController)
        {
            Debug.LogError("No Animator set for " + gameObject.name);
        }
    }

    public virtual void Update()
    {
        animData.dead = !Alive;

        switch(villagerState)
        {
            case VillagerState.PresentVillager:   

                //Get PlayerMovement
                xDir = 0;
                xDir = ((Input.GetKey(KeyCode.D)) ?  1 : xDir);
                xDir = ((Input.GetKey(KeyCode.A)) ? -1 : xDir);

                switch (villagerAttackType)
                {
                    case AttackType.Melee:
                        animData.meleeAttack = Input.GetKey(KeyCode.DownArrow);
                        CanAttack(animData.meleeAttack);
                        break;

                    case AttackType.Ranged:
                        animData.rangedAttack = Input.GetKey(KeyCode.DownArrow);
                        CanAttack(animData.rangedAttack);
                        break;    
                }

                switch (specialType)
                {
                    case SpecialType.Hold:
                        OnSpecial(Input.GetKey(KeyCode.LeftArrow));
                        break;

                    case SpecialType.Press:
                        OnSpecial(Input.GetKeyDown(KeyCode.LeftArrow));
                        break;
                }

                if (!m_Jump)
                {
                    // Read the jump input in Update so button presses aren't missed.
                    m_Jump = Input.GetKeyDown(KeyCode.Space);
                }

#if UNITY_EDITOR

                if (Input.GetKeyDown(KeyCode.K))
                {
                    Kill();
                }
#endif

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

            case VillagerState.PastVillager:

                if (Game.timeState == TimeState.Backward)
                {
                    if (reverseDeathTimeStamp != 0 &&
                        reverseDeathTimeStamp == Game.t)
                    {
                        //Debug.Break();
                        Debug.Log("Villager Un-Dying");
                        //m_Animator.SetTrigger("ExitDeath");
                    }
                }
                break;
        }
    }

    public void CanAttack(bool attack)
    {
        if (!attack)
        {
            m_Animator.SetBool("CanAttack", true);
        }
    }

    private void FixedUpdate()
    {
        switch (villagerState)
        {
            case VillagerState.PresentVillager:

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
                animData.meleeAttack = false;
                m_Character.Move(animData);
                break;

            case VillagerState.PastVillager:

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

    public abstract void OnSpecial(bool _PlayerSpecial);

    public virtual void OnHit()
    {
        health--;
    }

    public virtual void OnPastHit(Collider2D collider)
    {
        if (collider.GetComponent<BossAttack>() && !animData.dead &&
            Game.timeState == TimeState.Forward)
        {
            Debug.Log("Past Villager Hit By Boss Attack");
            animData.dead = true;
            m_Character.Move(animData);

            GetComponentInChildren<ParticleSystem>().Play();
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (villagerState == VillagerState.PastVillager)
        {
            OnPastHit(collider);
        }
    }

    public void FireProjectile()
    {
        if (villagerState == VillagerState.PresentVillager)
        {
            Debug.Log("Ranged Attack");

            GameObject attack = AssetManager.projectile.Spawn(rangedTrans.position);

            float direction = rangedTrans.position.x - transform.position.x;

            attack.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Sign(direction)
                , 0) * rangedProjectileStrength, ForceMode2D.Impulse);
        }
    }

    public void CannotAttack()
    {
        GetComponentInChildren<MeleeAttack>().GetComponent<CircleCollider2D>().enabled = false;
    }

    public void CanAttack()
    {
        GetComponentInChildren<MeleeAttack>().GetComponent<CircleCollider2D>().enabled = true;
    }

    public void SetDamageMult(int val)
    {
        melee.GetComponent<MeleeAttack>().damageMult = val;
        AssetManager.projectile.GetComponent<VillagerAttack>().damageMult = val;
    }
}

