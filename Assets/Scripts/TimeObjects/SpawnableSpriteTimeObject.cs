using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Maintains logic for Spawnable Sprite objects
/// Created by : Ian Jones - 22/04/17
/// Updated by : Ian Jones - 06/04/18
/// </summary>
public class SpawnableSpriteTimeObject : RigidbodyTimeObject
{
    protected SpawnableSpriteFrameData tempSSFrame;
    protected List<SpawnableSpriteFrameData> sSFrames = new List<SpawnableSpriteFrameData>();

    protected Animator m_anim;

    /// <summary>
    /// Who created the Sprite object, if any
    /// </summary>
    public Villager creator;

    protected bool spawnableActive = false;
    /// <summary>
    /// Lifetime of spawnable object, 0=doesn't expire to time
    /// </summary>
    public float    spawnableLife = 5,
                    spawnableTimer = 0;


    protected override void Awake()
    {
        base.Awake();

        m_anim = GetComponent<Animator>();

        OnTrackFrame += TrackSpawnableSpriteFrame;
        OnPlayFrame += PlaySpawnableSpriteFrame;
        OnFinishReverse += OnFinishSpawnableSpriteReverse;
        OnFinishPlayback += OnFinishSpawnableSpritePlayback;
    }

    private void OnEnable()
    {
        if(TimeObjectManager.t > 0 && startFrame == 0)
        {
            startFrame = (int)TimeObjectManager.t;
        }
    }

    protected virtual void Start()
    {
        //TODO: Remove this if not needed
        tObjectState = TimeObjectState.Present;

        spawnableActive = true;
        spawnableTimer = spawnableLife;

        if (spawnableTimer == 0)
        {
            spawnableLife = Mathf.Infinity;
            spawnableTimer = Mathf.Infinity;
        }
    }

    protected override void Update()
    {
        if (spawnableTimer > 0 && ( tObjectState == TimeObjectState.Present ||
                                    tObjectState == TimeObjectState.PastPlaying))
        {
            spawnableTimer -= Time.deltaTime;
        }
        else if (spawnableTimer <= 0 && spawnableActive)
        {
            SetActive(false);

            if (finishFrame == 0)
            {
                finishFrame = (int)TimeObjectManager.t;
            }

            spawnableActive = false;
        }

        base.Update();
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
            finishFrame = (int)TimeObjectManager.t;
        }
    }

    protected void PlaySpawnableSpriteFrame()
    {
        if (sSFrames.WithinRange(currentFrame))
        {
            gameObject.SetActive(sSFrames[(int)currentFrame].active);
            m_Sprite.enabled = sSFrames[(int)currentFrame].active;

            switch (TimeObjectManager.timeState)
            {
                case TimeState.Forward:

                    if (sSFrames[(int)currentFrame].marty)
                    {
                        Debug.Log(this.name + " Martyed");
                        m_anim.SetTrigger("Marty");
                    }
                    break;

                case TimeState.Backward:

                    if (sSFrames[(int)currentFrame].marty)
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
            tempSSFrame = sSFrames[(int)currentFrame];
            tempSSFrame.marty = true;
            m_anim.SetTrigger("Marty");
            sSFrames[(int)currentFrame] = tempSSFrame;

            finishFrame = bFrames[(int)currentFrame].timeStamp;

            for (int i = (int)currentFrame + 1; i < bFrames.Count; i++)
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

    private void OnDestroy()
    {
        OnTrackFrame -= TrackSpawnableSpriteFrame;
        OnPlayFrame -= PlaySpawnableSpriteFrame;
        OnFinishReverse -= OnFinishSpawnableSpriteReverse;
        OnFinishPlayback -= OnFinishSpawnableSpritePlayback;
    }
}
