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
    protected VHSEffect vhsEffect;

    protected SpriteFrameData tempSFrame;
    protected List<SpriteFrameData> sFrames = new List<SpriteFrameData>();

    protected override void Awake()
    {
        base.Awake();
        m_Sprite = GetComponent<SpriteRenderer>();

        if (GetComponent<VHSEffect>())
        {
            vhsEffect = GetComponent<VHSEffect>();
        }
        else
        {
            vhsEffect = gameObject.AddComponent<VHSEffect>();
        }

        OnPlayFrame += PlaySpriteFrame;
        OnTrackFrame += TrackSpriteFrame;

        OnStartReverse += OnSpriteStartReverse;
        OnFinishReverse += OnSpriteFinishReverse;
    }

    protected void PlaySpriteFrame()
    {
        if (Tools.WithinRange(currentFrame, sFrames))
        {
            m_Sprite.color = new Color( sFrames[currentFrame].color.r,
                                        sFrames[currentFrame].color.g,
                                        sFrames[currentFrame].color.b,
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
        m_Sprite.material = AssetManager.SpriteMaterials["Sprite"];
        vhsEffect.enabled = false;
    }

    protected void OnSpriteStartReverse()
    {
        m_Sprite.material = AssetManager.SpriteMaterials["VHSSprite"];
        vhsEffect.enabled = true;
    }

    protected override void OnPast()
    {
        m_Sprite.material = AssetManager.SpriteMaterials["VHSSprite"];
        vhsEffect.enabled = true;

        base.OnPast();
    }
}
