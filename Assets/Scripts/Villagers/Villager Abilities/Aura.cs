using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aura : SpawnableSpriteTimeObject
{
    public float health = 4;

    protected bool auraActive = false;

    public float auraLife  = 5,
                    auraTimer = 0;

    public bool comboUsed = false;

    public event TimeObjectEvent OnEnterAuraEvent;

    protected override void Awake()
    {
        base.Awake();

        OnPlayFrame -= PlaySpriteFrame;
        OnTrackFrame -= TrackSpriteFrame;
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        auraActive = true;
        auraTimer = auraLife;

        OnPlayFrame += PlayAuraFrame;

        OnStartPlayback += DecreaseStrength;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (auraTimer > 0 && (tObjectState == TimeObjectState.Present || 
            tObjectState == TimeObjectState.PastPlaying))
        {
            auraTimer -= Time.deltaTime;
        }
        else if (auraTimer <= 0 && auraActive)
        {
            SetActive(false);

            if (finishFrame == 0)
            {
                finishFrame = (int)TimeObjectManager.t;
            }

            auraActive = false;
        }

        base.Update();
    }

    protected void PlayAuraFrame()
    {
        if (sSFrames.WithinRange(currentFrame))
        {
            if (GetComponent<Collider2D>())
                GetComponent<Collider2D>().enabled = sSFrames[(int)currentFrame].active;

            if (GetComponent<Rigidbody2D>())
                GetComponent<Rigidbody2D>().simulated = sSFrames[(int)currentFrame].active;

            auraActive = sSFrames[(int)currentFrame].active;
        }
    }

    public void DecreaseStrength()
    {
        if (health > 0)
        {
            health--;

            Color col = m_Sprite.color;
            m_Sprite.color = new Color(col.r, col.g, col.b, health / 4);
        }
        else
        {
            OnStartPlayback -= DecreaseStrength;
        }
    }

    protected virtual void OnEnterAura(Collider2D coll)
    {
        if (coll.gameObject != creator.gameObject)
        {
            if (!comboUsed)
            {
                comboUsed = true;

                if (OnEnterAuraEvent != null)
                {
                    OnEnterAuraEvent();
                }
                else
                {
                    Debug.LogWarning("No event tied");
                }
            }
        }
    }

    protected virtual void OnExitAura(Collider2D coll) { }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (auraActive && coll.GetComponent<Villager>())
        {
            Villager temp = coll.GetComponent<Villager>();

            if (temp.CurrentVillager)
            {
                OnEnterAura(coll);
            }
        }
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.GetComponent<Villager>())
        {
            Villager temp = coll.GetComponent<Villager>();

            if (temp.CurrentVillager && auraActive)
            {
                OnEnterAura(coll);
            }
            else
            {
                OnExitAura(coll);
            }
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.GetComponent<Villager>())
        {
            Villager temp = coll.GetComponent<Villager>();

            if (temp.CurrentVillager)
            {
                OnExitAura(coll);
            }
        }
    }

}
