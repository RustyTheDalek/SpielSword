using System.Collections;
using System.IO;
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

    public SpecialType specialType = SpecialType.Press;

    public VillagerState villagerState = VillagerState.PresentVillager;

    //Whether it's currently in control by the player
    public bool ActivePlayer
    {
        get
        {
            return villagerState == VillagerState.PresentVillager;
        }
    }

    public SpriteRenderer Sprite
    {
        get
        {
            return m_Sprite;
        }
    }

    public GroundCharacter2D GroundController2D
    {
        get
        {
            return m_Ground;
        }
    }

    //TODO: bring this up a level when attacks for minions are sorted
    public CircleCollider2D melee;

    public CircleCollider2D[] PlayerCollisions;

    public SpriteRenderer hat;

    /// <summary>
    /// If a Villager is shielded they are unable to take damage from attacks
    /// </summary>
    public bool shielded = false;

    /// <summary>
    /// If a Villager is able to carry out actions
    /// </summary>
    public bool canAct = true;

    /// <summary>
    /// Whether a Villager can move
    /// </summary>
    protected bool canMove = true;

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

    public VillagerTimeObject m_VTimeObject;

    public SpriteMask portal;

    #endregion

    #region Protected Variables

    protected SpriteRenderer m_Sprite;
    protected GroundCharacter2D m_Ground;
     
    /// <summary>
    /// Temporary GameObject for tracking Ranged attack
    /// </summary>
    protected GameObject rangedAtk;

    protected float rangedProjectileStrength = 25;

    protected AudioSource EffectNoise;

    protected MeleeAttack classMeleeAttack;

    protected readonly int m_HashMeleeParam = Animator.StringToHash("MeleeAttack");
    protected readonly int m_HashRangedParam = Animator.StringToHash("RangedAttack");
    protected readonly int m_HashSpecialParam = Animator.StringToHash("PlayerSpecial");
    protected readonly int m_HashGroundParam = Animator.StringToHash("Ground");

    #endregion

    #region Private Variables

    bool attack = false;
    #endregion

    protected static AssetBundle abilities;

    //Input variables
    protected bool m_Jump;
    protected bool meleeAttack, rangedAttack;
    protected bool playerSpecial;
    
    //Control variables
    protected bool canSpecial = true;

    protected override void Awake()
    {
        base.Awake();

        portal = GetComponentInChildren<SpriteMask>();

        m_Sprite = GetComponentInChildren<SpriteRenderer>();
        m_Ground = GetComponentInChildren<GroundCharacter2D>();
        m_VTimeObject = GetComponent<VillagerTimeObject>();
        m_rigidbody = transform.Find("Motion").GetComponent<Rigidbody2D>();

        EffectNoise = GetComponent<AudioSource>();

        if (abilities == null)
            abilities = AssetBundle.LoadFromFile(Path.Combine(
                Application.streamingAssetsPath, "AssetBundles/abilities"));

        switch (attackType)
        {
            case AttackType.Ranged:

                rangedTrans = GameObject.Find(this.name + "/Character/RangedTransform").transform;

                break;

            case AttackType.Melee:

                try
                {
                    classMeleeAttack = GetComponentInChildren<MeleeAttack>();
                    melee = classMeleeAttack.GetComponentInChildren<CircleCollider2D>();
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
            Debug.LogWarning("No hat set");


        PlayerCollisions = GetComponents<CircleCollider2D>();

    }

    protected void Start()
    {
        SceneLinkedSMB<Villager>.Initialise(m_Animator, this);
    }

    protected override void Update()
    {
        m_Animator.SetBool(m_HashGroundParam, m_Ground.m_Grounded);

        base.Update();

        if (Alive)
        {
            switch (villagerState)
            {
                case VillagerState.PresentVillager:

                    if (canAct)
                    {
                        //Get PlayerMovement
                        if (canMove)
                        {
                            moveDir = Vector2.zero;
                            moveDir = new Vector2((int)Input.GetAxisRaw("Horizontal"), 0);
                        }

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

                        if (!m_Jump)
                        {
                            m_Jump = Input.GetButtonDown("Jump");
                        }
                    }
                    break;
            }
        }

    }

    protected override void FixedUpdate()
    {
        if (!Alive)
            return;

        m_Animator.SetFloat("xSpeed", m_rigidbody.velocity.x);
        m_Animator.SetFloat("ySpeed", m_rigidbody.velocity.y);
        m_Animator.SetFloat("xSpeedAbs", Mathf.Abs(m_rigidbody.velocity.x));

        m_Ground.Move(moveDir, m_Jump);

        m_Jump = false;
    }

    /// <summary>
    /// Kills player
    /// </summary>
    [ContextMenu("Kill")]
    public void Kill()
    {
        health = 0;

        //Useful catch to prevent Animator from getting stuck on death;
        m_Animator.SetBool(m_HashMeleeParam, false);
        m_Animator.SetBool(m_HashRangedParam, false);
        m_Animator.SetBool(m_HashSpecialParam, false);

        OnDeath(Vector2.zero);
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
                    m_Animator.SetBool(m_HashSpecialParam, playerSpecial);
                else
                    m_Animator.SetBool(m_HashSpecialParam, false);
                break;

            case SpecialType.Press:

                if (playerSpecial && canSpecial)
                    m_Animator.SetTrigger(m_HashSpecialParam);
                break;
        }
    }

    public virtual void OnHit(Vector2 attackDirection)
    {
        if (!shielded)
        {
            health--;

            if (health <= 0)
            {
                OnDeath(attackDirection);
            }
        }
    }

    public override void OnDeath(Vector2 attackDirection)
    {
        this.NamedLog("Dead");

        PlayEffect();

        moveDir = Vector2.zero;
        m_rigidbody.velocity = Vector2.zero;


        m_Animator.SetFloat("xSpeed", 0);
        m_Animator.SetFloat("ySpeed", 0);
        m_Animator.SetFloat("xSpeedAbs", 0);
        m_Animator.SetBool(m_HashDeadParam, true);
        m_Ground.SetCharacterCollisions(false);
        

        if (GameManager.gManager)
            GameManager.gManager.UnlockHat("Anor");
    }

    public virtual void FireProjectile()
    {
        if (villagerState == VillagerState.PresentVillager)
        {

            rangedAtk = abilities.LoadAsset<GameObject>("Range").Spawn(rangedTrans.position);
            rangedAtk.GetComponent<VillagerAttack>().damageMult = damageMult;

            float direction = rangedTrans.position.x - m_rigidbody.transform.position.x;

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

    public void EnableInsideMask()
    {
        m_Sprite.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
    }

    public void EnableOutsidMask()
    {
        m_Sprite.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
    }

    public void NoMask()
    {
        m_Sprite.maskInteraction = SpriteMaskInteraction.None;
    }

    public void DisableAnimator()
    {
        m_Animator.enabled = false;
    }

    public IEnumerator SlowTime()
    {
        float startTime = Time.time;

        while (Time.timeScale > .5f)
        {
            Time.timeScale = Mathf.Lerp(1, 0, Time.time - startTime);

            yield return null;
        }
    }

    private void PlayEffect()
    {
        if (EffectNoise)
        {
            EffectNoise.Stop();
            EffectNoise.Play();
        }
        else
        {
            Debug.LogWarning("No effect assigned, is this intended");
        }
    }

    public void PlayMeleeEffect()
    {
        classMeleeAttack.PlayEffect();
    }
}

