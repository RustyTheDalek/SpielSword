using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base Clas for objects to be controlled by time
/// </summary>
/// <typeparam name="T"> Frame Data to be used to replaying objects actions</typeparam>
public abstract class BaseTimeObject<T> : MonoBehaviour where T : FrameData
{

#if UNITY_EDITOR

    protected TextMesh debugText;

#endif

    protected T tempFrame;
    protected List<T> frames = new List<T>();

    public int startFrame, finishFrame;

    public TimeObjectState tObjectState = TimeObjectState.Void;

    public int currentFrame;

    protected int TotalFrames
    {
        get
        {
            if (frames != null)
                return frames.Count;
            else
            {
                Debug.LogWarning("Frame list null, returning 0");
                return 0;
            }
        }
    }

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
                            //Just in case a frame or to is skipped we will attempt to 
                            //keep object in sync by subtracting the difference between their start frame and current game time
                            currentFrame = 0;
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
                            //Just in case a frame or to is skipped we will attempt to 
                            //keep object in sync by subtracting the difference between their finish frame and current game time
                            //- (finishFrame - Game.t)
                            currentFrame =  (frames.Count - 1) ;
                            OnStartReverse();
                        }
                        break;
                }

                break;
        }

#if UNITY_EDITOR

        if (debugText && frames != null)
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

    protected abstract void PlayFrame();
    protected abstract void TrackFrame();

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

    /// <summary>
    /// Called when an object finishes its rewind
    /// </summary>
    protected virtual void OnFinishReverse() { }

    /// <summary>
    /// Called when an objects starts its rewind
    /// </summary>
    protected virtual void OnStartReverse() { }

    /// <summary>
    /// Called when an object starts playback
    /// </summary>
    protected virtual void OnStartPlayback() { }

    /// <summary>
    /// Called when an object finishes playback
    /// </summary>
    protected virtual void OnFinishPlayback() { }
}
                                                          