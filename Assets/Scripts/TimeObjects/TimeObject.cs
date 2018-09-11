using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Base Class for objects to be controlled by time
/// Created by : Ian Jones - 19/03/17
/// Updated by : Ian Jones - 06/04/18
/// </summary>
public class TimeObject : MonoBehaviour
{
    protected TransformFrameData tempBFrame;
    protected List<TransformFrameData> bFrames = new List<TransformFrameData>();

    public int startFrame, finishFrame;

    public TimeObjectState tObjectState = TimeObjectState.Void;

    public float currentFrame;

    #region Events
    public delegate void TimeObjectEvent();
    public event TimeObjectEvent OnPlayFrame;
    public event TimeObjectEvent OnStartPlayback;
    public event TimeObjectEvent OnFinishPlayback;
    public event TimeObjectEvent OnStartReverse;
    public event TimeObjectEvent OnFinishReverse;
    public event TimeObjectEvent OnTrackFrame;
    #endregion

    public int TotalFrames
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

    protected virtual void Awake()
    {
        startFrame = (int)TimeObjectManager.t;

        OnTrackFrame += TrackTransform;
        OnPlayFrame += PlayTransormFrame;
    }

    protected virtual void Update()
    {
        switch (TimeObjectManager.timeState)
        {
            case TimeState.Forward:

                switch (tObjectState)
                {
                    case TimeObjectState.Present:

                        OnTrackFrame();
                        break;

                    case TimeObjectState.PastStart:

                        if (TimeObjectManager.t >= startFrame)
                        {
                            //This (hopefully keeps the object in sync if game time 
                            //skips past it's start frame somehow
                            if (TimeObjectManager.t > startFrame)
                            {
                                //Debug.Log("Game time (" + TimeObjectManager.t + ") + greater than " +
                                //    "start frame (" + startFrame + ") + skipping ahead");
                                currentFrame = (int)TimeObjectManager.t - startFrame;
                            }
                            tObjectState = TimeObjectState.PastPlaying;

                            if (OnStartPlayback != null)
                                OnStartPlayback();
                        }

                        break;

                    case TimeObjectState.PastPlaying:

                        //If finish frame is 0 then timeobject hasn't finished yet and 
                        //will need extra tracking
                        if (TimeObjectManager.t >= finishFrame && finishFrame != 0 ||
                            (finishFrame == 0 &&
                            TimeObjectManager.t >= bFrames[bFrames.Count - 1].timeStamp))
                        {
                            if (finishFrame == 0)
                            {
                                Debug.Log("Gotta finish: " + name);
                                tObjectState = TimeObjectState.Present;
                                OnTrackFrame();
                            }
                            else
                            {
                                tObjectState = TimeObjectState.PastFinished;

                                if (OnFinishPlayback != null)
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
                    //When time is reversing if objects are present we need to begin reversing them
                    case TimeObjectState.Present:
                    case TimeObjectState.PresentDead:

                        OnPast();

                        break;

                    case TimeObjectState.PastStart:
                        break;

                    case TimeObjectState.PastPlaying:

                        if (TimeObjectManager.t <= startFrame || TimeObjectManager.t <= TimeObjectManager.startT)
                        {
                            tObjectState = TimeObjectState.PastStart;

                            if (OnFinishReverse != null)
                                OnFinishReverse();

                            currentFrame = 0;
                            break;
                        }

                        OnPlayFrame();

                        break;

                    case TimeObjectState.PastFinished:

                        //We want to make sure it starts at the right time or starts 
                        //reversing if it's in the present
                        if (TimeObjectManager.t <= finishFrame || (finishFrame == 0 &&
                            TimeObjectManager.t <= bFrames[bFrames.Count - 1].timeStamp))
                        {
                            tObjectState = TimeObjectState.PastPlaying;
                            if (OnStartReverse != null)
                                OnStartReverse();
                        }
                        break;
                }

                break;
        }
    }

    protected void TrackTransform()
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

    protected void PlayTransormFrame()
    {
        if (bFrames.WithinRange(currentFrame))
        {
            transform.position = bFrames[(int)currentFrame].m_Position;
            transform.rotation = bFrames[(int)currentFrame].m_Rotation;
            transform.localScale = bFrames[(int)currentFrame].m_Scale;

            gameObject.SetActive(bFrames[(int)currentFrame].enabled);

            currentFrame += TimeObjectManager.DeltaT;
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
            finishFrame = (int)TimeObjectManager.t;
        }

        currentFrame = bFrames.Count + TimeObjectManager.DeltaT;

        //Just in case a frame or to is skipped we will attempt to 
        //keep object in sync by subtracting the difference between their finish frame and current game time
        //- (finishFrame - TimeObjectManager.t)
        //if (finishFrame != 0)
        //{
        //    currentFrame = (int)(bFrames.Count - Mathf.Abs(finishFrame - TimeObjectManager.t) + TimeObjectManager.DeltaT);
        //}
        //else //With no finish frame we just start from the end of the list
        //{
        //}
    }

    private void OnDestroy()
    {
        OnTrackFrame -= TrackTransform;
        OnPlayFrame -= PlayTransormFrame;
    }
}
                                                          