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

    /// <summary>
    /// Special time in which Villagers "Finish" Dying.
    /// </summary>
    public int reverseDeathTimeStamp = 0;

    private void Awake()
    {
        m_Character = GetComponent<PlatformerCharacter2D>();
    }

    int t
    {
        get
        {
            return (int)Game.t;
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
                animData.move = actions[t].move;
                animData.jump = actions[t].jump;
                animData.attack = actions[t].attack;
                animData.shieldSpecial = actions[t].special;
                animData.canSpecial = actions[t].canSpecial;
                animData.dead = actions[t].dead;
            }
            else if (t == actions.Count)
            {
                animData.move = 0;
                animData.jump = false;
                animData.attack = false;
                animData.dead = false;
            }
            else if (Game.timeState == TimeState.Backward)
            {
                if (reverseDeathTimeStamp != 0 &&
                    reverseDeathTimeStamp == Game.t)
                {
                    //Debug.Break();
                    Debug.Log("Villager Un-Dying");
                    GetComponent<Animator>().SetTrigger("ExitDeath");
                }
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
                m_Character.Move(animData);
            }
            else if (t == actions.Count)
            {
                m_Character.Move(animData);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<Attack>() && !animData.dead && Game.timeState == TimeState.Forward)
        {
            Debug.Log("Past Villager Hit By Boss Attack");
            animData.dead = true;
            m_Character.Move(animData);

            GetComponentInChildren<ParticleSystem>().Play();
        }
    }

    public void OnCollisionEnter2D()
    {
        Debug.Log("Past Villager Hit Collision");
    }

}
