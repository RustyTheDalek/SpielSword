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

    protected override void Start()
    {
        base.Start(true);

        m_Character = GetComponent<Character>();
        m_Platformer = GetComponent<PlatformerCharacter2D>();

        TimeObjectManager.platformers.Add(this);

        tObjectState = TimeObjectState.Present;
    }

    protected override void TrackFrame()
    {
        base.TrackFrame();

        tempFrame = new PlatformerFrameData()
        {
            move = m_Character.xDir,
            //TODO:Stop using animdata if nessecary
            jump = m_Character.animData.jump,
            meleeAttack = m_Character.animData.jump,
            rangedAttack = m_Character.animData.rangedAttack,
            dead = m_Character.Alive,

        };

        pFrames.Add(tempFrame);
        
    }

    protected override void PlayFrame()
    {
        base.PlayFrame();

        if (Tools.WithinRange(currentFrame, pFrames))
        {
            m_Character.xDir = pFrames[currentFrame].move;
            m_Character.animData.jump = pFrames[currentFrame].jump;
            m_Character.animData.meleeAttack = pFrames[currentFrame].meleeAttack;
            m_Character.animData.rangedAttack = pFrames[currentFrame].rangedAttack;
            m_Character.animData.dead = pFrames[currentFrame].dead; 
        }

    }

    protected override void OnStartPlayback()
    {
        Debug.Log("Finish Frame is 0, Platformer not died becoming present again");
        pFrames.Clear();
        sFrames.Clear();
        bFrames.Clear();
        finishFrame = 0;
        tObjectState = TimeObjectState.Present;

        m_Sprite.material = AssetManager.SpriteMaterials[0];
        vhsEffect.enabled = false;
    }
}
