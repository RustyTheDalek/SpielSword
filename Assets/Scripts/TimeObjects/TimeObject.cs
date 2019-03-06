using System.Collections.Generic;
using UnityEngine;
using System;
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

    public int startFrame, finishFrame;

    public TimeObjectState tObjectState = TimeObjectState.Void;

    public float currentFrame;

    [Tooltip("Script relating to logic that object uses in present so that it " +
    "can be disabled during runtime")]
    public List<MonoBehaviour> m_Behaviours;

    [Tooltip("If enabled timeObject will recycle itself after rewinding")]
    public bool oneLife = false;

    [Tooltip("If enabled timeObject will reset after rewinding")]
    public bool rewindOnly = false;

    public List<ObjectTrackBase> componentsToTrack;

    public bool finished = false;

    #region Events
    public delegate void TimeObjectEvent();

    public delegate void TimeObjectEventFrame(int frame);
    public event TimeObjectEventFrame OnPlayFrame;
    public event TimeObjectEventFrame OnStartPlayback;
    public event TimeObjectEvent OnFinishPlayback;
    public event TimeObjectEvent OnStartReverse;
    public event TimeObjectEventFrame OnFinishReverse;
    public event TimeObjectEvent OnTrackFrame;
    #endregion

    public int TotalFrames
    {
        get
        {
            //    if (componentsToTrack != null)
            //    {
            //        return componentsToTrack[0].FrameCount;
            //    }
            //    else
            //    {
            //        Debug.LogWarning("Frame list null, returning 0");
            return 0;
        }
        //}
    }

    protected virtual void Awake()
    {
        startFrame = (int)TimeObjectManager.t;

        ObjectTrackBase[] objsToTrack = GetComponentsInChildren<ObjectTrackBase>();

        componentsToTrack.AddRange(objsToTrack);

        foreach(ObjectTrackBase componentToTrack in componentsToTrack)
        {
            //componentToTrack.Initalise(this);
            if (componentToTrack.objectTrackType == ObjectTrackType.FrameTracking)
            {
                OnTrackFrame += componentToTrack.TrackFrame;
                OnPlayFrame += componentToTrack.PlayFrame;
            }
            OnStartPlayback     += componentToTrack.OnStartPlayback;
            OnFinishPlayback    += componentToTrack.OnFinishPlayback;
            OnStartReverse      += componentToTrack.OnStartReverse;
            OnFinishReverse     += componentToTrack.OnFinishReverse;
        }
    }

    protected virtual void OnEnable()
    {
        tObjectState = TimeObjectState.Present;
        startFrame = (int)TimeObjectManager.t;
    }

    protected virtual void Update()
    {
        currentFrame = Mathf.Clamp(TimeObjectManager.t - startFrame - 1, 0, finishFrame);

        switch (TimeObjectManager.timeState)
        {
            case TimeState.Forward:

                switch (tObjectState)
                {
                    case TimeObjectState.Present:

                        if (OnTrackFrame != null)
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
                            {
                                if (m_Behaviours != null)
                                {
                                    foreach (MonoBehaviour behaviour in m_Behaviours)
                                    {
                                        behaviour.enabled = false;
                                    }
                                }

                                OnStartPlayback(startFrame);
                            }
                        }

                        break;

                    case TimeObjectState.PastPlaying:

                        //If finish frame is 0 then timeobject hasn't finished yet and 
                        //will need extra tracking
                        if ( TimeObjectManager.t >= finishFrame && finishFrame != 0 ||
                            (TimeObjectManager.t >= finishFrame && !finished))
                        {
                            if (!finished)
                            {
                                Debug.Log("Gotta finish: " + name);
                                tObjectState = TimeObjectState.Present;
                                OnTrackFrame();
                                finishFrame = 0;

                                if (m_Behaviours != null)
                                {
                                    foreach (MonoBehaviour behaviour in m_Behaviours)
                                    {
                                        behaviour.enabled = true;
                                    }
                                }
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

                        if(OnPlayFrame != null)
                            OnPlayFrame((int)currentFrame);

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

                        if (TimeObjectManager.t <= startFrame || 
                            TimeObjectManager.t <= TimeObjectManager.startT)
                        {
                            tObjectState = TimeObjectState.PastStart;

                            if (OnFinishReverse != null)
                            {
                                if (oneLife)
                                    this.Recycle();

                                OnFinishReverse(startFrame);

                                if (rewindOnly)
                                {

                                    foreach (ObjectTrackBase objectToTrack in componentsToTrack)
                                    {
                                        objectToTrack.ResetToPresent();

                                        if (m_Behaviours != null)
                                        {
                                            foreach (MonoBehaviour behaviour in m_Behaviours)
                                            {
                                                behaviour.enabled = true;
                                            }
                                        }

                                        tObjectState = TimeObjectState.Present;
                                        finishFrame = 0;
                                    }
                                }
                            }

                            currentFrame = 0;
                            break;
                        }

                        if(OnPlayFrame != null)
                            OnPlayFrame((int)currentFrame);

                        break;

                    case TimeObjectState.PastFinished:

                        //We want to make sure it starts at the right time or starts 
                        //reversing if it's in the present
                        if (TimeObjectManager.t <= finishFrame)
                        {
                            tObjectState = TimeObjectState.PastPlaying;
                            if (OnStartReverse != null)
                            {
                                if (m_Behaviours != null)
                                {
                                    foreach (MonoBehaviour behaviour in m_Behaviours)
                                    {
                                        behaviour.enabled = false;
                                    }
                                }

                                OnStartReverse();
                            }
                        }
                        break;
                }

                break;
        }
    }

    public void FinishTracking()
    {
        tObjectState = TimeObjectState.PresentDead;
        finishFrame = (int)TimeObjectManager.t;
        finished = true;
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

        //currentFrame = bFrames.Count + TimeObjectManager.DeltaT - 1;

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
        foreach (ObjectTrackBase componentToTrack in componentsToTrack)
        {
            if (componentToTrack.objectTrackType == ObjectTrackType.FrameTracking)
            {
                OnTrackFrame -= componentToTrack.TrackFrame;
                OnPlayFrame -= componentToTrack.PlayFrame;
            }
            OnStartPlayback -= componentToTrack.OnStartPlayback;
            OnFinishPlayback -= componentToTrack.OnFinishPlayback;
            OnStartReverse -= componentToTrack.OnStartReverse;
            OnFinishReverse -= componentToTrack.OnFinishReverse;
        }
    }
}
                                                          