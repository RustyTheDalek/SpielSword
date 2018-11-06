﻿using UnityEngine;
using System.Collections;

public class MeleeAttack : MonoBehaviour
{
    int _Damage = 3;
    public int damageMult = 1;
    public AudioSource EffectNoise;

    public int Damage
    {
        get
        {
            return _Damage * damageMult;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (TimeObjectManager.timeState == TimeState.Forward)
        {
            if (collision.name == "Head" && collision.GetComponent<Head>())
            {
                collision.GetComponent<Head>().OnHit(Damage);
            }
        }
    }

    public void PlayEffect()
    {
        EffectNoise.Stop();
        EffectNoise.Play();
    }
}
