using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableSprite : MonoBehaviour
{
    public SpriteRenderer m_Sprite;

    public List<Sprite> m_DamageSprites;

    public void Awake()
    {
        m_Sprite = GetComponent<SpriteRenderer>();
    }

    public void SetDamageSprite(int counter)
    {
        if (m_DamageSprites.WithinRange(counter))
            m_Sprite.sprite = m_DamageSprites[counter];
        else
            m_Sprite.sprite = m_DamageSprites[0];

    }
}
