using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonEye : MonoBehaviour
{
    public AnimationCurve xFall, yFall;
    [Range(0, 5)]
    public float fallTime = 1.7f;
    [Range(-10, 0)]
    public float yKillZone = 0f;
    [Range(0f, 500f)]
    public float eyeReturnSpeed = 20f;
    public GameObject RockMinion;
    public SpriteRenderer m_SpriteRenderer;
    public MinionGibTracking m_MinionGibTracking;
    public Collider2D m_Collider;
    public Rigidbody2D m_Rigidbody;
    public SummonEyeState eyeState = SummonEyeState.Firing;

    RockMinion rockMinion;
    Transform parentTransform;
    Vector3 startPos, firePos;
    Quaternion startRot;
    float interPol;
    float timer = 0;
    int direction = 0;

    private void Awake()
    {
        parentTransform = transform.parent;
        startPos = transform.localPosition;
        startRot = transform.localRotation;
        direction = (int)Mathf.Sign(transform.localPosition.x);
    }

    private void OnEnable()
    {
        eyeState = SummonEyeState.Firing;
        timer = 0;
        firePos = transform.position;
    }

    void Update()
    {
        switch(eyeState)
        {
            case SummonEyeState.Firing:

                interPol = Mathf.InverseLerp(0, 1, timer / fallTime);

                transform.position = new Vector3(firePos.x + xFall.Evaluate(interPol) * direction,
                                                firePos.y + yFall.Evaluate(interPol),
                                                transform.position.z);

                transform.rotation = Quaternion.Euler(0, 0, interPol * -180 * direction);

                timer += Time.deltaTime;

                if(timer > fallTime || transform.position.y <= yKillZone)
                { 
                    var hit = Physics2D.Raycast(transform.position + Vector3.up * 10, Vector3.down, 10, LayerMask.GetMask("Ground"));

                    rockMinion = RockMinion.Spawn(null, hit.point).GetComponent<RockMinion>();
                    rockMinion.randomStartDir = false;
                    rockMinion.startDir = (Direction)direction;
                    rockMinion.pData.moveDir = new Vector2(direction, 0);
                    rockMinion.GetComponent<TimeObject>().oneLife = true;
                    rockMinion.GetComponent<TimeObject>().rewindOnly = true;
                    m_SpriteRenderer.enabled = false;

                    eyeState = SummonEyeState.Summoned;
                }

                break;

            case SummonEyeState.Summoned:

                if(!rockMinion.Alive)
                {
                    m_SpriteRenderer.enabled = true;
                    eyeState = SummonEyeState.Returning;
                    //TODO:Change this to match perfectly with Minions eye
                    transform.SetParent(parentTransform, true);
                    transform.position = rockMinion.Rigidbody.transform.position;
                }

                break;

            case SummonEyeState.Returning:

                transform.localPosition = Vector3.MoveTowards(transform.localPosition, startPos, eyeReturnSpeed * Time.deltaTime);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, startRot, eyeReturnSpeed * 100 * Time.deltaTime);

                if(Mathf.Approximately(transform.localPosition.x, startPos.x) && Mathf.Approximately(transform.localPosition.y, startPos.y))
                {
                    transform.localPosition = startPos;
                    transform.rotation = startRot;
                    eyeState = SummonEyeState.None;
                }

                break;
        }
    }

    public void Throw()
    {
        m_MinionGibTracking.Throw(Vector2.zero);
    }

    public void DisablePhysics()
    {
        Destroy(m_Collider);
        Destroy(m_Rigidbody);
    }

    public void KillMinion()
    {
        rockMinion.Kill();
    }
}