using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tracks and plays back Sprite based information
/// Created by : Ian Jones - 22/04/17
/// Updated by : Ian Jones - 06/04/18
/// </summary>
public class SpriteTimeObject : TimeObject
{
    protected SpriteRenderer m_Sprite;

    protected SpriteFrameData tempSFrame;
    protected List<SpriteFrameData> sFrames = new List<SpriteFrameData>();

    protected override void Awake()
    {
        base.Awake();
        m_Sprite = GetComponent<SpriteRenderer>();

        OnPlayFrame += PlaySpriteFrame;
        OnTrackFrame += TrackSpriteFrame;

        OnStartReverse += OnSpriteStartReverse;
        OnFinishReverse += OnSpriteFinishReverse;

        OnStartPlayback += OnSpriteStartPlayback;
    }

    protected void PlaySpriteFrame()
    {
        if (sFrames.WithinRange(currentFrame))
        {
            m_Sprite.color = new Color( sFrames[(int)currentFrame].color.r,
                                        sFrames[(int)currentFrame].color.g,
                                        sFrames[(int)currentFrame].color.b,
                                        .75f);
        }
    }

    protected void TrackSpriteFrame()
    {
        tempSFrame = new SpriteFrameData()
        {
            color = m_Sprite.color,
        };

        sFrames.Add(tempSFrame);
    }

    protected void OnSpriteFinishReverse()
    {

        //If Object did not exist at start we need to hide it
        if(startFrame != 0)
        {
            m_Sprite.enabled = false;
        }
    }

    protected void OnSpriteStartReverse()
    {
    }

    protected void OnSpriteStartPlayback()
    {
        if (!m_Sprite.enabled && startFrame != 0)
            m_Sprite.enabled = true;
    }

    private void OnDestroy()
    {
        OnPlayFrame -= PlaySpriteFrame;
        OnTrackFrame -= TrackSpriteFrame;

        OnStartReverse -= OnSpriteStartReverse;
        OnFinishReverse -= OnSpriteFinishReverse;
    }
}
