using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableSpriteTimeObject : BaseTimeObject<SpawnableFrameData>
{

    protected override void Start()
    {
        base.Start();

        TimeObjectManager.vSpawnable.Add(this);
    }

    protected override void PlayFrame()
    {
        //Debug.Log(currentFrame);

        transform.position = frames[currentFrame].m_Position;
        transform.rotation = frames[currentFrame].m_Rotation;

        GetComponent<SpriteRenderer>().color = frames[currentFrame].color;
        GetComponent<SpriteRenderer>().enabled = frames[currentFrame].active;
        GetComponent<Collider2D>().enabled = frames[currentFrame].active;

        if(GetComponent<Rigidbody2D>())
            GetComponent<Rigidbody2D>().simulated = frames[currentFrame].active;

        gameObject.SetActive(frames[currentFrame].enabled);

        currentFrame += (int)Game.timeState;
    }

    protected override void TrackFrame()
    {
        tempFrame = new SpawnableFrameData();

        tempFrame.m_Position = transform.position;
        tempFrame.m_Rotation = transform.rotation;

        tempFrame.color = GetComponent<SpriteRenderer>().color;
        tempFrame.active = GetComponent<SpriteRenderer>().enabled;

        tempFrame.timeStamp = Game.t;

        tempFrame.enabled = gameObject.activeSelf;

        frames.Add(tempFrame);
    }

    protected override void OnFinishReverse()
    {
        SetActive(false);
    }

    protected override void OnStartReverse()
    {
        SetActive(true);
    }

    protected void SetActive(bool active)
    {
        GetComponent<SpriteRenderer>().enabled = active;
        GetComponent<Collider2D>().enabled = active;
        if(GetComponent<Rigidbody2D>())
            GetComponent<Rigidbody2D>().simulated = active;
    }
}
