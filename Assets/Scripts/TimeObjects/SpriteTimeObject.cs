using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteTimeObject : BaseTimeObject<FrameData>
{
    SpriteRenderer m_Sprite;

    protected override void Start()
    {
        base.Start();

        m_Sprite = GetComponent<SpriteRenderer>();

        TimeObjectManager.spriteObjects.Add(this);
    }

    protected override void PlayFrame()
    {
            transform.position = frames[currentFrame].m_Position;
            transform.rotation = frames[currentFrame].m_Rotation;

            m_Sprite.color = frames[currentFrame].color;

            currentFrame += (int)Game.timeState;
    }

    protected override void TrackFrame()
    {
        tempFrame = new FrameData()
        {
            m_Position = gameObject.transform.position,
            m_Rotation = gameObject.transform.rotation,
            color = m_Sprite.color
        };
        frames.Add(tempFrame);
    }

    
}
