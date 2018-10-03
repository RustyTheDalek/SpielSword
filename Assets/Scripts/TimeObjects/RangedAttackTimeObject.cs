using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackTimeObject : SpawnableSpriteTimeObject 
{
    Collider2D m_Collider;

    protected override void Awake()
    {
        base.Awake();

        m_Collider = GetComponent<Collider2D>();

        OnStartPlayback += EnableCOllider;
        OnFinishPlayback += DisableCollider;
        OnStartReverse += DisableCollider;
    }

    protected void EnableCOllider()
    {
        Debug.Log("Starting hit mode");
        m_Collider.enabled = true;
    }

    protected void DisableCollider()
    {
        Debug.Log("stopping hit mode");
        m_Collider.enabled = false;
    }

    private void OnDestroy()
    {
        OnStartPlayback -= EnableCOllider;
        OnFinishPlayback -= DisableCollider;
        OnStartReverse -= DisableCollider;
    }

}
