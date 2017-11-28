using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpawnableSpriteTimeObject : SpriteTimeObject
{
    protected SpawnableSpriteFrameData tempSSFrame;
    protected List<SpawnableSpriteFrameData> sSFrames = new List<SpawnableSpriteFrameData>();

    protected Animator m_anim;

    protected override void Start()
    {
        base.Start();

        tObjectState = TimeObjectState.Present;

        TimeObjectManager.vSpawnable.Add(this);

        m_anim = GetComponent<Animator>();
    }

    protected override void Update()
    {
        switch (Game.timeState)
        {
            case TimeState.Forward:

                switch (tObjectState)
                {
                    case TimeObjectState.Present:

                        if (m_Sprite.enabled)
                        {
                            TrackFrame();
                        }
                        else
                        {
                            tObjectState = TimeObjectState.PresentDead;
                            finishFrame = Game.t;
                        }

                        break;

                    case TimeObjectState.PastStart:

                        if (Game.t >= startFrame)
                        {
                            tObjectState = TimeObjectState.PastPlaying;
                            //Just in case a frame or to is skipped we will attempt to 
                            //keep object in sync by subtracting the difference between their start frame and current game time
                            currentFrame = 0;
                            OnStartPlayback();
                        }

                        break;

                    case TimeObjectState.PastPlaying:

                        if ((Game.t >= finishFrame && finishFrame != 0) ||
                            (finishFrame == 0 && Game.t >= bFrames[bFrames.Count - 1].timeStamp))
                        {
                            if (finishFrame == 0)
                            {
                                tObjectState = TimeObjectState.Present;
                                TrackFrame();
                            }
                            else
                            {
                                tObjectState = TimeObjectState.PastFinished;
                                OnFinishPlayback();
                            }
                            break;
                        }

                        Playback();

                        break;

                    case TimeObjectState.PastFinished:

                        break;
                }

                break;

            case TimeState.Backward:

                switch (tObjectState)
                {
                    case TimeObjectState.PastStart:
                        break;

                    case TimeObjectState.PastPlaying:

                        if (Game.t <= startFrame)
                        {
                            tObjectState = TimeObjectState.PastStart;

                            OnFinishReverse();

                            break;
                        }

                        Playback();

                        break;

                    case TimeObjectState.PastFinished:

                        if (Game.t <= finishFrame || (finishFrame == 0 && Game.t <= bFrames[bFrames.Count - 1].timeStamp))
                        {
                            tObjectState = TimeObjectState.PastPlaying;
                            //Just in case a frame or to is skipped we will attempt to 
                            //keep object in sync by subtracting the difference between their finish frame and current game time
                            //- (finishFrame - Game.t)
                            currentFrame = (bFrames.Count - 1);
                            OnStartReverse();
                        }
                        break;
                }

                break;
        }
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (Game.debugText)
        {
            Handles.Label(transform.position, "Time State: " + tObjectState.ToString() +
                                "\nTotal Frames: " + TotalFrames +
                                "\nCurrent Frame: " + currentFrame +
                                "\nStart Frame: " + startFrame +
                                "\nFinish Frame: " + finishFrame);
        }
        //else
        //{
        //    debugText.gameObject.SetActive(false);
        //}
    }

#endif

    protected override void PlayFrame()
    {
        base.PlayFrame();

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

    protected override void TrackFrame()
    {
        base.TrackFrame();

        tempSSFrame = new SpawnableSpriteFrameData()
        {
            active = gameObject.activeSelf,
        };

        sSFrames.Add(tempSSFrame);
    }

    protected override void OnFinishReverse()
    {
        base.OnFinishReverse();
        m_Sprite.enabled = false;
    }

    protected override void OnStartReverse()
    {
        base.OnStartReverse();
        //m_Sprite.enabled = true;
    }

    protected void SetActive(bool active)
    {
        GetComponent<SpriteRenderer>().enabled = active;

        if (GetComponent<Collider2D>())
            GetComponent<Collider2D>().enabled = active;

        if (GetComponent<Rigidbody2D>())
            GetComponent<Rigidbody2D>().simulated = active;
    }

    protected override void OnFinishPlayback()
    {
        base.OnFinishPlayback();

        m_Sprite.enabled = false;
    }

    protected override void OnPast()
    {
        tObjectState = TimeObjectState.PastFinished;
        base.OnPast();
    }

    public void SetMartyPoint()
    {
        //deathOrMarty = false;
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
}
