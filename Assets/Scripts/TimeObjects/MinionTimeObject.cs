using UnityEngine;

/// <summary>
/// To be used with Minions so they rewind but don't replay
/// </summary>
public class MinionTimeObject : AnimatorTimeObject {

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();

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

        m_Sprite.color = new Color(m_Sprite.color.r, m_Sprite.color.g, m_Sprite.color.b, 1f);

        //TODO: Improve this trash - Just use Minion base class instead?
        if (GetComponent<FlightMinion>())
        {
            GetComponent<FlightMinion>().enabled = true;
        }

        if (GetComponent<GroundMinion>())
        {
            GetComponent<GroundMinion>().enabled = true;
        }

        GetComponent<Minion>().Reset();
        m_Anim.enabled = true;
    }

    private void DisableMinion()
    {
        //TODO: Improve this trash
        if(GetComponent<FlightMinion>())
        {
            GetComponent<FlightMinion>().enabled = false;
        }

        if(GetComponent<GroundMinion>())
        {
            GetComponent<GroundMinion>().enabled = false;
        }
    }

    private void OnDestroy()
    {
        OnStartPlayback -= ResetTracking;
    }
}
