using System.Collections.Generic;
using UnityEngine;

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

    //protected override void Start()
    //{
    //    base.Start();

    //    TimeObjectManager.spriteObjects.Add(this);
    //}

    //This constructor is used when we still want the base Start function but the child 
    //of this object has a different list that's managed by TimeObjectManager
    //protected void Start(bool newList)
    //{
    //    base.Start();

    //    if (!newList)
    //    {
    //        TimeObjectManager.spriteObjects.Add(this);
    //    }
    //}

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
