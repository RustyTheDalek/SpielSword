using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubstitutionTotem : MonoBehaviour
{

    #region Public Variables

    public SpriteRenderer m_Sprite;
    public ParticleSystem teleportFX;

    public float m_MaxMoveDelta;

    //If the Totem is moving it is not active move time acts as a recharge
    public bool Active
    {
        get
        {
            return !moving;
        }
    }

    #endregion

    #region Private Variables

    Vector3 targetPos;

    bool moving = false;

    float   travelledDistance,
            totalDistance;

    float _H, _S, _V;

    #endregion

    public void Substitute(Vector3 _TargetPos)
    {
        teleportFX.Play();
        moving = true;
        targetPos = _TargetPos;
        travelledDistance = 0;
        totalDistance = Vector3.Distance(transform.position, targetPos);
    }

    private void Update()
    {
        if(moving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 
                m_MaxMoveDelta);

            travelledDistance += m_MaxMoveDelta;

            Color.RGBToHSV(m_Sprite.color, out _H, out _S, out _V);

            _V = Mathf.InverseLerp(0, totalDistance, travelledDistance);

            m_Sprite.color = Color.HSVToRGB(_H, _S, _V);

            if(Vector3.Distance(transform.position, targetPos) < 1)
            {
                moving = false;
            }
        }
    }

}
