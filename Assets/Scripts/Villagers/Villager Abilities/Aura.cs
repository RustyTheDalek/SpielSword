using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aura : SpawnableSpriteTimeObject
{
    public float health = 4;

    protected bool auraActive = false;

    public float auraLife  = 5,
                    auraTimer = 0;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        auraActive = true;
        auraTimer = auraLife;

        OnPlayFrame += PlayAuraFrame;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (auraTimer > 0 && (tObjectState == TimeObjectState.Present || tObjectState == TimeObjectState.PastPlaying))
        {
            auraTimer -= Time.deltaTime;
        }
        else if (auraTimer <= 0 && auraActive)
        {
            SetActive(false);

            finishFrame = Game.t;

            auraActive = false;
        }

        base.Update();
    }

    protected void PlayAuraFrame()
    {
        if (Tools.WithinRange(currentFrame, sSFrames))
        {
            GetComponent<SpriteRenderer>().enabled = sSFrames[currentFrame].active;

            if (GetComponent<Collider2D>())
                GetComponent<Collider2D>().enabled = sSFrames[currentFrame].active;

            if (GetComponent<Rigidbody2D>())
                GetComponent<Rigidbody2D>().simulated = sSFrames[currentFrame].active;

            auraActive = sSFrames[currentFrame].active;
        }
    }

    public void DecreaseStrength()
    {
        if (health > 0)
        {
            health--;

            Color col = GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color = new Color(col.r, col.g, col.b, health / 4);
        }
    }
}
