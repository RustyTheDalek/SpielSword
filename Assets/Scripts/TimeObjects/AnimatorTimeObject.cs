using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tracking for Platformer characters such as Villagers and Minions
/// Created by : Ian Jones - 11/11/17
/// Updated by : Ian Jones - 06/04/18
/// </summary>
public class AnimatorTimeObject : RigidbodyTimeObject
{
    #region Public Variables

    #endregion

    #region Protected Variables

    protected Character m_Character;

    protected AnimatorFrameData tempFrame;
    protected List<AnimatorFrameData> pFrames = new List<AnimatorFrameData>();

    public Dictionary<string, Sprite> sprites;

    protected PlatformerAnimData pAnimData;

    protected Animator m_Anim;

    #endregion

    #region Private Variables

    #endregion

    protected override void Awake()
    {
        base.Awake();

        sprites = new Dictionary<string, Sprite>();

        m_Character = GetComponent<Character>();
        m_Anim = GetComponent<Animator>();

        OnTrackFrame += TrackSpriteSheetFrame;
        OnPlayFrame += PlaySpriteSheetFrame;
        OnFinishPlayback += OnFinishSpriteSheetPlayback;
        OnStartReverse += OnSpriteSheetStartReverse;
    }

    //protected override void OnPast()
    //{
    //    tObjectState = TimeObjectState.PastFinished;
    //}

    protected virtual void Start()
    {
        //TODO:Remove this as it might not be totally nesecary 
        tObjectState = TimeObjectState.Present;
    }

    protected void TrackSpriteSheetFrame()
    {
        //Adds new sprite to List
        if(!sprites.ContainsKey(m_Sprite.sprite.name))
        {
            sprites.Add(m_Sprite.sprite.name, m_Sprite.sprite);
        }

        tempFrame = new AnimatorFrameData()
        {
            sprite = m_Sprite.sprite.name
        };

        pFrames.Add(tempFrame);

    }

    protected void PlaySpriteSheetFrame()
    {
        if (pFrames.WithinRange(currentFrame))
        {
            m_Sprite.sprite = sprites[pFrames[(int)currentFrame].sprite];
        }
    }

    protected void OnFinishSpriteSheetPlayback()
    {
        if (finishFrame == 0)
        {
            Debug.Log("Finish Frame is 0, Platformer not died becoming present again");
            tObjectState = TimeObjectState.Present;
        }
    }

    protected void OnSpriteSheetStartReverse()
    {
        m_Anim.enabled = false;
    }

    private void OnDestroy()
    {
        OnTrackFrame -= TrackSpriteSheetFrame;
        OnPlayFrame -= PlaySpriteSheetFrame;
        OnFinishPlayback -= OnFinishSpriteSheetPlayback;
        OnStartPlayback -= OnSpriteSheetStartPlayback;
        OnStartReverse -= OnSpriteSheetStartReverse;
    }

}
