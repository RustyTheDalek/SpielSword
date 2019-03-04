using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformTracking : ObjectTrackBase
{
    protected TransformFrameData tempBFrame;
    protected List<TransformFrameData> bFrames = new List<TransformFrameData>();

    public override void ResetToPresent()
    {
        bFrames.Clear();
    }

    public override void PlayFrame(int currentFrame)
    {
        if (bFrames.WithinRange(currentFrame))
        {
            transform.position = bFrames[(int)currentFrame].m_Position;
            transform.rotation = bFrames[(int)currentFrame].m_Rotation;
            transform.localScale = bFrames[(int)currentFrame].m_Scale;

            gameObject.SetActive(bFrames[(int)currentFrame].enabled);
        }
    }

    public override void TrackFrame()
    {
        tempBFrame = new TransformFrameData()
        {
            m_Position = transform.position,
            m_Rotation = transform.rotation,
            m_Scale = transform.localScale,
            timeStamp = (int)TimeObjectManager.t,

            enabled = gameObject.activeSelf
        };
        bFrames.Add(tempBFrame);
    }
}
