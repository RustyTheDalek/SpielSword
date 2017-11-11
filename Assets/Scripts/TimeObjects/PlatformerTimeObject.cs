using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tracking for Platformer characters such as Villagers and Minions
/// </summary>
public class PlatformerTimeObject : SpriteTimeObject
{
    protected PlatformerCharacter2D m_Platformer;
    PlatformerAnimData pAnimData;

    private PlatformerFrameData tempFrame;
    private List<PlatformerFrameData> pFrames = new List<PlatformerFrameData>();

    protected override void Start()
    {
        base.Start();

        m_Platformer = GetComponent<PlatformerCharacter2D>();
        
          
    }

    protected override void TrackFrame()
    {
        base.TrackFrame();

        tempFrame = new PlatformerFrameData()
        {
            move = 0,
            jump = false,
            meleeAttack = false,
            rangedAttack = false,
            dead = false,

        };
        
    }
}
