using UnityEngine;

/// <summary>
/// Class for controlling Villager
/// Created GGJ16
/// Updated by : Ian Jones - 10/04/18
/// </summary>
[RequireComponent(typeof(GroundCharacter2D))]
public abstract class Villager : Character
{
    #region Public Variables

    public VillagerState villagerState = VillagerState.PresentVillager;

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

    public SpriteRenderer hat;

    public VillagerTimeObject vTO;

    /// <summary>
    /// If a Villager is shielded they are unable to take damage from attacks
    /// </summary>
    public bool shielded = false;

    /// <summary>
    /// If a Villager is able to carry out actions
    /// </summary>
    public bool canAct = true;

    public float damageMult = 1;

    public bool deathEnd = false;

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

    protected GroundCharacter2D m_Ground;

    public SpecialType specialType = SpecialType.Press;
     
    /// <summary>
    /// Temporary GameObject for tracking Ranged attack
    /// </summary>
    protected GameObject rangedAtk;

    /// <summary>
    /// Spawn position for Ranged projectiles
    /// </summary>
    protected Transform rangedTrans;

    protected float rangedProjectileStrength = 25;

    protected readonly int m_HashMeleeParam = Animator.StringToHash("MeleeAttack");
    protected readonly int m_HashRangedParam = Animator.StringToHash("RangedAttack");
    protected readonly int m_HashSpecialParam = Animator.StringToHash("PlayerSpecial");
    protected readonly int m_HashGroundPara = Animator.StringToHash("Ground");

    #endregion

    #region Private Variables

    bool attack = false;
    #endregion

    static GameObject _Projectile;

    protected static GameObject Projectile
    {
        get
        {
            if (_Projectile == null)
            {
                _Projectile = new GameObject();

                _Projectile = (GameObject)Resources.Load("Range");

                _Projectile.CreatePool(100);
            }

            return _Projectile;
        }
    }

    //Input variables
    protected bool m_Jump;
    protected bool meleeAttack, rangedAttack;
    protected bool playerSpecial;
    
    //Control variables
    protected bool canSpecial = true;

    protected override void Awake()
    {
        base.Awake();

        m_Ground = GetComponent<GroundCharacter2D>();
        deathEffect = GetComponentInChildren<ParticleSystem>();
        vTO = GetComponent<VillagerTimeObject>();

        switch(attackType)
        {
            case AttackType.Ranged:

                rangedTrans = GameObject.Find(this.name + "/RangedTransform").transform;

                break;

            case AttackType.Melee:

                try
                {
                    melee = GetComponentInChildren<MeleeAttack>().GetComponentInChildren<CircleCollider2D>();
                }
                catch
                {
                    Debug.LogWarning("No Melee component on " + name);
                }

                break;
        }

        //villagerState = VillagerState.Waiting;

        //TODO: FIX THIS TRASH
        if (hat == null)
            hat = transform.Find("Hat").GetComponent<SpriteRenderer>();


        PlayerCollisions = GetComponents<CircleCollider2D>();

    }

    protected override void Update()
    {
        base.Update();

        if (Alive)
        {
            switch (villagerState)
            {
                case VillagerState.PresentVillager:

                    if (canAct)
                    {
                        //Get PlayerMovement
                        moveDir = Vector2.zero;
                        moveDir = new Vector2((int)Input.GetAxisRaw("Horizontal"), 0);

                        attack = Input.GetButtonDown("Attack");

                        if (attack)
                        {
                            switch (attackType)
                            {
                                case AttackType.Melee:
                                    m_Animator.SetTrigger(m_HashMeleeParam);
                                    break;

                                case AttackType.Ranged:

                                    m_Animator.SetTrigger(m_HashRangedParam);
                                    break;
                            }
                        }
                        switch (specialType)
                        {
                            case SpecialType.Hold:
                                OnSpecial(Input.GetButton("Special"));
                                break;

                            case SpecialType.Press:
                                OnSpecial(Input.GetButtonDown("Special"));
                                break;
                        }

                        m_Jump = Input.GetButtonDown("Jump");
                    }

                    m_Animator.SetBool(m_HashGroundPara, m_Ground.m_Grounded);

                    break;
            }
        }

        //Whether Villager is grounded is decided regardless of villagers state
        m_Ground.Move(moveDir, m_Jump);
    }
    
    /// <summary>
    /// Kills player
    /// </summary>
    public void Kill()
    {
        health = 0;
    }

    public void SetTrigger(bool active)
    {
        PlayerCollisions[0].isTrigger = active;
        PlayerCollisions[1].isTrigger = active;
    }

    public virtual void OnSpecial(bool _PlayerSpecial)
    {
        playerSpecial = _PlayerSpecial;

        switch(specialType)
        {
            case SpecialType.Hold:

                if (canSpecial)
                    m_Animator.SetBool(m_HashSpecialParam, _PlayerSpecial);
                else
                    m_Animator.SetBool(m_HashSpecialParam, false);
                break;

            case SpecialType.Press:

                if (playerSpecial)
                    m_Animator.SetTrigger(m_HashSpecialParam);
                break;
        }
    }

    public virtual void OnHit()
    {
        if (!shielded)
        {
            health--;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == (LayerMask.NameToLayer("Minion")) && !LevelManager.GodMode)
        {
            Debug.Log("It was minion");
            OnHit();
        }
    }

    public virtual void FireProjectile()
    {
        if (villagerState == VillagerState.PresentVillager)
        {

            rangedAtk = Projectile.Spawn(rangedTrans.position);
            rangedAtk.GetComponent<VillagerAttack>().damageMult = damageMult;

            float direction = rangedTrans.position.x - transform.position.x;

            rangedAtk.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Sign(direction)
                , 0) * rangedProjectileStrength, ForceMode2D.Impulse);
        }
    }

    public void SetDamageMult(int val)
    {
        damageMult = val;
        if (melee)
        {
            melee.GetComponent<MeleeAttack>().damageMult = val;
        }
    }

    public void OnDeathEnd()
    {
        deathEnd = true;
    }
}

