using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableSpriteTimeObject : SpriteTimeObject
{
    protected SpawnableSpriteFrameData tempSSFrame;
    protected List<SpawnableSpriteFrameData> sSFrames = new List<SpawnableSpriteFrameData>();

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

                        if (Game.t <= startFrame || currentFrame < 0)
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

#if UNITY_EDITOR

        if (debugText && bFrames != null)
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
        base.PlayFrame();

        if (Tools.WithinRange(currentFrame, sSFrames))
        {
            gameObject.SetActive(sSFrames[currentFrame].active);
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
}
