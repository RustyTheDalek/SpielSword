using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Maintains logic for Spawnable Sprite objects
/// Created by : Ian Jones - 22/04/17
/// Updated by : Ian Jones - 06/04/18
/// </summary>
public class SpawnableSpriteTimeObject : SpriteTimeObject
{
    protected SpawnableSpriteFrameData tempSSFrame;
    protected List<SpawnableSpriteFrameData> sSFrames = new List<SpawnableSpriteFrameData>();

    protected Animator m_anim;

    /// <summary>
    /// Who created the Sprite object, if any
    /// </summary>
    public GameObject creator;

    protected override void Awake()
    {
        base.Awake();

        m_anim = GetComponent<Animator>();

        OnTrackFrame += TrackSpawnableSpriteFrame;
        OnPlayFrame += PlaySpawnableSpriteFrame;
        OnFinishReverse += OnFinishSpawnableSpriteReverse;
        OnFinishPlayback += OnFinishSpawnableSpritePlayback;
    }

    protected virtual void Start()
    {
        //TODO: Remove this if not needed
        tObjectState = TimeObjectState.Present;

        //TimeObjectManager.vSpawnable.Add(this);
    }

    protected void TrackSpawnableSpriteFrame()
    {
        tempSSFrame = new SpawnableSpriteFrameData()
        {
            active = gameObject.activeSelf,
        };

        sSFrames.Add(tempSSFrame);
        
        //When the sprite is disabled that means the spawnable sprite has finished and needs to stop
        if(!m_Sprite.enabled)
        {
            tObjectState = TimeObjectState.PresentDead;
            finishFrame = Game.t;
        }
    }

    protected void PlaySpawnableSpriteFrame()
    {
        if (Tools.WithinRange(currentFrame, sSFrames))
        {
            gameObject.SetActive(sSFrames[currentFrame].active);
            m_Sprite.enabled = sSFrames[currentFrame].active;

            switch (Game.timeState)
            {
                case TimeState.Forward:

                    if (sSFrames[currentFrame].marty)
                    {
                        Debug.Log(this.name + " Martyed");
                        m_anim.SetTrigger("Marty");
                    }
                    break;

                case TimeState.Backward:

                    if (sSFrames[currentFrame].marty)
                    {
                        Debug.Log(this.name + " UnMartyed");
                        m_anim.SetTrigger("UnMarty");
                    }
                    break;
            }
        }
    }

    //protected override void OnStartSpawnableSpriteReverse()
    //{
    //    //m_Sprite.enabled = true;
    //}

    protected void OnFinishSpawnableSpriteReverse()
    {
        m_Sprite.enabled = false;
    }

    protected void OnFinishSpawnableSpritePlayback()
    {
        m_Sprite.enabled = false;
    }

    protected void SetActive(bool active)
    {
        GetComponent<SpriteRenderer>().enabled = active;

        if (GetComponent<Collider2D>())
            GetComponent<Collider2D>().enabled = active;

        if (GetComponent<Rigidbody2D>())
            GetComponent<Rigidbody2D>().simulated = active;
    }

    protected override void OnPast()
    {
        tObjectState = TimeObjectState.PastFinished;
        base.OnPast();
    }

    public void SetMartyPoint()
    {
        //deathOrMarty = false;
        try
        {
            tempSSFrame = sSFrames[currentFrame];
            tempSSFrame.marty = true;
            m_anim.SetTrigger("Marty");
            sSFrames[currentFrame] = tempSSFrame;

            finishFrame = bFrames[currentFrame].timeStamp;

            for (int i = currentFrame + 1; i < bFrames.Count; i++)
            {
                bFrames.RemoveAt(i);
                sSFrames.RemoveAt(i);
                sFrames.RemoveAt(i);
            }
        }
        catch
        {
            Debug.LogError("Frames " + TotalFrames + " : " + sSFrames.Count);
        }
    }
}
