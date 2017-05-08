using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object that can be affected by time in both directions
/// </summary>
public class TimeObject : BaseTimeObject<FrameData>
{
    protected override void PlayFrame()
    {
        transform.position = frames[currentFrame].m_Position;
        transform.rotation = frames[currentFrame].m_Rotation;

        gameObject.SetActive(frames[currentFrame].enabled);

        currentFrame += (int)Game.timeState;
    }

    protected override void TrackFrame()
    {
        tempFrame = new FrameData()
        {
            m_Position = transform.position,
            m_Rotation = transform.rotation,

            timeStamp = Game.t,

            enabled = gameObject.activeSelf
        };
        frames.Add(tempFrame);
    }
}
