
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlatformerMotion2D))]
public class FlightMinion : Minion
{
    #region Public Variables

    /// <summary>
    /// Distance Minion orbits from origin
    /// </summary>
    public int orbitDistance = 5;

    /// <summary>
    /// The strength of the force that maintains the distance
    /// </summary>
    public float distanceAmp = 2;

    /// <summary>
    /// Kill object once it has passed over the threshold for the level
    /// </summary>
    public float killZone;

    #endregion

    #region Protected Variables

    protected MinionGibTracking[] minionParts;

    protected readonly int m_HashStuckParam = Animator.StringToHash("Stuck");

    [Header("Eye setup")]

    [SerializeField] protected Sprite eyeSpriteDefault;
    [SerializeField] protected Sprite eyeSpriteAngry;

    [SerializeField] protected SpriteRenderer eyeRenderer;

    #endregion

    #region Private Variables

    private Vector3 orginalPosition;

    #endregion

    protected override void Awake()
    {
        base.Awake();

        orginalPosition = transform.position;
        //Move the Minion by the distance we want him to Orbit
        transform.position = transform.position + Vector3.right * orbitDistance;

        minionParts = GetComponentsInChildren<MinionGibTracking>();
    }

    public override void OnEnable()
    {
        base.OnEnable();

        pData.moveDir = Vector2.zero;
        transform.position = orginalPosition + Vector3.right * orbitDistance;

        SceneLinkedSMB<FlightMinion>.Initialise(m_Animator, this);
    }

    protected override void FixedUpdate()
    {
        m_Animator.SetFloat("xSpeed", pData.moveDir.x * 7);
        m_Animator.SetFloat("ySpeed", pData.moveDir.y * 7);
        m_Animator.SetFloat("xSpeedAbs", Mathf.Abs(m_rigidbody.velocity.x));

        m_Character.Move(pData);
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(orginalPosition, .1f);
    }

    public override void Migrate()
    {
        //TODO: Make this appropriate in future
        if(transform.position.x < killZone)
        {
            gameObject.SetActive(false);
            return;
        }

        //TODO: improve this, sort of works but it takes them a while to get back
        pData.moveDir = new Vector2(-1, transform.position.PointTo(orginalPosition).y);
    }

    public override void Patrol()
    {
        //This code finds vector towards it's origin then finds the 
        //perpendicular vector with a clockwise bias so that it moves
        //in that direction
        Vector2 toOriginal = transform.position - orginalPosition;
        Vector2 forwardPoint = new Vector2(-toOriginal.y, toOriginal.x) /
            Mathf.Sqrt(toOriginal.x.Sqd() + toOriginal.y.Sqd());
        //We use the direction to the Origin and the desired distance to 
        //adjust the move direction to keep them the right distance away
        pData.moveDir = forwardPoint.normalized + (toOriginal.normalized *
            (orbitDistance - toOriginal.magnitude) * distanceAmp);

        //moveDir = moveDir.normalized;
    }

    protected override void OnFoundTarget()
    {
        base.OnFoundTarget();
        eyeRenderer.sprite = eyeSpriteAngry;
    }

    protected override void OnNoMoreTargets()
    {
        base.OnNoMoreTargets();
        eyeRenderer.sprite = eyeSpriteDefault;
    }

    protected void OnStuck()
    {
        //We want to keep, the direction the same in this case so we grab the current X &
        //Y Speed and hold them separately
        m_Animator.SetFloat("x", m_Animator.GetFloat("xSpeed"));
        m_Animator.SetFloat("y", m_Animator.GetFloat("ySpeed"));
        m_Animator.SetTrigger(m_HashStuckParam);
        m_rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        pData.maxVelocity = patrolSpeed;
    }

    public void OnUnstuck()
    {
        this.NamedLog("I'm unstuck now");
        m_rigidbody.constraints = RigidbodyConstraints2D.None;
        state = startingState;
        pData.moveDir = transform.position.PointTo(orginalPosition);
    }

    protected override void OnDeath(Vector2 attackDirection)
    {
        StopAllCoroutines();

        pData.moveDir = Vector2.zero;
        m_rigidbody.velocity = Vector2.zero;
        m_rigidbody.simulated = false;

        m_Animator.SetFloat("xSpeed", 0);
        m_Animator.SetFloat("ySpeed", 0);
        m_Animator.SetFloat("xSpeedAbs", 0);
        m_Animator.SetBool(m_HashDeadParam, true);
        m_Animator.enabled = false;

        m_rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

        //TODO:Re-add this by the separating from TimeObject code
        foreach (MinionGibTracking part in minionParts)
        {
            part.Throw(attackDirection);
        }

        StartCoroutine(DelayToDeath());
    }

    public IEnumerator DelayToDeath()
    {
        yield return new WaitForSeconds(5f);

        GetComponent<TimeObject>().tObjectState = TimeObjectState.PresentDead;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        this.NamedLog("Hit a thing");

        if (state == MinionState.Attacking)
        {
            switch (LayerMask.LayerToName(collision.gameObject.layer))
            {
                case "Ground":

                    this.NamedLog("Ooof, Hit a Wall");

                    OnStuck();

                    break;
            }
        }
    }
}

