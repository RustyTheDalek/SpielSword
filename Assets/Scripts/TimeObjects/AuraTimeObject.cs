using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraTimeObject : SpawnableSpriteTimeObject
{

    Aura m_Aura;

    Collider2D m_Collider;

    protected override void Awake()
    {
        base.Awake();

        m_Aura = GetComponent<Aura>();
        m_Collider = GetComponent<Collider2D>();

        OnPlayFrame -= PlaySpriteFrame;
        OnTrackFrame -= TrackSpriteFrame;
    }

    protected override void Start()
    { 
        base.Start();

        OnPlayFrame += PlayAuraFrame;

        OnStartPlayback += DecreaseStrength;
    }

    protected override void Update()
    {
        base.Update();

        m_Sprite.enabled    = spawnableActive;
        m_Collider.enabled  = spawnableActive;
    }

    protected void PlayAuraFrame()
    {
        if (sSFrames.WithinRange(currentFrame))
        {
            if (m_Collider)
                m_Collider.enabled = sSFrames[(int)currentFrame].active;

            if (m_Rigidbody2D)
                m_Rigidbody2D.simulated = sSFrames[(int)currentFrame].active;

            spawnableActive = sSFrames[(int)currentFrame].active;
        }
    }

    public void DecreaseStrength()
    {
        if (m_Aura.health > 0)
        {
            m_Aura.health--;

            Color col = m_Sprite.color;
            m_Sprite.color = new Color(col.r, col.g, col.b, m_Aura.health / 4);
        }
        else
        {
            OnStartPlayback -= DecreaseStrength;
        }
    }
}
