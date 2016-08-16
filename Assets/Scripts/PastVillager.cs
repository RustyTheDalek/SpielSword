using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets._2D;

/// <summary>
/// Script that handles past versions of the Player
/// </summary>
public class PastVillager : MonoBehaviour {

    private PlatformerCharacter2D m_Character;

    public List<Action> actions;
    public int size;

    VillagerAnimData animData;

    private void Awake()
    {
        m_Character = GetComponent<PlatformerCharacter2D>();
    }

    int t
    {
        get
        {
            return (int)VillagerManager.t;
        }
    }

    // Use this for initialization
    void Start () {
	
	}

    public void Setup(List<Action> _Actions)
    {
        actions = new List<Action>();
        actions.AddRange(_Actions);
        size = actions.Count;
    }
	
	// Update is called once per frame
	void Update ()
    {     
        if (actions != null)
        {
            //So long as T is within Range
            if (t < actions.Count && t >= 0)
            {
                //Set new position and adjust for Time Scale
                GetComponent<Rigidbody2D>().transform.position = actions[t].pos;
            }
        }
    }

    void FixedUpdate()
    {
        if (actions != null)
        {
            if (t < actions.Count &&
                t >= 0)
            {
                animData.move = actions[t].move;
                animData.jump = actions[t].jump;
                animData.attack = actions[t].attack;
                animData.dead = actions[t].dead;
                m_Character.Move(animData);
            }
            else if (t >= actions.Count)
            {
                animData.move = 0;
                animData.jump = false;
                animData.attack = false;
                animData.dead = true;
                m_Character.Move(animData);
            }
        }
    }

}
