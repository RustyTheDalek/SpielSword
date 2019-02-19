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

    public SpecialType special1Type = SpecialType.Press,
                       special2Type = SpecialType.Press;

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

    public Transform CharacterPosition
    {
        get
        {
            return m_Ground.m_Character;
        }
    }

    public CircleCollider2D melee;

    public CircleCollider2D[] PlayerCollisions;

    public SpriteRenderer hat;

    /// <summary>
    /// If a Villager is able to carry out actions
    /// </summary>
    public bool canAct = true;

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

    /// <summary>
    /// Whether a Villager can move
    /// </summary>
    protected bool canMove = true;

    protected float damageMult = 1;

    protected float     maxSpeed = 15f,
                        moveSpeed;

    protected SpriteRenderer m_Sprite;
    protected GroundCharacter2D m_Ground;
     
    /// <summary>
    /// Temporary GameObject for tracking Ranged attack
    /// </summary>
    protected VillagerAttack rangedAtk;

    protected AudioSource DeathSound;

    protected static AssetBundle abilities;

    //Input variables
    protected bool m_Jump;
    protected bool attack = false;
    protected bool meleeAttack, rangedAttack;
    protected bool special1, special2;

    protected bool canSpecial = true;
    protected float rangedProjectileStrength = 25;

    protected readonly int m_HashMeleeParam = Animator.StringToHash("MeleeAttack");
    protected readonly int m_HashRangedParam = Animator.StringToHash("RangedAttack");
    protected readonly int m_HashSpecial1Param = Animator.StringToHash("Special1");
    protected readonly int m_HashSpecial2Param = Animator.StringToHash("Special2");
    protected readonly int m_HashGroundParam = Animator.StringToHash("Ground");

    protected string rangeName = "Range";

    #endregion

    protected override void Awake()
    {
        base.Awake();

        moveSpeed = maxSpeed;

        portal = GetComponentInChildren<SpriteMask>();
        m_Sprite = GetComponentInChildren<SpriteRenderer>();
        m_Ground = GetComponentInChildren<GroundCharacter2D>();
        m_VTimeObject = GetComponent<VillagerTimeObject>();
        m_rigidbody = transform.Find("Motion").GetComponent<Rigidbody2D>();

        DeathSound = GetComponent<AudioSource>();

        if (abilities == null)
            abilities = AssetBundle.LoadFromFile(Path.Combine(
                Application.streamingAssetsPath, "AssetBundles/abilities"));

        if (hat == null)
            Debug.LogWarning("No hat set");

        PlayerCollisions = Sprite.GetComponents<CircleCollider2D>();

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

                        switch (special1Type)
                        {
                            case SpecialType.Hold:
                                special1 = OnSpecial(Input.GetButton("Special1"), special1Type, m_HashSpecial1Param);
                                break;

                            case SpecialType.Press:
                                special1 = OnSpecial(Input.GetButtonDown("Special1"), special1Type, m_HashSpecial1Param);
                                break;
                        }

                        switch (special2Type)
                        {
                            case SpecialType.Hold:
                                special2 = OnSpecial(Input.GetButton("Special2"), special2Type, m_HashSpecial2Param);
                                break;

                            case SpecialType.Press:
                                special2 = OnSpecial(Input.GetButtonDown("Special2"), special2Type, m_HashSpecial2Param);
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

        m_Ground.Move(moveDir, moveSpeed, m_Jump);

        m_Jump = false;
    }

    public void SetTrigger(bool active)
    {
        foreach(Collider2D collider in PlayerCollisions)
        {
            collider.isTrigger = active;
        }
    }

    public virtual bool OnSpecial(bool playerSpecial, SpecialType _SpecialType, int specialHash)
    {

        switch(_SpecialType)
        {
            case SpecialType.Hold:

                if (canSpecial)
                    m_Animator.SetBool(specialHash, playerSpecial);
                else
                    m_Animator.SetBool(specialHash, false);
                break;

            case SpecialType.Press:

                if (playerSpecial && canSpecial)
                    m_Animator.SetTrigger(specialHash);
                break;
        }

        return playerSpecial;
    }

    public override void OnDeath(Vector2 attackDirection)
    {
        this.NamedLog("Dead");

        PlayDeathEffect();

        moveDir = Vector2.zero;
        m_rigidbody.velocity = Vector2.zero;


        m_Animator.SetFloat("xSpeed", 0);
        m_Animator.SetFloat("ySpeed", 0);
        m_Animator.SetFloat("xSpeedAbs", 0);
        m_Animator.SetBool("Special1", false);
        m_Animator.SetBool("Special2", false);
        m_Animator.SetBool(m_HashDeadParam, true);
        m_Ground.SetCharacterCollisions(false);
        

        if (GameManager.gManager)
            GameManager.gManager.UnlockHat("Anor");
    }

    public virtual void FireProjectile()
    {
        FireNamedProjectile(rangeName);
    }

    public void FireNamedProjectile(string projName)
    {
        if (villagerState == VillagerState.PresentVillager)
        {
            GameObject gObject = abilities.LoadAsset<GameObject>(projName).Spawn(rangedSpawn.position);
            rangedAtk = gObject.GetComponent<VillagerAttack>();
            rangedAtk.damageMult = damageMult;

            float direction = rangedSpawn.position.x - m_rigidbody.transform.position.x;

            rangedAtk.Fire(new Vector2(Mathf.Sign(direction)
                , 0) * rangedProjectileStrength);
        }
    }

    public void SetDamageMult(int val)
    {
        damageMult = val;

        if (melee)
            classMeleeAttack.damageMult = val;
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

    private void PlayDeathEffect()
    {
        if (DeathSound)
        {
            DeathSound.Stop();
            DeathSound.Play();
        }
        else
        {
            Debug.LogWarning("No effect assigned, is this intended?");
        }
    }

    public override void Kill()
    {
        //Useful catch to prevent Animator from getting stuck on death;
        m_Animator.SetBool(m_HashMeleeParam, false);
        m_Animator.SetBool(m_HashRangedParam, false);
        m_Animator.SetBool(m_HashSpecial1Param, false);
        m_Animator.SetBool(m_HashSpecial2Param, false);


        base.Kill();
    }
}

