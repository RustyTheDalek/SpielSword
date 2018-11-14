using UnityEngine;

public class Camera2DFollow : MonoBehaviour
{
    public Transform target;
    public float damping = .25f;
    public float lookAheadFactor = 10;
    public float lookAheadReturnSpeed = 20;
    public float lookAheadMoveThreshold = 0.1f;
    public Vector3 offset;

    public Vector2 minPos, maxPos;

    private float m_OffsetZ;
    private Vector3 m_LastTargetPosition;
    private Vector3 m_CurrentVelocity;
    private Vector3 m_LookAheadPos;

    // Use this for initialization
    private void Start()
    {
        if(minPos == Vector2.zero)
        {
            minPos = new Vector2(-Mathf.Infinity, -Mathf.Infinity);
        }

        if (maxPos == Vector2.zero)
        {
            maxPos = new Vector2(Mathf.Infinity, Mathf.Infinity);
        }

        if (target)
        {
            m_LastTargetPosition = target.position;
            m_OffsetZ = (transform.position - target.position).z;
            transform.parent = null;
        }
    }


    // Update is called once per frame
    private void LateUpdate()
    {
        if (target)
        {
            // only update lookahead pos if accelerating or changed direction
            float xMoveDelta = (target.position - m_LastTargetPosition).x;

            bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

            if (updateLookAheadTarget)
            {
                m_LookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
            }
            else
            {
                m_LookAheadPos = Vector3.MoveTowards(
                    m_LookAheadPos, 
                    Vector3.zero, 
                    Time.deltaTime * lookAheadReturnSpeed);
            }

            Vector3 aheadTargetPos = target.position + m_LookAheadPos + Vector3.forward * m_OffsetZ + offset;
            Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);

            transform.position = new Vector3(Mathf.Clamp(newPos.x, minPos.x, maxPos.x),
                                             Mathf.Clamp(newPos.y, minPos.y, maxPos.y),
                                             -20);

            m_LastTargetPosition = target.position;
        }
    }
}
