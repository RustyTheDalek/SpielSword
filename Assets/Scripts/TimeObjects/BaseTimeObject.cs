using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base Clas for objects to be controlled by time
/// </summary>
/// <typeparam name="T"> Frame Data to be used to replaying objects actions</typeparam>
public abstract class BaseTimeObject <T> : MonoBehaviour where T : FrameData
{

#if UNITY_EDITOR

    protected TextMesh debugText;

#endif

    protected T tempFrame;
    protected List<T> frames = new List<T>();

    /// <summary>
    /// If the object is replaying it's past
    /// </summary>
    public bool replaying = false;

    protected TimeObjectState tObjectState = TimeObjectState.Present;

    public int currentFrame;

    protected int totalFrames
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

#if UNITY_EDITOR

        debugText = GetComponentInChildren<TextMesh>();

#endif

    }

    protected void LateUpdate()
    {
        switch (tObjectState)
        {
            case TimeObjectState.Present:

                TrackFrame();
                break;

            case TimeObjectState.Past:

                Playback();
                break;
        }

#if UNITY_EDITOR

        debugText.text = "Time State : " + tObjectState.ToString() +
                            "\nTotal Frames: " + totalFrames +
                            "\nCurrent Frame: " + currentFrame +
                            "\nStart Frame: " + frames[0].timeStamp +
                            "\nFinish Frame: " + frames[frames.Count - 1].timeStamp;

#endif
    }

    protected void Playback()
    {
        if (totalFrames > 0)
        {
            //If Game time matches start frame begin playback
            if (Game.t == frames[0].timeStamp && !replaying)
            {
                replaying = true;
            }

            if (replaying)
            {
                PlayFrame();
            }

            if (Game.t == frames[frames.Count - 1].timeStamp && replaying)
            {
                replaying = false;
            }
        }
    }

    protected abstract void PlayFrame();
    protected abstract void TrackFrame();

    public virtual void Reset()
    {
        switch (tObjectState)
        {
            case TimeObjectState.Present:
                OnPast();
                break;

            case TimeObjectState.Past:
                currentFrame = 0;
                break;
        }
    }

    protected virtual void OnPast()
    {
        tObjectState = TimeObjectState.Past;
    }
}
                                                          