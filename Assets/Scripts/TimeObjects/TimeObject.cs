using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Base Class for objects to be controlled by time
/// </summary>
public class TimeObject : MonoBehaviour
{
    protected TransformFrameData tempBFrame;
    protected List<TransformFrameData> bFrames = new List<TransformFrameData>();

    public int startFrame, finishFrame;

    public TimeObjectState tObjectState = TimeObjectState.Void;

    public int currentFrame;

    public delegate void PlayingFrame();
    public event PlayingFrame OnPlayFrame;

    public delegate void StartingPlayback();
    public event StartingPlayback OnStartPlayback;

    public delegate void FinishingPlayback();
    public event StartingPlayback OnFinishPlayback;

    public delegate void StartReverse();
    public event StartReverse OnStartReverse;

    public delegate void FinishReverse();
    public event FinishReverse OnFinishReverse;

    public delegate void TrackingFrame();
    public event TrackingFrame OnTrackFrame;

    protected int TotalFrames
    {
        get
        {
            if (bFrames != null)
                return bFrames.Count;
            else
            {
                Debug.LogWarning("Frame list null, returning 0");
                return 0;
            }
        }
    }

#if UNITY_EDITOR

    public GUIStyle DebugUI;

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

    protected virtual void Awake()
    {
        startFrame = Game.t;

        OnTrackFrame += TrackTransform;
        OnPlayFrame += PlayTransormFrame;

        GameManager.OnPlayerDeath += SoftReset;
    }

    protected virtual void Update()
    {
        switch (Game.timeState)
        {
            case TimeState.Forward:

                switch (tObjectState)
                {
                    case TimeObjectState.Present:

                        //TrackFrame();
                        OnTrackFrame();
                        break;

                    case TimeObjectState.PastStart:

                        if (Game.t >= startFrame)
                        {
                            //This (hopefully keeps the object in sync if game time 
                            //skips past it's start frame somehow
                            if (Game.t > startFrame)
                            {
                                //Debug.Log("Game time (" + Game.t + ") + greater than " +
                                //    "start frame (" + startFrame + ") + skipping ahead");
                                currentFrame = Game.t - startFrame;
                            }
                            tObjectState = TimeObjectState.PastPlaying;

                            if(OnStartPlayback != null)
                                OnStartPlayback();
                        }

                        break;

                    case TimeObjectState.PastPlaying:

                        //If finish frame is 0 then timeobject hasn't finished yet and 
                        //will need extra tracking
                        if (Game.t >= finishFrame && finishFrame != 0 ||
                            (finishFrame == 0 && 
                            Game.t >= bFrames[bFrames.Count - 1].timeStamp))
                        {
                            if (finishFrame == 0)
                            {
                                Debug.Log("Gotta finish");
                                tObjectState = TimeObjectState.Present;
                                OnTrackFrame();
                            }
                            else
                            {
                                tObjectState = TimeObjectState.PastFinished;

                                if(OnFinishPlayback != null)
                                    OnFinishPlayback();

                                currentFrame = finishFrame;
                            }
                            break;
                        }

                        OnPlayFrame();

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

                            if(OnFinishReverse != null)
                                OnFinishReverse();

                            currentFrame = 0;
                            break;
                        }

                        OnPlayFrame();

                        break;

                    case TimeObjectState.PastFinished:

                        //We want to make sure it starts at the right time or starts 
                        //reversing if it's in the present
                        if (Game.t <= finishFrame || (finishFrame == 0 && 
                            Game.t <= bFrames[bFrames.Count - 1].timeStamp))
                        {
                            tObjectState = TimeObjectState.PastPlaying;
                            if(OnStartReverse != null)
                                OnStartReverse();

                            //Just in case a frame or to is skipped we will attempt to 
                            //keep object in sync by subtracting the difference between their finish frame and current game time
                            //- (finishFrame - Game.t)
                            currentFrame = (bFrames.Count - Mathf.Abs(finishFrame - Game.t) - 1);
                        }
                        break;
                }

                break;
        }
    }

    //protected abstract void PlayFrame();
    //protected abstract void TrackFrame();

    protected void TrackTransform()
    {
        tempBFrame = new TransformFrameData()
        {
            m_Position = transform.position,
            m_Rotation = transform.rotation,
            m_Scale = transform.localScale,
            timeStamp = Game.t,

            enabled = gameObject.activeSelf
        };
        bFrames.Add(tempBFrame);
    }

    protected void PlayTransormFrame()
    {
        if (Tools.WithinRange(currentFrame, bFrames))
        {
            transform.position = bFrames[currentFrame].m_Position;
            transform.rotation = bFrames[currentFrame].m_Rotation;
            transform.localScale = bFrames[currentFrame].m_Scale;

            gameObject.SetActive(bFrames[currentFrame].enabled);

            currentFrame += Game.GameScale;
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
}
                                                          