using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Script for registering hit to the boss' head
/// </summary>
public class Head : MonoBehaviour {

    public BossManager boss;

    public Sprite originalHead;
    public Sprite damageHead;

    Timer damageTimer;
	// Use this for initialization
	void Start ()
    {
        damageTimer = gameObject.AddComponent<Timer>();
        damageTimer.Setup("Damager", .25f, true);

        boss = GetComponentInParent<BossManager>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(damageTimer.complete)
        {
            GetComponent<SpriteRenderer>().sprite = originalHead;
        }
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        //Debug.Log("Collision");

        if (coll.transform.name == "Melee" && boss.attackable)
        {
            //Debug.Log("Attack Succesful");
            Golem.health--;
            GetComponent<SpriteRenderer>().sprite = damageHead;
            damageTimer.StartTimer();
        }
        else
        {
            //Debug.Log("Collision with" + coll.name);
        }
    }
}
