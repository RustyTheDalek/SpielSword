using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteTimeObject : BaseTimeObject<FrameData>
{
    SpriteRenderer m_Sprite;
    VHSEffect vhsEffect;

    protected override void Start()
    {
        base.Start();

        m_Sprite = GetComponent<SpriteRenderer>();

        if (GetComponent<VHSEffect>())
        {
            vhsEffect = GetComponent<VHSEffect>();
        }
        else
        {
            vhsEffect = gameObject.AddComponent<VHSEffect>();
        }

        TimeObjectManager.spriteObjects.Add(this);
    }

    protected override void PlayFrame()
    {
            transform.position = frames[currentFrame].m_Position;
            transform.rotation = frames[currentFrame].m_Rotation;

            m_Sprite.color = frames[currentFrame].color;

            currentFrame += Game.GameScale;
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

    protected override void OnFinishReverse()
    {
        base.OnFinishReverse();

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
