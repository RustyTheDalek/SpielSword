using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableSpriteTimeObject : BaseTimeObject<SpawnableFrameData>
{

    protected override void Start()
    {
        base.Start();

        tObjectState = TimeObjectState.Present;

        TimeObjectManager.vSpawnable.Add(this);
    }

    protected override void Update()
    {
        switch (Game.timeState)
        {
            case TimeState.Forward:

                switch (tObjectState)
                {
                    case TimeObjectState.Present:

                        if (GetComponent<SpriteRenderer>().enabled)
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
                            (finishFrame == 0 && Game.t >= frames[frames.Count - 1].timeStamp))
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

                        if (Game.t <= finishFrame || (finishFrame == 0 && Game.t <= frames[frames.Count-1].timeStamp))
                        {
                            tObjectState = TimeObjectState.PastPlaying;
                            //Just in case a frame or to is skipped we will attempt to 
                            //keep object in sync by subtracting the difference between their finish frame and current game time
                            //- (finishFrame - Game.t)
                            currentFrame = (frames.Count - 1);
                            OnStartReverse();
                        }
                        break;
                }

                break;
        }

#if UNITY_EDITOR

        if (debugText && frames != null)
        {
            if (Game.debugText)
            {
                debugText.text = "Time State: " + tObjectState.ToString() +
                                    "\nTotal Frames: " + TotalFrames +
                                    "\nCurrent Frame: " + currentFrame +
                                    "\nStart Frame: " + startFrame +
                                    "\nFinish Frame: " + finishFrame;
            }
            else
            {
                debugText.gameObject.SetActive(false);
            }
        }
#endif
    }

    protected override void PlayFrame()
    {
        //Debug.Log(currentFrame);

        transform.position = frames[currentFrame].m_Position;
        transform.rotation = frames[currentFrame].m_Rotation;

        GetComponent<SpriteRenderer>().color = frames[currentFrame].color;
        GetComponent<SpriteRenderer>().enabled = frames[currentFrame].active;

        if(GetComponent<Collider2D>())
            GetComponent<Collider2D>().enabled = frames[currentFrame].active;

        if(GetComponent<Rigidbody2D>())
            GetComponent<Rigidbody2D>().simulated = frames[currentFrame].active;

        gameObject.SetActive(frames[currentFrame].enabled);

        currentFrame += Game.GameScale;
    }

    protected override void TrackFrame()
    {
        tempFrame = new SpawnableFrameData()
        {
            m_Position = transform.position,
            m_Rotation = transform.rotation,

            color = GetComponent<SpriteRenderer>().color,
            active = GetComponent<SpriteRenderer>().enabled,

            timeStamp = Game.t,

            enabled = gameObject.activeSelf,
        };

        frames.Add(tempFrame);
    }

    protected override void OnFinishReverse()
    {
        base.OnStartReverse();
        SetActive(false);
    }

    protected override void OnStartReverse()
    {
        Debug.Log("am here");
        base.OnStartReverse();
        SetActive(true);
    }

    protected void SetActive(bool active)
    {
        GetComponent<SpriteRenderer>().enabled = active;

        if(GetComponent<Collider2D>())
            GetComponent<Collider2D>().enabled = active;

        if(GetComponent<Rigidbody2D>())
            GetComponent<Rigidbody2D>().simulated = active;
    }

    protected override void OnPast()
    {
        tObjectState = TimeObjectState.PastFinished;
        base.OnPast();
    }
}
