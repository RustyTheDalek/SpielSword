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

        currentFrame += (int)Game.timeState;
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
        SetActive(false);
    }

    protected override void OnStartReverse()
    {
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
}
