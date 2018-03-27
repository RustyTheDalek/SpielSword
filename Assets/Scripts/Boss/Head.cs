using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Script for registering hit to the boss' head
/// </summary>
public class Head : MonoBehaviour {

    public Sprite originalHead;
    public Sprite damageHead;

    Timer damageTimer;
	// Use this for initialization
	void Start ()
    {
        damageTimer = gameObject.AddComponent<Timer>();
        damageTimer.Setup("Damager", .25f, true);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(damageTimer.complete)
        {
            GetComponent<SpriteRenderer>().sprite = originalHead;
        }
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.transform.name.Contains("Range") && !Game.StageMetEarly)
        {
            OnHit(1);
        }
    }

    public void OnHit(float damageMultiplier)
    {
        Debug.Log("Boss took " + 1 * damageMultiplier + " Damage!");
        Golem.health -= 1 * damageMultiplier;
        GetComponent<SpriteRenderer>().sprite = damageHead;
        damageTimer.StartTimer();
    }
}
