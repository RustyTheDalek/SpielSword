using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tracking for Platformer characters such as Villagers and Minions
/// Created by : Ian Jones - 11/11/17
/// Updated by : Ian Jones - 06/04/18
/// </summary>
public class PlatformerTimeObject : SpriteTimeObject
{
    #region Public Variables
    #endregion

    #region Protected Variables

    protected Character m_Character;
    protected PlatformerCharacter2D m_Platformer;

    #endregion

    #region Private Variables

    PlatformerAnimData pAnimData;

    PlatformerFrameData tempFrame;
    List<PlatformerFrameData> pFrames = new List<PlatformerFrameData>();

    #endregion


    protected override void Awake()
    {
        base.Awake();

        m_Character = GetComponent<Character>();
        m_Platformer = GetComponent<PlatformerCharacter2D>();

        OnTrackFrame += TrackPlatformerFrame;
        OnPlayFrame += PlayPlatformerFrame;
        OnFinishPlayback += OnFinishPlatformerPlayback;
    }

    protected override void OnPast()
    {
        tObjectState = TimeObjectState.PastFinished;
    }

    protected void Start()
    {
        //TODO:Remove this as it might not be totally nesecary 
        tObjectState = TimeObjectState.Present;
    }

    protected void TrackPlatformerFrame()
    {
        tempFrame = new PlatformerFrameData()
        {
            move = m_Character.xDir,
            //TODO:Stop using animdata if nessecary
            jump = (bool)m_Character.animData["Jump"],
            meleeAttack = (bool)m_Character.animData["MeleeAttack"],
            rangedAttack = (bool)m_Character.animData["RangedAttack"],
            dead = m_Character.Alive,

        };

        pFrames.Add(tempFrame);

    }

    protected void PlayPlatformerFrame()
    {
        if (Tools.WithinRange(currentFrame, pFrames))
        {
            m_Character.xDir = pFrames[currentFrame].move;
            m_Character.animData["Jump"] = pFrames[currentFrame].jump;
            m_Character.animData["MeleeAttack"] = pFrames[currentFrame].meleeAttack;
            m_Character.animData["RangedAttack"] = pFrames[currentFrame].rangedAttack;
            m_Character.animData["Dead"] = pFrames[currentFrame].dead;
        }

    }

    protected void OnFinishPlatformerPlayback()
    {
        if (finishFrame == 0)
        {
            Debug.Log("Finish Frame is 0, Platformer not died becoming present again");
            tObjectState = TimeObjectState.Present;

            m_Sprite.material = SpriteMaterials["Sprite"];
            vhsEffect.enabled = false;
        }
    }

    private void OnDestroy()
    {
        OnTrackFrame -= TrackPlatformerFrame;
        OnPlayFrame -= PlayPlatformerFrame;
        OnFinishPlayback -= OnFinishPlatformerPlayback;
    }

}
