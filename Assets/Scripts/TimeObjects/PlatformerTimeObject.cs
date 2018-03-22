using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tracking for Platformer characters such as Villagers and Minions
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
        OnStartPlayback += OnStartPlatformerPlayback;
    }

    protected void Start()
    {
        //base.Start(true);

        //TimeObjectManager.platformers.Add(this);

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
            m_Character.animData["Jump"]= pFrames[currentFrame].jump;
            m_Character.animData["MeleeAttack"]= pFrames[currentFrame].meleeAttack;
            m_Character.animData["RangedAttack"]= pFrames[currentFrame].rangedAttack;
            m_Character.animData["Dead"] = pFrames[currentFrame].dead; 
        }

    }

    protected void OnStartPlatformerPlayback()
    {
        if (finishFrame == 0)
        {
            Debug.Log("Finish Frame is 0, Platformer not died becoming present again");
            pFrames.Clear();
            sFrames.Clear();
            bFrames.Clear();
            finishFrame = 0;
            tObjectState = TimeObjectState.Present;

            m_Sprite.material = AssetManager.SpriteMaterials["Sprite"];
            //vhsEffect.enabled = false;
        }
    }
}
