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
        tempFrame = new FrameData();

        tempFrame.m_Position = transform.position;
        tempFrame.m_Rotation = transform.rotation;

        tempFrame.timeStamp = Game.t;

        tempFrame.enabled = gameObject.activeSelf;

        frames.Add(tempFrame);
    }
}
