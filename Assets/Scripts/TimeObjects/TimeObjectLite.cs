using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For objects that you want to work within the contstraints of Time but not to be 
/// tracked or reset
/// </summary>
public abstract class TimeObjectLite : MonoBehaviour {
	
	// Update is called once per frame
	void Update ()
    {
        if (TimeObjectManager.timeState == TimeState.Forward)
        {
            TOLUpdate();
        }
	}

    /// <summary>
    /// TimeObjectLite's update function to only be called when game is progressing 
    /// forward in time
    /// </summary>
    protected abstract void TOLUpdate();
}
