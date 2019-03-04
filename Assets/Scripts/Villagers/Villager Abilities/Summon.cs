using UnityEngine;

/// <summary>
/// Basic AI for Warlock/Shamans Summoned creature
/// </summary>
public class Summon : Character
{
    #region Public Variables

    public float lifeSpan = 5;

    public GroundCharacter2D m_GroundCharacter;

    [Tooltip("At what distance does the summonn react to a wall")]
    [Range(0f, 10f)]
    public float wallReactDistance = .4f;

    [Tooltip("At what distance does the summonn react to the floor")]
    [Range(0f, 10f)]
    public float floorReactDistance = 2;

    public Shaman creator;

    #endregion

    #region Protected Variables 

    protected readonly int  m_HashJumpParam     = Animator.StringToHash("Jump"),
                            m_HashAttackParam   = Animator.StringToHash("Attack"),
                            m_HashGroundParam = Animator.StringToHash("Ground");

    protected float lifeTimer;

    protected bool jump = false;

    #endregion

    protected override void Awake()
    {
        base.Awake();

    }

    private void OnEnable()
    {
        lifeTimer = lifeSpan;

        SceneLinkedSMB<Summon>.Initialise(m_Animator, this);
        SceneLinkedSMB<TimeObject>.Initialise(GetComponent<Animator>(), GetComponent<TimeObject>());
    }

    protected override void Update()
    {
        if (!Alive)
            return;

        m_Animator.SetBool(m_HashGroundParam, m_GroundCharacter.m_Grounded);

        if (lifeTimer > 0)
        {
            lifeTimer -= Time.deltaTime;
        }
        else
        {
            Kill();
        }
    }

    protected override void FixedUpdate()
    {
        if (!Alive)
            return;

        m_Animator.SetFloat("xSpeed", m_rigidbody.velocity.x);
        m_Animator.SetFloat("ySpeed", m_rigidbody.velocity.y);
        m_Animator.SetFloat("xSpeedAbs", Mathf.Abs(m_rigidbody.velocity.x));

        m_GroundCharacter.Move(moveDir, jump);

        jump = false;
    }

    #region Behaviour Functions


    public void CheckForWall()
    {
        Debug.DrawRay(  m_GroundCharacter.m_Front.position,
                        transform.right * moveDir * wallReactDistance,
                        Color.red);

        if (Physics2D.Raycast(  m_GroundCharacter.m_Front.position,
                                transform.right * moveDir, wallReactDistance,
                                LayerMask.GetMask("Ground")))
        {
            moveDir = -moveDir;

            Debug.Log("At a wall swapping", this);
        }

    }

    public void CheckForLedge()
    {
        if (m_GroundCharacter.m_Grounded && 
            !Physics2D.Raycast( m_GroundCharacter.m_Front.position, 
                                -transform.up, floorReactDistance, 
                                LayerMask.GetMask("Ground")))
        {

            jump = true;

            Debug.Log("At a Ledge, Jumping", this);
        }
    }

    #endregion

    public override void OnDeath(Vector2 attackDirection)
    {
        m_Animator.SetFloat("xSpeed", 0);
        m_Animator.SetFloat("ySpeed", 0);
        m_Animator.SetFloat("xSpeedAbs", 0);
        m_Animator.SetBool(m_HashDeadParam, true);

        if (creator)
            creator.summonActive = false;

        m_GroundCharacter.SetCharacterCollisions(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (LayerMask.LayerToName(collision.gameObject.layer))
        {
            case "Minion":
            case "Boss":

                Debug.Log("Saw Minion or boss, attacking", this);
                m_Animator.SetTrigger(m_HashAttackParam);
                moveDir = Vector2.zero;

                break;
            default :

                Debug.Log("Saw: " + collision.gameObject.name + " But I don't have a precedent for that", this);

                break;
        }
    }
}
