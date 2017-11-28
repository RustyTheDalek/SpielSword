using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base Class for objects to be controlled by time
/// </summary>
public abstract class BaseTimeObject : MonoBehaviour
{
    protected BaseFrameData tempBFrame;
    protected List<BaseFrameData> bFrames = new List<BaseFrameData>();

    public int startFrame, finishFrame;

    public TimeObjectState tObjectState = TimeObjectState.Void;

    public int currentFrame;

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

#endif

    protected abstract void PlayFrame();
    protected abstract void TrackFrame();

    /// <summary>
    /// Called when an object finishes its rewind
    /// </summary>
    protected abstract void OnFinishReverse();

    /// <summary>
    /// Called when an objects starts its rewind
    /// </summary>
    protected abstract void OnStartReverse();

    /// <summary>
    /// Called when an object starts playback
    /// </summary>
    protected abstract void OnStartPlayback();

    /// <summary>
    /// Called when an object finishes playback
    /// </summary>
    protected abstract void OnFinishPlayback();
}
                                                          