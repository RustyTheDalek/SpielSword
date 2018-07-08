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

    static Dictionary<string, Material> _SpriteMaterials;

    protected static Dictionary<string, Material> SpriteMaterials
    {
        get
        {
            if (_SpriteMaterials == null)
            {
                _SpriteMaterials = new Dictionary<string, Material>();

                Object[] objs = Resources.LoadAll("Materials");

                foreach (object obj in objs)
                {
                    _SpriteMaterials.Add(((Material)obj).name, (Material)obj);
                }
            }

            return _SpriteMaterials;
        }
    }

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

        OnStartPlayback += OnSpriteStartPlayback;
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
        m_Sprite.material = SpriteMaterials["Sprite"];
        vhsEffect.enabled = false;

        //If Object did not exist at start we need to hide it
        if(startFrame != 0)
        {
            m_Sprite.enabled = false;
        }
    }

    protected void OnSpriteStartReverse()
    {
        m_Sprite.material = SpriteMaterials["VHSSprite"];
        vhsEffect.enabled = true;
    }

    protected void OnSpriteStartPlayback()
    {
        if (!m_Sprite.enabled && startFrame != 0)
            m_Sprite.enabled = true;
    }

    protected override void OnPast()
    {
        m_Sprite.material = SpriteMaterials["VHSSprite"];
        vhsEffect.enabled = true;

        base.OnPast();
    }

    private void OnDestroy()
    {
        OnPlayFrame -= PlaySpriteFrame;
        OnTrackFrame -= TrackSpriteFrame;

        OnStartReverse -= OnSpriteStartReverse;
        OnFinishReverse -= OnSpriteFinishReverse;
    }
}
