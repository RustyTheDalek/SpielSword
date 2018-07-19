using UnityEngine;

[RequireComponent(typeof(FlyingCharacter2D))]
public class FlightMinions : Minion
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

    protected readonly int m_HashAttackParam = Animator.StringToHash("Attack");

    #endregion

    #region Private Variables

    private Vector3 orginalPosition;

    private FlyingCharacter2D m_Flying;

    #endregion

    protected override void Awake()
    {
        base.Awake();

        m_Flying = GetComponent<FlyingCharacter2D>();
    }

    protected override void Start()
    {
        base.Start();

        orginalPosition = transform.position;
        //Move the Minion by the distance we want him to Orbit
        transform.position = transform.position + Vector3.right * orbitDistance;
    }

    protected override void Migrate()
    {
        //TODO: Make this appropriate in future
        if(transform.position.x < killZone)
        {
            gameObject.SetActive(false);
            return;
        }

        moveDir = Vector2.left;

        m_Flying.Move(moveDir);
    }

    protected override void Patrol()
    {
        //This code finds vector towards the it's origin then finds the 
        //perpendicular vector with a clockwise bias so that it moves
        //that direction
        Vector2 toOriginal = transform.position - orginalPosition;
        Vector2 forwardPoint = new Vector2(-toOriginal.y, toOriginal.x) /
            Mathf.Sqrt(toOriginal.x.Sqd() + toOriginal.y.Sqd());
        //We use the direction to the Origin and the desired distance to 
        //adjust the move direction to keep them the righht distance away
        moveDir = forwardPoint.normalized + (toOriginal.normalized *
            (orbitDistance - toOriginal.magnitude) * distanceAmp);

        m_Flying.Move(moveDir);

    }

    //TODO: Look into making this so that the attack carries on until it finishs
    //Or Hits player
    protected override void Attack()
    {
        moveDir = transform.position.PointTo(closestVillager.transform.position);

        m_Animator.SetBool(m_HashAttackParam, true);

        m_Flying.Move(moveDir, 15);

        //We don't really want to start resting straight away but this will work for now
        StartRest();
    }

    protected override void MoveToClosest()
    {
        moveDir = transform.position.PointTo(closestVillager.transform.position);
        m_Flying.Move(moveDir);
    }

    protected override void OnNoMoreTargets()
    {
        base.OnNoMoreTargets();

        m_Animator.SetBool(m_HashAttackParam, false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If hits the player they're attacking
        if(collision.gameObject == closestVillager)
        {
            m_Animator.SetBool(m_HashAttackParam, false);
            m_rigidbody.velocity = Vector2.zero;
        }
        else if(collision.gameObject.layer == layerGround)
        {
            //TODO:Set Stuck Bool so that Flying Minion get stuck
        }
    }
}

