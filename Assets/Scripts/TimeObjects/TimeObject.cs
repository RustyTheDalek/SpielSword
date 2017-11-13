/// <summary>
/// Object that can be affected by time in both directions
/// </summary>
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TimeObject : BaseTimeObject
{

    // Use this for initialization
    protected virtual void Start()
    {
        startFrame = Game.t;
    }

    protected virtual void Update()
    {
        switch (Game.timeState)
        {
            case TimeState.Forward:

                switch (tObjectState)
                {
                    case TimeObjectState.Present:

                        TrackFrame();
                        break;

                    case TimeObjectState.PastStart:

                        if (Game.t >= startFrame)
                        {
                            tObjectState = TimeObjectState.PastPlaying;
                            OnStartPlayback();
                        }

                        break;

                    case TimeObjectState.PastPlaying:

                        if (Game.t >= finishFrame)
                        {
                            tObjectState = TimeObjectState.PastFinished;
                            OnFinishPlayback();
                            break;
                        }

                        Playback();

                        break;

                    case TimeObjectState.PastFinished:

                        break;
                }

                break;

            case TimeState.Backward:

                switch (tObjectState)
                {
                    case TimeObjectState.PastStart:
                        break;

                    case TimeObjectState.PastPlaying:

                        if (Game.t <= startFrame)
                        {
                            tObjectState = TimeObjectState.PastStart;
                            OnFinishReverse();
                            break;
                        }

                        Playback();

                        break;

                    case TimeObjectState.PastFinished:

                        if (Game.t <= finishFrame)
                        {
                            tObjectState = TimeObjectState.PastPlaying;
                            OnStartReverse();
                        }
                        break;
                }

                break;
        }
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (Game.debugText)
        {
            Handles.Label(transform.position, "Time State: " + tObjectState.ToString()
                                + "\nTotal Frames: " + TotalFrames +
                                "\nCurrent Frame: " + currentFrame +
                                "\nStart Frame: " + startFrame +
                                "\nFinish Frame: " + finishFrame, DebugUI);
        }
        //else
        //{
        //    debugText.gameObject.SetActive(false);
        //}
    }

#endif

    protected void Playback()
    {
        if (TotalFrames > 0)
        {
            PlayFrame();
        }
    }

    protected override void PlayFrame()
    {
        if (Tools.WithinRange(currentFrame, bFrames))
        {
            transform.position = bFrames[currentFrame].m_Position;
            transform.rotation = bFrames[currentFrame].m_Rotation;

            gameObject.SetActive(bFrames[currentFrame].enabled);

            currentFrame += Game.GameScale;
        }
    }

    protected override void TrackFrame()
    {
        tempBFrame = new BaseFrameData()
        {
            m_Position = transform.position,
            m_Rotation = transform.rotation,

            timeStamp = Game.t,

            enabled = gameObject.activeSelf
        };
        bFrames.Add(tempBFrame);
    }

    public virtual void HardReset()
    {
        switch (tObjectState)
        {
            case TimeObjectState.Present:
            case TimeObjectState.PresentDead:
                OnPast();
                break;

            case TimeObjectState.PastPlaying:
            case TimeObjectState.PastFinished:
            case TimeObjectState.PastStart:
                currentFrame = 0;
                break;
        }
    }

    public virtual void SoftReset()
    {

        switch (tObjectState)
        {
            case TimeObjectState.Present:
            case TimeObjectState.PresentDead:
                OnPast();
                break;

            case TimeObjectState.PastPlaying:
            case TimeObjectState.PastFinished:
            case TimeObjectState.PastStart:
                OnStartReverse();
                break;
        }
    }

    /// <summary>
    /// Called when an object becomes a past object
    /// </summary>
    protected virtual void OnPast()
    {
        tObjectState = TimeObjectState.PastFinished;

        if (finishFrame == 0)
        {
            finishFrame = Game.t;
        }
    }

    protected override void OnFinishPlayback()
    {

    }

    protected override void OnFinishReverse()
    {
        currentFrame = 0;
    }

    protected override void OnStartReverse()
    {
        //Just in case a frame or to is skipped we will attempt to 
        //keep object in sync by subtracting the difference between their finish frame and current game time
        //- (finishFrame - Game.t)
        currentFrame = (bFrames.Count - Mathf.Abs(finishFrame - Game.t) - 1);
    }

    protected override void OnStartPlayback()
    {
        tObjectState = TimeObjectState.PastPlaying;
        currentFrame = Game.t - startFrame;
    }

    public void OnFinishReverseCatch()
    {
        OnFinishReverse();
    }
}
