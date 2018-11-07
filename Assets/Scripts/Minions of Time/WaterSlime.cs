using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSlime : GroundMinion
{

    #region Public variables

    public GameObject trailVolume;

    #endregion
    #region Protected variables

    protected readonly int m_HashBiteAttack = Animator.StringToHash("Bite");

    protected float distanceTravelled = 0;
    protected float distanceThreshold = 1;

    protected Vector3 lastPos;

    protected Vector3 DeltaPos
    {
        get
        {
            return transform.position - lastPos;
        }
    }

    #endregion

    private void Start()
    {
        lastPos = transform.position;
    }

    protected override void Update()
    {
        base.Update();

        if(distanceTravelled >= 1)
        {
            distanceTravelled = 0;
            trailVolume.Spawn(transform.position);
        }

        distanceTravelled += DeltaPos.magnitude;

        lastPos = transform.position;
    }

    protected override void StartAttack()
    {
        base.StartAttack();

        m_Animator.SetTrigger(m_HashBiteAttack);
    }

    public override void Attack() {}
}
