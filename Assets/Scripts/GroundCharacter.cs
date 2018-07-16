using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCharacter : Character
{
    protected bool m_Jump;

    protected GroundCharacter2D m_Ground;

    protected override void Awake()
    {
        base.Awake();

        m_Ground = GetComponent<GroundCharacter2D>();
    }

    protected override void CreateHashtable()
    {
        base.CreateHashtable();

        animData.Add("Jump", false);
    }
}