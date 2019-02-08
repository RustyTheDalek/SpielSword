using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSlime : GroundMinion
{

    #region Public variables

    public GameObject trailVolume;

    #endregion

    #region Protected variables

    protected float distanceTravelled = 0;
    protected float distanceThreshold = 1;

    protected Vector2 lastPos;

    protected Vector2 DeltaPos
    {
        get
        {
            return m_rigidbody.position - lastPos;
        }
    }

    #endregion

    private void Start()
    {
        lastPos = m_rigidbody.position;
    }

    protected override void Update()
    {
        base.Update();

        if(distanceTravelled >= distanceThreshold)
        {
            distanceTravelled = 0;
            GameObject gO = trailVolume.Spawn(m_rigidbody.transform.position, 
                m_GroundCharacter.m_Character.rotation);
        }

        distanceTravelled += DeltaPos.magnitude;

        lastPos = m_rigidbody.position;
    }
}
