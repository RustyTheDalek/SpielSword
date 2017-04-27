﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectTracking : MonoBehaviour {

    public SpriteRenderer m_Sprite;
    Color spriteColor;

    BossFrame currentBossFrame;
    List<BossFrame> bossFrames = new List<BossFrame>();

    public int frames;

    int t
    {
        get
        {
            return (int)Game.t;
        }
    }

    public void Awake()
    {
        m_Sprite = GetComponent<SpriteRenderer>();
    }

    public void LateUpdate()
    {
        switch (Game.timeState)
        {
            case TimeState.Forward:

                TrackFrame();

                break;

            case TimeState.Backward:

                PlayFrame();

                break;
        }

        frames = bossFrames.Count;
    }

    public void TrackFrame()
    {
        currentBossFrame = new BossFrame();

        currentBossFrame.m_Position = gameObject.transform.position;
        currentBossFrame.m_Rotation = gameObject.transform.rotation;
        currentBossFrame.color = m_Sprite.color;

        bossFrames.Add(currentBossFrame);
    }

    public void PlayFrame()
    {
        if (bossFrames != null)
        {
            if (t < bossFrames.Count && t >= 0)
            {
                //Debug.Log("playing frame");

                //Debug.Log(transform.position);
                gameObject.transform.position = bossFrames[t].m_Position;
                gameObject.transform.rotation = bossFrames[t].m_Rotation;

                spriteColor = m_Sprite.color;

                m_Sprite.color = bossFrames[t].color;
            }
        }
        else
        {
            Debug.LogWarning("Frames are null, no movmement to play back");
        }
    }

    public void Reset()
    {
        bossFrames.Clear();
    }

    //void OnDrawGizmos()
    //{
    //    if (bossFrames != null && t >= 0 && t < bossFrames.Count)
    //    {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawWireSphere(bossFrames[t].m_Transform.position, 1);
    //    }
    //}
}