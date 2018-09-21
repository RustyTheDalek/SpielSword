using UnityEngine;

/// <summary>
/// To be used with Minions so they rewind but don't replay
/// </summary>
public class MinionTimeObject : AnimatorTimeObject {

    Minion minion;

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();

        minion = GetComponent<Minion>();

        OnStartPlayback += ResetTracking;
        OnStartReverse += DisableMinion;
    }

    private void ResetTracking()
    {
        bFrames.Clear();
        pFrames.Clear();
        sFrames.Clear();

        tObjectState = TimeObjectState.Present;

        finishFrame = 0;

        minion.enabled = true;

        m_Anim.enabled = true;
    }

    private void DisableMinion()
    {
        minion.enabled = false;
    }

    private void OnDestroy()
    {
        OnStartPlayback -= ResetTracking;
    }
}
