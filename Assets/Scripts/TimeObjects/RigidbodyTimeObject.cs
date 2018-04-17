using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles Rigidbody compoenets for TimeObject
/// </summary>
public class RigidbodyTimeObject : SpriteTimeObject
{

    protected Rigidbody2D m_Rigidbody2D;

    protected override void Awake()
    {
        base.Awake();

        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        if (m_Rigidbody2D)
        {
            OnStartReverse += OnRigidbody2DStartReverse;
            OnFinishReverse += OnRigidbody2DStartReverse;
        }
    }

    void OnRigidbody2DStartReverse()
    {
        m_Rigidbody2D.simulated = false;
    }

    void OnRigidbody2DFinishReverse()
    {
        m_Rigidbody2D.simulated = true;
    }
}
