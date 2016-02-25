using UnityEngine;
using System.Collections;

public struct Action
{
    public float timeStamp;

    //Movement variables
    public Vector3 pos;
    public float move;
    public bool attack;
    public bool dead;
    public bool jump;

    public float health;

    public void Reset()
    {
        pos = Vector3.zero;
        move = 0;
        attack = true;
        dead = true;
        jump = false;
        health = 0;
        timeStamp = 0;
    }
}