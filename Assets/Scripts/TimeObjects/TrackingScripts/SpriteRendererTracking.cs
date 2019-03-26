using System.Collections.Generic;
using UnityEngine;

public class SpriteRendererTracking : ObjectTrackBase
{

    protected SpriteRenderer m_Sprite;
    
    protected SpriteFrameData tempSFrame;
    protected List<SpriteFrameData> sFrames = new List<SpriteFrameData>();

    protected Dictionary<string, Sprite> sprites;

    public void Awake()
    {
        sprites = new Dictionary<string, Sprite>();

        m_Sprite = GetComponent<SpriteRenderer>();
    }

    public override void ResetToPresent()
    {
        if (objectTrackType.HasFlag(ObjectTrackType.FrameTracking))
        {
            if (sFrames != null && sFrames.Count > 1)
            {
                m_Sprite.enabled = sFrames[0].enabled;
                sFrames.Clear();
            }
        }
    }

    override public void TrackFrame()
    {
        //Adds new sprite to List
        if (!sprites.ContainsKey(m_Sprite.sprite.name))
        {
            sprites.Add(m_Sprite.sprite.name, m_Sprite.sprite);
        }

        tempSFrame = new SpriteFrameData()
        {
            enabled = m_Sprite.enabled,
            sprite = m_Sprite.sprite.name,
            color = m_Sprite.color,
            flipX = m_Sprite.flipX,
            flipY = m_Sprite.flipY,
            maskInteraction = m_Sprite.maskInteraction
        };

        sFrames.Add(tempSFrame);
    }

    override public void PlayFrame(int currentFrame)
    {
        if (sFrames.WithinRange(currentFrame))
        {
            m_Sprite.enabled = sFrames[currentFrame].enabled;
            m_Sprite.sprite = sprites[sFrames[currentFrame].sprite];

            m_Sprite.color = new Color(sFrames[currentFrame].color.r,
                                        sFrames[currentFrame].color.g,
                                        sFrames[currentFrame].color.b,
                                        sFrames[currentFrame].color.a);

            m_Sprite.flipX = sFrames[currentFrame].flipX;
            m_Sprite.flipY = sFrames[currentFrame].flipY;

            m_Sprite.maskInteraction = sFrames[currentFrame].maskInteraction;
        }
    }

    public override void OnStartReverse()
    {
        m_Sprite.enabled = sFrames[sFrames.Count -1].enabled;
    }

    public override void OnFinishReverse(int startFrame)
    {
        //If Object did not exist at start we need to hide it
        if (startFrame != 0)
        {
            m_Sprite.enabled = false;
        }
    }

    public override void OnStartPlayback(int startFrame)
    {
        if (!m_Sprite.enabled && startFrame != 0)
            m_Sprite.enabled = sFrames[sFrames.Count - 1].enabled;
    }

    public override void OnFinishPlayback()
    {
        m_Sprite.enabled = sFrames[sFrames.Count - 1].enabled;
    }

}
