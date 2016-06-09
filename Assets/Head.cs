using UnityEngine;
using System.Collections;

public class Head : MonoBehaviour {

    public Golem boss;

    public Sprite originalHead;
    public Sprite damageHead;

    Timer damageTimer;
	// Use this for initialization
	void Start () {

        damageTimer = gameObject.AddComponent<Timer>();
        damageTimer.Setup("Damager", .25f, true);
	
	}
	
	// Update is called once per frame
	void Update () {

        if(damageTimer.complete)
        {
            GetComponent<SpriteRenderer>().sprite = originalHead;
        }
	
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        Debug.Log("Collision");
        if(coll.transform.name == "Melee")
        {
            Golem.health--;
            GetComponent<SpriteRenderer>().sprite = damageHead;
            damageTimer.StartTimer();
        }
    }
}
