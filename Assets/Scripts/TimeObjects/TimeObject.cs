/// <summary>
/// Object that can be affected by time in both directions
/// </summary>
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeObject : BaseTimeObject2
{

    // Use this for initialization
    protected virtual void Start()
    {

        startFrame = Game.t;

#if UNITY_EDITOR

        debugText = GetComponentInChildren<TextMesh>();

#endif

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
                            currentFrame = Game.t - startFrame;
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
                            OnStartTime();
                            break;
                        }

                        Playback();

                        break;

                    case TimeObjectState.PastFinished:

                        if (Game.t <= finishFrame)
                        {
                            tObjectState = TimeObjectState.PastPlaying;
                            //Just in case a frame or to is skipped we will attempt to 
                            //keep object in sync by subtracting the difference between their finish frame and current game time
                            //- (finishFrame - Game.t)
                            currentFrame = (bFrames.Count - Mathf.Abs(finishFrame -Game.t) - 1);
                            OnStartReverse();
                        }
                        break;
                }

                break;
        }

#if UNITY_EDITOR

        if (debugText && bFrames != null)
        {
            if (Game.debugText)
            {
                debugText.text = "Time State: " + tObjectState.ToString() +
                                    "\nTotal Frames: " + TotalFrames +
                                    "\nCurrent Frame: " + currentFrame +
                                    "\nStart Frame: " + startFrame +
                                    "\nFinish Frame: " + finishFrame;
            }
            else
            {
                debugText.gameObject.SetActive(false);
            }
        }
#endif
    }

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
        finishFrame = Game.t;
    }

   
    public override void OnStartTime()
    {
        tObjectState = TimeObjectState.PastStart;
        currentFrame = 0;
        OnFinishReverse();
    }
}
