using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteTimeObject : TimeObject
{
    protected SpriteRenderer m_Sprite;
    protected VHSEffect vhsEffect;

    protected SpriteFrameData tempSFrame;
    protected List<SpriteFrameData> sFrames = new List<SpriteFrameData>();

    protected void Awake()
    {
        m_Sprite = GetComponent<SpriteRenderer>();

        if (GetComponent<VHSEffect>())
        {
            vhsEffect = GetComponent<VHSEffect>();
        }
        else
        {
            vhsEffect = gameObject.AddComponent<VHSEffect>();
        }
    }

    protected override void Start()
    {
        base.Start();

        TimeObjectManager.spriteObjects.Add(this);
    }

    //This constructor is used when we still want the base Start function but the child 
    //of this object has a different list that's managed by TimeObjectManager
    protected void Start(bool newList)
    {
        base.Start();

        if (!newList)
        {
            TimeObjectManager.spriteObjects.Add(this);
        }
    }

    protected override void PlayFrame()
    {
        base.PlayFrame();

        if (Tools.WithinRange(currentFrame, sFrames))
        {
            m_Sprite.color = sFrames[currentFrame].color;
        }
    }

    protected override void TrackFrame()
    {
        base.TrackFrame();

        tempSFrame = new SpriteFrameData()
        {
            color = m_Sprite.color,
        };

        sFrames.Add(tempSFrame);
    }

    protected override void OnFinishReverse()
    {
        base.OnFinishReverse();

        currentFrame = 0;
        m_Sprite.material = AssetManager.SpriteMaterials[0];
        vhsEffect.enabled = false;
    }

    protected override void OnStartReverse()
    {
        base.OnStartReverse();

        m_Sprite.material = AssetManager.SpriteMaterials[1];
        vhsEffect.enabled = true;
    }

    protected override void OnPast()
    {
        m_Sprite.material = AssetManager.SpriteMaterials[1];
        vhsEffect.enabled = true;
        base.OnPast();
    }
}
