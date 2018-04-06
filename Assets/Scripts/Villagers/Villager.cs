using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class for controlling Villager
/// </summary>
[RequireComponent(typeof(VillagerCharacter2D))]
public abstract class Villager : Character
{
    #region Public Variables

    public delegate void ActiveVillagerDeath();
    public static event ActiveVillagerDeath OnActiveVillagerDeath;

    public VillagerState villagerState = VillagerState.Waiting;

    //Target X position for Villager to aim for when they're waiting in queue
    public float targetX;

    //public VillagerAnimData vAnimData;

    //Whether the Villager is advancing in the queue
    public bool advancing;

    //Whether it's currently in control by the player
    public bool ActivePlayer
    {
        get
        {
            return villagerState == VillagerState.PresentVillager;
        }
    }

    public ParticleSystem deathEffect;

    //TODO: bring this up a level when attacks for minions are sorted
    public CircleCollider2D melee;

    public CircleCollider2D[] PlayerCollisions;

    public Transform hat;

    public VillagerTimeObject vTO;

    /// <summary>
    /// If a Villager is shielded they are unable to take damage from attacks
    /// </summary>
    public bool shielded = false;

    public float damageMult = 1;

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

    #endregion

    #region Protected Variables

    protected VillagerCharacter2D m_Villager;
    protected Animator m_Animator;

    protected SpecialType specialType;
     
    /// <summary>
    /// Temporary GameObject for tracking Ranged attack
    /// </summary>
    protected GameObject rangedAtk;

    /// <summary>
    /// Spawn position for Ranged projectiles
    /// </summary>
    protected Transform rangedTrans;

    protected float rangedProjectileStrength = 25;

    #endregion

    #region Private Variables
    #endregion

    public override void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_Villager = GetComponent<VillagerCharacter2D>();
        deathEffect = GetComponentInChildren<ParticleSystem>();
        vTO = GetComponent<VillagerTimeObject>();

        switch(attackType)
        {
            case AttackType.Ranged:

                rangedTrans = GameObject.Find(this.name + "/RangedTransform").transform;

                break;

            case AttackType.Melee:

                melee = GetComponentInChildren<MeleeAttack>().GetComponentInChildren<CircleCollider2D>();

                break;
        }

        //villagerState = VillagerState.Waiting;

        //TODO: FIX THIS TRASH

        PlayerCollisions = GetComponents<CircleCollider2D>();

        CreateHashtable();

        animData.Add("PlayerSpecial", false);
        animData.Add("CanSpecial", true);
        animData.Add("PlayerSpecialIsTrigger", false);
        animData.Add("Martyed", false);

        //vAnimData = new VillagerAnimData()
        //{
        //    move = 0,

        //    jump = false,
        //    meleeAttack = false,
        //    rangedAttack = false,
        //    dead = false,
        //    playerSpecial = false,
        //    canSpecial = true,
        //    playerSpecialIsTrigger = false,

        //    martyed = false
        //};

        hat = transform.Find("Hat");

    }

    public virtual void Start()
    {
        if (!m_Animator.runtimeAnimatorController)
        {
            Debug.LogError("No Animator set for " + gameObject.name);
        }
    }

    public override void Update()
    {
        switch(villagerState)
        {
            case VillagerState.PresentVillager:

                //Invokes current villager death event
                if (!Alive)
                {
                    if(OnActiveVillagerDeath != null)
                        OnActiveVillagerDeath();
                }
                //Get PlayerMovement
                xDir = 0;
                xDir = ((Input.GetKey(KeyCode.D)) ?  1 : xDir);
                xDir = ((Input.GetKey(KeyCode.A)) ? -1 : xDir);

                switch (attackType)
                {
                    case AttackType.Melee:
                        animData["MeleeAttack"] = Input.GetKey(KeyCode.DownArrow);
                        m_Villager.CanAttack((bool)animData["MeleeAttack"]);
                        break;

                    case AttackType.Ranged:
                        animData["RangedAttack"] = Input.GetKeyDown(KeyCode.DownArrow);
                        m_Villager.CanAttack((bool)animData["RangedAttack"]);
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
                if (advancing)
                {
                    if (Mathf.Abs(transform.localPosition.x - targetX) > .5f)
                    {
                        xDir = (int)Mathf.Clamp01(targetX - transform.localPosition.x);
                    }
                    else
                    {
                        advancing = false;
                        xDir = 0;
                    }
                }

                break;

            case VillagerState.PastVillager:

                break;
        }
    }

    public override void FixedUpdate()
    {
        switch (villagerState)
        {
            case VillagerState.PresentVillager:

                animData["Move"] = xDir;
                animData["Jump"] = m_Jump;

                //animData.dead = !alive;

                m_Villager.Move(animData);
                m_Jump = false;
                break;

            case VillagerState.Waiting:

                animData["Move"] = xDir;
                animData["Dead"] = false;
                animData["Jump"] = false;
                animData["MeleeAttack"] = false;
                m_Villager.Move(animData);
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

    public virtual void OnSpecial(bool _PlayerSpecial)
    {
        animData["PlayerSpecial"] = _PlayerSpecial;
    }

    public virtual void OnHit()
    {
        if (!shielded)
        {
            health--;
        }
    }

    public virtual void OnPastHit(Collider2D collider)
    {
        if (collider.GetComponent<BossAttack>() && !(bool)animData["Dead"] &&
            Game.timeState == TimeState.Forward)
        {
            Debug.Log("Past Villager Hit By Boss Attack");
            animData["Dead"] = true;
            m_Villager.Move(animData);

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

    public virtual void FireProjectile()
    {
        if (villagerState == VillagerState.PresentVillager)
        {

            rangedAtk = AssetManager.Projectile.Spawn(rangedTrans.position);
            rangedAtk.GetComponent<VillagerAttack>().damageMult = damageMult;

            float direction = rangedTrans.position.x - transform.position.x;

            rangedAtk.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Sign(direction)
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
        damageMult = val;
        if (melee)
        {
            melee.GetComponent<MeleeAttack>().damageMult = val;
        }
    }
}

