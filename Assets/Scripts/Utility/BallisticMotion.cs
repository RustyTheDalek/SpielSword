﻿// LICENSE
//
//   This software is dual-licensed to the public domain and under the following
//   license: you are granted a perpetual, irrevocable license to copy, modify,
//   publish, and distribute this file as you see fit.

using UnityEngine;
using System.Collections;

public class BallisticMotion : MonoBehaviour {

    // Private fields
    Vector3 lastPos;
    Vector3 impulse;
    float gravity;

    public Sprite[] m_SpriteVariants;

    SpriteRenderer m_Sprite;


    // Methods
    void Awake() {
        // Keep scene heirarchy clean
        //transform.parent = GameObject.FindGameObjectWithTag("Projectiles").transform;
        m_Sprite = GetComponent<SpriteRenderer>();
    }

    public void Initialize(Vector3 pos, float gravity) {
        transform.position = pos;
        lastPos = transform.position;
        this.gravity = gravity;

        m_Sprite.sprite = m_SpriteVariants[Random.Range(0, m_SpriteVariants.Length - 1)];
    }

	void FixedUpdate () {
        // Simple verlet integration
        float dt = Time.fixedDeltaTime;
        Vector3 accel = -gravity * Vector3.up;

        Vector3 curPos = transform.position;
        Vector3 newPos = curPos + (curPos-lastPos) + impulse*dt + accel*dt*dt;
        lastPos = curPos;
        transform.position = newPos;
        transform.right = newPos - lastPos;

        impulse = Vector3.zero;
	}

    public void AddImpulse(Vector3 impulse) {
        this.impulse += impulse;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            this.enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
