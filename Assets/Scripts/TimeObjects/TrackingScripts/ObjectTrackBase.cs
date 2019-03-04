using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectTrackBase : MonoBehaviour
{
    public ObjectTrackType objectTrackType;

    public virtual void ResetToPresent() { }
    public virtual void TrackFrame() { }
    public virtual void PlayFrame(int currentFrame) { }

    public virtual void OnStartReverse() { }
    public virtual void OnFinishReverse(int startFrame) { }

    public virtual void OnStartPlayback(int startFrame) { }
    public virtual void OnFinishPlayback() { }
   
}