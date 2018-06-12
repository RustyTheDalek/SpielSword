using UnityEngine;

/// <summary>
/// Script for registering hits to the boss' head
/// TODO: Change this to be more generic when new bosses are introduced
/// Created on : Ian Jones      - 18/08/16
/// Updated by : Ian Jones      - 01/04/18
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
        if (LayerMask.LayerToName(coll.gameObject.layer) == "Weapon")
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
