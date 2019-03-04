
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraTracking : ObjectTrackBase
{

    Aura m_Aura;
    SpriteRenderer m_Sprite;

    protected void Awake()
    {
        m_Aura = GetComponent<Aura>();

        m_Sprite = GetComponent<SpriteRenderer>();
    }

    public override void OnStartPlayback(int startFrame)
    {
        if (m_Aura.health > 0)
        {
            m_Aura.health--;

            Color col = m_Sprite.color;
            m_Sprite.color = new Color(col.r, col.g, col.b, m_Aura.health / 4);
        }
    }
}
