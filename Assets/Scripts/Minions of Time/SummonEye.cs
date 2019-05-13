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
    public GameObject RockMinion;
    public SpriteRenderer m_SpriteRenderer;

    public SummonEyeState eyeState = SummonEyeState.Firing;

    RockMinion rockMinion;
    Vector2 startPos;
    float startRot;
    float interPol;
    float timer = 0;
    int direction = 0;

    void Start()
    {
        startPos = transform.position.XY();

        //change to local position when needed
        direction = (int)Mathf.Sign(transform.localPosition.x);
    }

    void Update()
    {
        switch(eyeState)
        {

            case SummonEyeState.Firing:

                interPol = Mathf.InverseLerp(0, 1, timer / fallTime);

                transform.position = new Vector3(startPos.x + xFall.Evaluate(interPol) * direction,
                                                startPos.y + yFall.Evaluate(interPol),
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
                    m_SpriteRenderer.enabled = false;

                    eyeState = SummonEyeState.Summoned;
                }

                break;

            case SummonEyeState.Summoned:

                if(!rockMinion.Alive)
                {
                    m_SpriteRenderer.enabled = true;
                    eyeState = SummonEyeState.Returning;
                }

                break;

            case SummonEyeState.Returning:

                transform.position = Vector3.MoveTowards(transform.position, startPos, .5f);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, startRot), .5f);

                if(Mathf.Approximately(transform.position.x, startPos.x) && Mathf.Approximately(transform.position.y, startPos.y))
                {
                    transform.position = startPos;
                    transform.rotation = Quaternion.Euler(0, 0, startRot);
                    eyeState = SummonEyeState.None;
                }

                break;
        }
    }
}
