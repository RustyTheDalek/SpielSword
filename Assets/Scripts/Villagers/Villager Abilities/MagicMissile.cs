using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMissile : VillagerAttack
{
    public float moveForce = 10f;

    public float m_MaxSpeed = 5f;

    public float risingTimeout = 5f;
    public float divergentTimeout = .75f;

    public List<Transform> targetsInSight = new List<Transform>(5);
    public Transform closestTarget;

    float closestTargetDist;

    float timer;

    float   targetDivergentAngle,
            currentDivergentAngle;

    public MagicMissileState m_MMState = MagicMissileState.Rising;

    public bool AnyTargets
    {
        get
        {
            return targetsInSight.Count > 0;
        }
    }

    Vector2 forceDirection, divergentForce;

    protected void Start()
    {
        timer = risingTimeout;
    }

    protected override void Update()
    {
        base.Update();

        switch(m_MMState)
        {
            case MagicMissileState.Rising:

                if(timer > 0)
                    timer -= Time.deltaTime;
                else
                {
                    m_MMState = MagicMissileState.Tracking;
                    timer = divergentTimeout;
                }
                    
                break;

            case MagicMissileState.Tracking:

                FindClosest();

                if (timer > 0)
                    timer -= Time.deltaTime;
                else
                {
                    m_MMState = MagicMissileState.ToDiversionAngle;

                    currentDivergentAngle = 0;
                    targetDivergentAngle = Random.Range(20, 40);

                    if (Random.Range(1, 3) == 1)
                        targetDivergentAngle *= -1;

                }

                break;

            case MagicMissileState.ToDiversionAngle:

                FindClosest();

                divergentForce = forceDirection.Rotate(currentDivergentAngle);

                if (currentDivergentAngle != targetDivergentAngle)
                    currentDivergentAngle += Mathf.Sign(targetDivergentAngle);
                else
                    m_MMState = MagicMissileState.ToOriginAngle;

                break;

            case MagicMissileState.ToOriginAngle:

                FindClosest();

                divergentForce = forceDirection.Rotate(currentDivergentAngle);

                if(currentDivergentAngle != 0)
                    currentDivergentAngle -= Mathf.Sign(targetDivergentAngle);
                else
                {
                    m_MMState = MagicMissileState.Tracking;
                    timer = divergentTimeout;
                }

                break;
        }

        

    }

    protected void FixedUpdate()
    {
        m_Rigidbody.velocity = Vector2.zero;

        if (AnyTargets)
        {
            switch(m_MMState)
            {
                case MagicMissileState.ToDiversionAngle:
                case MagicMissileState.ToOriginAngle:

                    Debug.DrawRay(transform.position, divergentForce, Color.red);
                    m_Rigidbody.AddForce(divergentForce * moveForce);

                    break;

                case MagicMissileState.Tracking:

                    Debug.DrawRay(transform.position, forceDirection, Color.green);
                    m_Rigidbody.AddForce(forceDirection * moveForce);
                    break;

                case MagicMissileState.Rising:

                    Debug.DrawRay(transform.position, Vector2.up, Color.yellow);
                    m_Rigidbody.AddForce(Vector2.up * moveForce);
                    break;
            }
        }
        else
        {
            Debug.DrawRay(transform.position, Vector2.up, Color.yellow);
            m_Rigidbody.AddForce(Vector2.up * moveForce);
        }

        m_Rigidbody.velocity = new Vector2(Mathf.Clamp(
            m_Rigidbody.velocity.x, -m_MaxSpeed, m_MaxSpeed),
            m_Rigidbody.velocity.y);
    }

    protected void FindClosest()
    {
        //Reset closest to max
        closestTargetDist = Mathf.Infinity;
        closestTarget = null;
        //Find the closest
        foreach (Transform target in targetsInSight)
        {
            float dist = (target.transform.position.XY() - m_Rigidbody.position).magnitude;

            if (dist < closestTargetDist)
            {
                if(target.GetComponent<Character>())
                {
                    if(!target.GetComponent<Character>().Alive)
                    {
                        break;
                    }
                }
                closestTarget = target;
                closestTargetDist = dist;
            }
        }

        if (closestTarget)
            forceDirection = transform.position.PointTo(closestTarget.position);
        else
            forceDirection = Vector2.up;
    }

    protected void OnTriggerEnter2D(Collider2D coll)
    {

        Transform collTrans;

        switch (LayerMask.LayerToName(coll.gameObject.layer))
        {
            case "Minion":
            case "Boss":

                collTrans = coll.transform;
                LivingObject target = coll.gameObject.GetComponentInParent<LivingObject>();

                if (target.Alive && 
                    !targetsInSight.Contains(collTrans) && targetsInSight.Count < 5)
                {
                    targetsInSight.Add(collTrans);
                }

                break;
        }
    }

    protected void OnTriggerExit2D(Collider2D coll)
    {
        Transform collTrans;

        switch (LayerMask.LayerToName(coll.gameObject.layer))
        {
            case "Minion":

                collTrans = coll.transform;

                if (targetsInSight.Contains(collTrans))
                {
                    targetsInSight.Remove(collTrans);
                }

                break;

            case "Boss":

                collTrans = coll.transform;

                if (targetsInSight.Contains(collTrans))
                {
                    targetsInSight.Remove(collTrans);
                }

                break;
        }
    }
}
